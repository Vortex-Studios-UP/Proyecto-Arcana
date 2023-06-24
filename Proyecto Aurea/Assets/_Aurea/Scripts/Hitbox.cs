/* 
@Author: Christian Matos
@Date: 2023-06-19 20:15:25
@Last Modified by: Christian Matos
@Last Modified Date: 2023-06-19 20:15:25

* Functionality:
* Approach:
* To Use:
* Dependencies:
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    private BoxCollider2D _boxCollider2d;

    private void Awake()
    {
        _boxCollider2d = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Hit Enemy with damage");
        }
    }
}
