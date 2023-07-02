/* 
@Author: Christian Matos
@Date: 2023-06-28 14:29:58
@Last Modified by: Christian Matos
@Last Modified Date: 2023-06-28 20:47

* Functionality: Handle jumping behavior.
* Approach: 
    * Different gravity multipliers are used on jump and fall.
    * Holding the jump button increases the jump height.
    * Coyote time allows the player to jump even if they are not on the ground for a short period of time.
    * Jump buffer allows the player to jump even if they press the button before landing.
* To Use: Attach to a player object. Suscribe the OnJump method to the jump input action.
* Dependencies: Rigidbody2D, CollisionCheck, InputController
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CollisionCheck))]
public class Jump : MonoBehaviour
{
    // Jump settings
    [Header("Jump Settings")]
    [SerializeField, Range(0f, 100f)] private float jumpHeight = 5.5f; // The force applied to the player when jumping
    [SerializeField, Range(0.3f, 1.25f)] public float timeToJumpApex = 0.5f; // The time it takes for the player to reach the apex of their jump.
    [SerializeField, Range(0, 5)] private int numberOfJumps = 0; // The maximum number of air jumps the player can perform.

    [Header("Gravity Multipliers")]
    [SerializeField, Range(0f, 5f)] private float jumpMultiplier = 1f; // Gravity multiplier applied to the player when jumping.
    [SerializeField, Range(0f, 5f)] private float fallMultiplier = 1f; // Gravity multiplier applied to the player when falling.
    [SerializeField, Range (0f, 5f)] public float jumpCutOffMultiplier = 1f; // Gravity multiplier when the player releases the jump button.
    [SerializeField, Range (1f, 5f)]private float defaultGravityScale = 1f;


    [Header("Player Commodities")]
    [SerializeField, Range(0f, 0.3f)] private float coyoteTime = 0.3f; // The time window in which the player can jump after leaving the ground.
    [SerializeField, Range(0f, 0.3f)] private float jumpBufferTime = 0.3f; // The time window in which the player can ask to jump before landing.
    [SerializeField] private float fallSpeedClamp = 50f; // The maximum speed the player can fall at.

    [Header("Class Communication")]
    public UnityEvent jumpEvent; // Event called when the player jumps.


    public bool onGround;
    public bool isJumping;
    public bool jumpRequested;


    // Components
    private Rigidbody2D _rigidbody2D;
    private CollisionCheck _collisionCheck;

    // Private variables
    private Vector2 velocity;
    private int currentJumpNumber;

    private float jumpSpeed;
    private float coyoteCounter;
    private float jumpBufferCounter;
    private float currentGravityMultiplier;
    private bool pressingJump;
    private bool canJumpAgain;
    private bool onWall;
    private bool onLeftWall;

    void Awake()
    {
        // Get components
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _collisionCheck = GetComponent<CollisionCheck>();

        // Reset the player's jump to the default state.
        currentGravityMultiplier = defaultGravityScale;
    }

    // Requests a jump on input (actual jumping is handled in FixedUpdate).
    public void OnJump(InputAction.CallbackContext context)
    {
        // Depending on the context, the class will either start or stop the jump through the pressingJump variable.
        if (context.started)
        {
            // jumpRequested lets the class know that the player wants to jump, buffering the jump if necessary.
            jumpRequested = true;
            pressingJump = true;
        }
        if (context.canceled)
            pressingJump = false;
    }

    // Update contains the physics-independent code, namely the jump buffer and coyote timers.
    void Update()
    {
        // Update the player's gravity based on the jump height, time to jump apex and current gravity multiplier.
        SetGravity();

        // Check if the player is on the ground, using the CollisionCheck component.
        onGround = _collisionCheck.onGround;
        onWall = _collisionCheck.onWall;

        // Buffer a jump request if the player presses the jump button before landing.
        if (jumpBufferTime > 0f)
            JumpBuffer();

        // Count down the ability to jump after leaving the ground.
        if (coyoteTime > 0f)
            CoyoteTime();
    }

    void FixedUpdate() 
    {
        // Get the player's current velocity from the Rigidbody2D component.
        velocity = _rigidbody2D.velocity;

        // Execute a jump on jump request.
        if (jumpRequested)
        {
            JumpAction();
            _rigidbody2D.velocity = velocity;

            // This return skips gravity calculations on the first frame of the jump to prevent bugs.
            return;
        }

        // Modify currentGravityMultiplier according to context.
        CalculateGravityMultiplier();
    }

    // Set the player's gravity based on the jump height, time to jump apex and current gravity multiplier.
    private void SetGravity()
    {
        // Player's gravity is determined by the formula g = (-2 * h) / (t^2)
        Vector2 newGravity = new Vector2(0, (-2 * jumpHeight) / (timeToJumpApex * timeToJumpApex));

        // currentGravityMultiplier stores the current gravity multiplier. It should be set to the jump multiplier when jumping and the fall multiplier when falling.
        _rigidbody2D.gravityScale = (newGravity.y / Physics2D.gravity.y) * currentGravityMultiplier;
    }

    // Buffer a jump if the player presses the jump button before landing.
    private void JumpBuffer()
    {
        if (!jumpRequested) return;

        // Starts the jump buffer counter when the player requests a jump.
        jumpBufferCounter += Time.deltaTime;

        // Resets the jump buffer counter and the jump request when the jump buffer time is reached.
        if (jumpBufferCounter > jumpBufferTime)
        {
            jumpRequested = false;
            jumpBufferCounter = 0f;
        }  
    }

    // Count down the ability to jump after leaving the ground.
    private void CoyoteTime()
    {
        // Not jumping and not on the ground means the player is in the coyote time window.
        if (!isJumping && !onGround)
            coyoteCounter += Time.deltaTime;
        else
            coyoteCounter = 0;
    }

    // Modify currentGravityMultiplier according to variable jump height.
    private void CalculateGravityMultiplier()
    {
        // Apply default gravity when grounded.
        if (onGround)
        {
            isJumping = false;
            currentGravityMultiplier = defaultGravityScale;
        }

        // When the player is going up, use the jump multiplier. When the jump is released, use the jump cut off multiplier.
        else if (_rigidbody2D.velocity.y > 0.01f)
            currentGravityMultiplier = (pressingJump && isJumping) ? jumpMultiplier : jumpCutOffMultiplier;

        // When the player is falling, use the fall multiplier.
        else if (_rigidbody2D.velocity.y < -0.01f)
            currentGravityMultiplier = fallMultiplier;

        // Clamp the player's fall speed.
        _rigidbody2D.velocity = new Vector2(velocity.x, Mathf.Clamp(velocity.y, -fallSpeedClamp, 100));
    }

    // Execute a jump using Unity's physics engine.
    private void JumpAction()
    {
        // The player can jump in one of three cases:
        // 1. They are on the ground.
        // 2. They are in the coyote time window.
        // 3. They have not reached the maximum number of jumps.
        // 4. They are on a wall.
        Debug.Log("onGround: " + onGround);
        Debug.Log("coyote: " + (coyoteCounter < coyoteTime));
        Debug.Log("jump: " + (canJumpAgain));
        Debug.Log("wall: " + onWall);

        // if (!onGround && coyoteCounter >= coyoteTime && !canJumpAgain && !onWall)
        if (!onGround && !canJumpAgain && !onWall)
            return;

        // Reset counters and assign flags.
        jumpRequested = false;
        jumpBufferCounter = 0f;
        isJumping = true;

        // Check if the player can jump again.
        canJumpAgain = (currentJumpNumber < numberOfJumps) ? true : false;

        // Determine the power of the jump, based on our gravity and stats.
        jumpSpeed = Mathf.Sqrt(-2f * Physics2D.gravity.y * _rigidbody2D.gravityScale * jumpHeight);

        // Keep the jump strength consistent if the player is jumping in mid-air.
        if (velocity.y > 0f)
            jumpSpeed = Mathf.Max(jumpSpeed - velocity.y, 0f);
        else if (velocity.y < 0f)
            jumpSpeed = Mathf.Abs(_rigidbody2D.velocity.y);

        // Apply the new jumpSpeed to the player's velocity. This is the jump.
        velocity.y += jumpSpeed;

        // Add horizontal force on wall jump.
        if (onWall)
            velocity = (new Vector2(-_collisionCheck.contactNormal.x, 1) * jumpSpeed).normalized;
        
        // Invoke the jump event for things like animations and particle effects.
        jumpEvent.Invoke();
    }
}
