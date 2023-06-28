/* 
@Author: Christian Matos
@Date: 2023-06-27 15:22:55
@Last Modified by: Christian Matos
@Last Modified Date: 2023-06-27 15:22:55

* Functionality: Check if the object is grounded and retrieve the friction of the ground.
* Approach: 
    * Determines if the object is grounded by checking the normal of the collision. 
    Retrieves the friction of the ground from the collider material.
* To Use: Attach to a character object.
* Dependencies: Collider2D
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]

public class GroundCheck : MonoBehaviour
{
    private bool onGround;
    private float friction;

    public bool GetGrounded { get => onGround; }
    public float GetFriction { get => friction; }

    // Check grounded and friction on collision.
    private void OnCollisionEnter2D(Collision2D other) 
    {
        EvaluateCollision(other);
        RetrieveFriction(other);
    }

    private void OnCollisionStay2D(Collision2D other) 
    {
        EvaluateCollision(other);
        RetrieveFriction(other);
    }

    // Not grounded 
    private void OnCollisionExit2D(Collision2D other) 
    {
        onGround = false;
        friction = 0;
    }

    private void EvaluateCollision(Collision2D collision)
    {
        for (int i = 0; i < collision.contactCount; i++)
        {
            Vector2 normal = collision.GetContact(i).normal;
            onGround |= normal.y > 0.5f;
        }
    }

    private void RetrieveFriction(Collision2D collision)
    {
        PhysicsMaterial2D material = collision.collider.sharedMaterial;

        friction = 0;

        if (material != null)
        {
            friction = material.friction;
        }
    }
}
