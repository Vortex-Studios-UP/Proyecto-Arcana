/* 
@Author: Christian Matos
@Date: 2023-06-28 14:29:58
@Last Modified by: Christian Matos
@Last Modified Date: 2023-06-28 17:01:05

* Functionality: Handle jumping behavior.
* Approach: 
    * Gravity is modified when the player is falling or jumping.
    * Holding the jump button increases the jump height.
    * Coyote time allows the player to jump even if they are not on the ground for a short period of time.
    * Jump buffer allows the player to jump even if they press the button before landing.
* To Use: Attach to a player object.
* Dependencies: Rigidbody2D, CollisionCheck, InputController
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CollisionCheck))]
public class Jump : MonoBehaviour
{
    // Jump settings
    [SerializeField] private InputController _inputController;
    [SerializeField, Range(0f, 100f)] private float jumpHeight = 3f; // The force applied to the player when jumping
    [SerializeField, Range(0, 5)] private int maxAirJump = 0; // The maximum number of air jumps the player can perform
    [SerializeField, Range(0f, 5f)] private float fallMultiplier = 3f; // The multiplier applied to the player's gravity when falling.
    [SerializeField, Range(0f, 5f)] private float jumpMultiplier = 1.7f; // The multiplier applied to the player's gravity when jumping.
    [SerializeField, Range(0f, 0.3f)] private float coyoteTime = 0.2f; // The time window in which the player can jump after leaving the ground.
    [SerializeField, Range(0f, 0.3f)] private float jumpBufferTime = 0.1f; // The time window in which the player can ask to jump before landing.

    // Components
    private Rigidbody2D _rigidbody2D;
    private CollisionCheck _groundCheck;

    // Private variables
    private Vector2 velocity;
    private int jumpPhase;

    private float defaultGravityScale;
    private float jumpSpeed;
    private float coyoteCounter;
    private float jumpBufferCounter;

    private bool desiredJump;
    private bool onGround;
    private bool isJumping;

    void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _groundCheck = GetComponent<CollisionCheck>();

        defaultGravityScale = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        desiredJump |= _inputController.RetrieveJumpInput(this.gameObject);
    }

    void FixedUpdate() 
    {
        onGround = _groundCheck.onGround;
        velocity = _rigidbody2D.velocity;

        if (onGround && _rigidbody2D.velocity.y == 0f)
        {
            jumpPhase = 0;
            coyoteCounter = coyoteTime;
            isJumping = false;
        }
        else 
        {
            coyoteCounter -= Time.deltaTime;
        }
            
        
        if (desiredJump)
        {
            desiredJump = false;
            jumpBufferCounter = jumpBufferTime;
        }
        else if (!desiredJump && jumpBufferCounter > 0f)
        {
            jumpBufferCounter -= Time.deltaTime;
        }

        if (jumpBufferCounter > 0f)
            JumpAction();

        if (_inputController.RetrieveJumpHoldInput(this.gameObject) && _rigidbody2D.velocity.y > 0f)
            _rigidbody2D.gravityScale = defaultGravityScale * jumpMultiplier;
        else if (!_inputController.RetrieveJumpHoldInput(this.gameObject) || _rigidbody2D.velocity.y < 0f)
            _rigidbody2D.gravityScale = defaultGravityScale * fallMultiplier;
        else
            _rigidbody2D.gravityScale = defaultGravityScale;

        _rigidbody2D.velocity = velocity;
            
    }

    private void JumpAction()
    {
        if (coyoteCounter > 0f || (jumpPhase < maxAirJump && isJumping))
        {
            if (isJumping)
                jumpPhase += 1;

            jumpBufferCounter = 0f;
            coyoteCounter = 0f;
            jumpSpeed = Mathf.Sqrt(-2f * Physics2D.gravity.y * jumpHeight * jumpMultiplier);
            isJumping = true;


            if (velocity.y > 0f)
            {
                jumpSpeed = Mathf.Max(jumpSpeed - velocity.y, 0f);
            }
            velocity.y += jumpSpeed;
        }
    }
}
