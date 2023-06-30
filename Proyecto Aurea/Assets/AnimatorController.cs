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

public class AnimatorController : MonoBehaviour
{
    private Animator _animator;
    private Rigidbody2D _rigidbody2D;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Debug.Log(_rigidbody2D.velocity.x);
        _animator.SetFloat("speed", Mathf.Abs(_rigidbody2D.velocity.x));
        _animator.SetFloat("verticalVelocity",  _rigidbody2D.velocity.y);
    }
}
