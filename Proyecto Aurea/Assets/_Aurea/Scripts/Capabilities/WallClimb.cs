/* 
@Author: Christian Matos
@Date: 2023-06-28 16:17:05
@Last Modified by: Christian Matos
@Last Modified Date: 2023-06-28 20:47

* Functionality: Handles wall climbing and sliding.
* Approach: Checks if the player is on a wall and applies a force to the opposite direction of the wall.
* To Use: Attach to a player object.
* Dependencies: Rigidbody2D, CollisionCheck
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WallClimb : MonoBehaviour
{
    // Wall interaction settings
    [SerializeField, Range(0.1f, 5f)] private float maxWallGrabTime = 2f;
    [SerializeField, Range(0.1f, 5f)] private float wallSlideMaxSpeed = 2f;
    [SerializeField] private Vector2 wallJumpClimb = new Vector2(4f, 12f);
    [SerializeField] private Vector2 wallJumpBounce = new Vector2(10.7f, 10f);
    [SerializeField] private Vector2 wallJumpLeap = new Vector2(14f, 12f);
    [SerializeField, Range(0.05f, 0.5f)] private float wallStickTime = 0.25f;

    // Public variables
    public bool wallJumping { get; private set; }

    // Components
    private CollisionCheck _collisionCheck;
    private Rigidbody2D _rigidbody2D;
    private Move _move;
    private Jump _jump;

    // Private variables
    private Vector2 velocity;
    private bool onWall;
    private bool onGround;
    private bool pressingGrab;
    private bool desiredJump;

    private bool grabbingWall;
    private bool onLeftWall; // 1 = left, 0 = right
    // private bool isJumpReset;

    private Vector2 inputDirection;
    private float wallDirectionX;
    private float wallStickCounter;
    private float currentWallGrabTime;

    public void OnGrab(InputAction.CallbackContext context)
    {
        // Depending on the context, the class will either start or stop the grab through the pressingGrab variable.
        // pressingGrab = context.ReadValueAsButton();

        if (context.started)
            pressingGrab = true;
        if (context.canceled)
            pressingGrab = false;
    }

    void Awake()
    {
        // Get components
        _collisionCheck = GetComponent<CollisionCheck>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _move = GetComponent<Move>();
        _jump = GetComponent<Jump>();

        // isJumpReset = true;
    }

    private void FixedUpdate()
    {
        onWall = _collisionCheck.onWall;

        if (onWall)
        {
            // Get current state
            // velocity = _rigidbody2D.velocity;
            // onGround = _collisionCheck.onGround;
            onLeftWall = (_collisionCheck.contactNormal.x == 1) ? true : false;

            // Wall behavior (mostly changes to velocity)
            WallGrab();



            // Set new state
            _rigidbody2D.velocity = velocity;
        }


    }

    private void Update()
    {
        // Reset wall grab time
        if (_jump.onGround)
            currentWallGrabTime = 0;
    }

    private void WallGrab()
    {
        if (pressingGrab && currentWallGrabTime < maxWallGrabTime)
        {
            Debug.Log("Grabbing wall");
            grabbingWall = true;
            velocity.y = 0f;
            _rigidbody2D.gravityScale = 0f;
            currentWallGrabTime += Time.deltaTime;
        }
        else
            grabbingWall = false;
    }

    // // Slide downwards on wall
    // private void WallSlide()
    // {
    //     if (velocity.y < wallSlideMaxSpeed)
    //         velocity.y = -wallSlideMaxSpeed;
    // }

    // private void WallStick()
    // {
    //     if (!onGround && !wallJumping)
    //         if (wallStickCounter > 0)
    //         {
    //             velocity.x = 0;

    //             if (inputDirection.y != 0 && Mathf.Sign(inputDirection.y) == Mathf.Sign(_collisionCheck.contactNormal.x))
    //                 wallStickCounter -= Time.deltaTime;
    //             else
    //                 wallStickCounter = wallStickTime;
    //         }
    //         else
    //             wallStickCounter = wallStickTime;
    // }

    // private void WallJump()
    // {
    //     if ((onWall && velocity.x == 0) || onGround)
    //     {
    //         wallJumping = false;
    //     }

    //     if (onWall && !onGround)
    //     {
    //         if (desiredJump && isJumpReset)
    //         {
    //             if (inputDirection == 0)
    //             {
    //                 velocity = new Vector2(wallDirectionX * wallJumpBounce.x, wallJumpBounce.y);
    //                 wallJumping = true;
    //                 desiredJump = false;
    //                 isJumpReset = false;
    //             }
    //             else if (Mathf.Sign(-wallDirectionX) == Mathf.Sign(inputDirection))
    //             {
    //                 velocity = new Vector2(wallJumpClimb.x * wallDirectionX, wallJumpClimb.y);
    //                 wallJumping = true;
    //                 desiredJump = false;
    //                 isJumpReset = false;
    //             }
    //             else
    //             {
    //                 velocity = new Vector2(wallJumpLeap.x * wallDirectionX, wallJumpLeap.y);
    //                 wallJumping = true;
    //                 desiredJump = false;
    //                 isJumpReset = false;
    //             }
    //         }
    //         else if (!desiredJump)
    //         {
    //             isJumpReset = true;
    //         }
    //     }
    // }

    private void OnCollisionEnter2D(Collision2D other) 
    {
        // Update collision state
        // isJumpReset = false;

        // Stop all movement on wall collision mid-wall jump
        // if (onWall && !onGround && wallJumping)
        //     _rigidbody2D.velocity = Vector2.zero;
    }
}
