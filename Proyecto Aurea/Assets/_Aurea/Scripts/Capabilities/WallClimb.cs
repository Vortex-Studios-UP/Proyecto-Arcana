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

public class WallClimb : MonoBehaviour
{
    // Wall interaction settings
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
    public InputController _inputController;

    // Private variables
    private Vector2 velocity;
    private bool onWall;
    private bool onGround;
    private bool desiredJump;
    private bool isJumpReset;

    private float wallDirectionX;
    private float wallStickCounter;

    // Start is called before the first frame update
    void Awake()
    {
        _collisionCheck = GetComponent<CollisionCheck>();
        _rigidbody2D = GetComponent<Rigidbody2D>();

        isJumpReset = true;
    }

    private void FixedUpdate()
    {
        velocity = _rigidbody2D.velocity;
        onWall = _collisionCheck.onWall;
        onGround = _collisionCheck.onGround;
        wallDirectionX = _collisionCheck.contactNormal.x;

        WallSlide();
        WallStick();
        WallJump();

        _rigidbody2D.velocity = velocity;
    }

    private void Update()
    {
        desiredJump = _inputController.RetrieveJumpInput(this.gameObject);
    }

    private void OnCollisionEnter2D(Collision2D other) 
    {
        _collisionCheck.EvaluateCollision(other);
        isJumpReset = false;

        if (_collisionCheck.onWall && !_collisionCheck.onGround && wallJumping)
        {
            _rigidbody2D.velocity = Vector2.zero;
        }
    }

    private void WallSlide()
    {
        if (onWall)
        {
            if (velocity.y < wallSlideMaxSpeed)
                velocity.y = wallSlideMaxSpeed;
        }
    }

    private void WallStick()
    {
        if (_collisionCheck.onWall && !_collisionCheck.onGround && !wallJumping)
            if (wallStickCounter > 0)
            {
                velocity.x = 0;

                if (_inputController.RetrieveMoveInput(this.gameObject) != 0 && Mathf.Sign(_inputController.RetrieveMoveInput(this.gameObject)) == Mathf.Sign(_collisionCheck.contactNormal.x))
                {
                    wallStickCounter -= Time.deltaTime;
                }
                else
                {
                    wallStickCounter = wallStickTime;
                }
            }
            else
            {
                wallStickCounter = wallStickTime;
            }
    }

    private void WallJump()
    {
        if ((onWall && velocity.x == 0) || onGround)
        {
            wallJumping = false;
        }

        if (onWall && !onGround)
        {
            if (desiredJump && isJumpReset)
            {
                if (_inputController.RetrieveMoveInput(this.gameObject) == 0)
                {
                    velocity = new Vector2(wallDirectionX * wallJumpBounce.x, wallJumpBounce.y);
                    wallJumping = true;
                    desiredJump = false;
                    isJumpReset = false;
                }
                else if (Mathf.Sign(-wallDirectionX) == Mathf.Sign(_inputController.RetrieveMoveInput(this.gameObject)))
                {
                    velocity = new Vector2(wallJumpClimb.x * wallDirectionX, wallJumpClimb.y);
                    wallJumping = true;
                    desiredJump = false;
                    isJumpReset = false;
                }
                else
                {
                    velocity = new Vector2(wallJumpLeap.x * wallDirectionX, wallJumpLeap.y);
                    wallJumping = true;
                    desiredJump = false;
                    isJumpReset = false;
                }
            }
            else if (!desiredJump)
            {
                isJumpReset = true;
            }
        }
    }
}
