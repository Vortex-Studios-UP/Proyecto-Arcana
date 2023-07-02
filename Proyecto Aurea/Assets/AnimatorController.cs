/* 
@Author: Christian Matos
@Date: 2023-06-29 14:55:56
@Last Modified by: Christian Matos
@Last Modified Date: 2023-06-29 14:55:56

* Functionality:
* Approach:
* To Use:
* Dependencies:
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AnimatorController : MonoBehaviour
{
    private Animator _animator;
    private Rigidbody2D _rigidbody2D;
    private CollisionCheck _collisionCheck;
    private Dash _dash;

    private Vector2 direction;

    public void OnMovement(InputAction.CallbackContext context)
    {
        direction.x = context.ReadValue<Vector2>().x;
    }

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _collisionCheck = GetComponent<CollisionCheck>();
        _dash = GetComponent<Dash>();
    }

    private void Update()
    {
        // Debug.Log(_rigidbody2D.velocity.x);
        _animator.SetFloat("speed", Mathf.Abs(_rigidbody2D.velocity.x));
        _animator.SetFloat("verticalVelocity",  _rigidbody2D.velocity.y);
        _animator.SetBool("onGround", _collisionCheck.onGround);
        _animator.SetBool("onWall", _collisionCheck.onWall);
        _animator.SetBool("isDashing", _dash.isDashing);

        TurnAroundAnimation();
    }

    private void TurnAroundAnimation()
    {
        if (Mathf.Sign(direction.x) != Mathf.Sign(_rigidbody2D.velocity.x))
            _animator.SetTrigger("turnAround");
        else    
            _animator.ResetTrigger("turnAround");
    }
}
