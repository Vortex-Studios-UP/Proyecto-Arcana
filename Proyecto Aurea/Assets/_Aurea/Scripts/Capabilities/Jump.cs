/* 
@Author: Christian Matos
@Date: 2023-06-28 14:29:58
@Last Modified by: Christian Matos
@Last Modified Date: 2023-06-28 14:29:58

* Functionality: Handle jumping behavior.
* Approach: Gravity is modified when the player is falling or jumping.
* To Use: Attach to a player object.
* Dependencies: Rigidbody2D component
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MonoBehaviour
{
    // Jump settings
    [SerializeField] private InputController _inputController;
    [SerializeField, Range(0f, 100f)] private float jumpHeight = 3f; // The force applied to the player when jumping
    [SerializeField, Range(0, 5)] private int maxAirJump = 0; // The maximum number of air jumps the player can perform
    [SerializeField, Range(0f, 5f)] private float fallMultiplier = 3f; // The multiplier applied to the player's gravity when falling.
    [SerializeField, Range(0f, 5f)] private float jumpMultiplier = 1.7f; // The multiplier applied to the player's gravity when jumping.
    [SerializeField] private LayerMask _groundLayer;

    // Components
    private Rigidbody2D _rigidbody2D;
    private GroundCheck _groundCheck;

    // Private variables
    private Vector2 velocity;
    private int jumpPhase;
    private float defaultGravityScale;
    private bool desiredJump;
    private bool onGround;

    void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _groundCheck = GetComponent<GroundCheck>();

        defaultGravityScale = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        desiredJump |= _inputController.RetrieveJumpInput();
    }

    void FixedUpdate() 
    {
        onGround = _groundCheck.GetGrounded;
        velocity = _rigidbody2D.velocity;

        if (onGround)
            jumpPhase = 0;
        
        if (desiredJump)
        {
            desiredJump = false;
            JumpAction();
        }

        if (_rigidbody2D.velocity.y > 0f)
            _rigidbody2D.gravityScale = defaultGravityScale * jumpMultiplier;
        else if (_rigidbody2D.velocity.y < 0f)
            _rigidbody2D.gravityScale = defaultGravityScale * fallMultiplier;
        else
            _rigidbody2D.gravityScale = defaultGravityScale;

        _rigidbody2D.velocity = velocity;
            
    }

    private void JumpAction()
    {
        if (onGround || jumpPhase < maxAirJump)
        {
            jumpPhase += 1;
            float jumpSpeed = Mathf.Sqrt(-2f * Physics2D.gravity.y * jumpHeight);
            if (velocity.y > 0f)
            {
                jumpSpeed = Mathf.Max(jumpSpeed - velocity.y, 0f);
            }
            velocity.y += jumpSpeed;
        }
    }
}
