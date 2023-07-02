/* 
@Author: Christian Matos
@Date: 2023-06-27 15:39:17
@Last Modified by: Christian Matos
@Last Modified Date: 2023-06-28 20:47

* Functionality: Move a character.
* Approach: Use a Rigidbody2D to move the character with specific acceleration and speed.
* To Use: Attach to a character object.
* Dependencies: GroundCheck.cs, InputController.cs
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Move : MonoBehaviour
{
    // Movement settings
    [Header("Ground Movement Settings")]
    [SerializeField, Range(0f, 20f)] private float maxSpeed = 4f;
    [SerializeField, Range(0f, 100f)] private float maxAcceleration = 35f;
    [SerializeField, Range(0f, 100f)] private float maxDeceleration = 35f;
    [SerializeField, Range(0f, 100f)] private float turnSpeed = 35f;


    [Header("Air Movement Settings")]
    [SerializeField, Range(0f, 20f)] private float maxAirSpeed = 4f;
    [SerializeField, Range(0f, 100f)] private float maxAirAcceleration = 20f;
    [SerializeField, Range(0f, 100f)] private float maxAirDeceleration = 20f;
    [SerializeField, Range(0f, 100f)] private float airTurnSpeed = 35f;

    // Public variables
    public Vector2 direction { get; private set; } // Direction of movement from input

    // Private variables
    private Vector2 velocity; // Current velocity
    private Vector2 desiredVelocity;
    
    private float acceleration; // How fast to reach max speed
    private float deceleration; // How fast to reach 0 speed
    private float speed; // Current speed
    private float maxSpeedChange; // Maximum speed change per frame
    private bool onGround;
    private bool pressingKey;

    // Onject components
    private Rigidbody2D _rigidbody2D;
    private CollisionCheck _collisionCheck;
    private WallClimb _wallClimb;
    private SpriteRenderer _spriteRenderer;

    public void OnMovement(InputAction.CallbackContext context)
    {
        direction = context.ReadValue<Vector2>();
    }

    void Awake()
    {
        // Get components
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _collisionCheck = GetComponent<CollisionCheck>();
        _wallClimb = GetComponent<WallClimb>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        // Flip the character in the direction of movement
        if (direction.x != 0f && !_collisionCheck.onWall)
        {
            //_spriteRenderer.flipX = direction.x < 0f;
            transform.localScale = new Vector3(direction.x > 0 ? 1 : -1, 1, 1);
            pressingKey = true;
        }
        else
            pressingKey = false;

        // Set the desired velocity
        speed = onGround ? maxSpeed : maxAirSpeed;
        desiredVelocity = new Vector2(direction.x, 0f) * Mathf.Max(speed - _collisionCheck.friction, 0f);
    }

    private void FixedUpdate() 
    {
        // Get current state
        onGround = _collisionCheck.onGround;
        velocity = _rigidbody2D.velocity;

        // Set acceleration, decelartion and turn speed according to grounded state.
        MoveAction();
    }

    private void MoveAction()
    {
        // Set acceleration and speed according to grounded state
        acceleration = onGround ? maxAcceleration : maxAirAcceleration;
        deceleration = onGround ? maxDeceleration : maxAirDeceleration;
        turnSpeed = onGround ? turnSpeed : airTurnSpeed;

        // Calculate the maximum speed change. Use turn speed if changing direction, otherwise use acceleration.
        if (pressingKey)
        {
            if (Mathf.Sign(direction.x) != Mathf.Sign(velocity.x))
                maxSpeedChange = turnSpeed * Time.deltaTime;
            else
                maxSpeedChange = acceleration * Time.deltaTime;
        }
        // If not pressing a key, use deceleration.
        else 
            maxSpeedChange = deceleration * Time.deltaTime;

        // Set velocity to desired velocity at the rate of max speed change.
        velocity.x = Mathf.MoveTowards(velocity.x, desiredVelocity.x, maxSpeedChange);
        _rigidbody2D.velocity = velocity; 
    }
}
