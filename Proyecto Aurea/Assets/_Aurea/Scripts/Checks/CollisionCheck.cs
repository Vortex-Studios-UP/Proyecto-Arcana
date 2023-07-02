/* 
@Author: Christian Matos
@Date: 2023-06-27 15:22:55
@Last Modified by: Christian Matos
@Last Modified Date: 2023-06-28 19:56:05

* Functionality: Check if the object is grounded and retrieve the friction of the ground.
* Approach: 
    * Determines if the object is grounded or on wall by checking the normal of the collision. 
    * Retrieves the friction of the ground from the collider material.
* To Use: Attach to a character object.
* Dependencies: Collider2D
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]

public class CollisionCheck : MonoBehaviour
{
    // Public variables
    public bool onGround { get; private set; } // Is the object grounded?
    public bool onWall { get; private set; } // Is the object on a wall?
    public float friction { get; private set; } // Friction of the ground.
    public Vector2 contactNormal { get; private set; } // Normal of the collision.

    // Private variables
    [SerializeField] private LayerMask groundLayer; // Layer of the ground.
    private PhysicsMaterial2D material { get; set; } // Material of the collider.

    // Update variables according collisions with the object.
    public void EvaluateCollision(Collision2D collision)
    {
        for (int i = 0; i < collision.contactCount; i++)
        {
            contactNormal = collision.GetContact(i).normal;
            onGround |= contactNormal.y > 0.5f;
            onWall = Mathf.Abs(contactNormal.x) >= 0.9f;
        }
    }

    // Check grounded and friction on collision.
    private void OnCollisionEnter2D(Collision2D other) 
    {
        EvaluateCollision(other);
        RetrieveFriction(other);

        if (other.gameObject.layer == groundLayer)
        {
            
        }
    }

    private void OnCollisionStay2D(Collision2D other) 
    {
        EvaluateCollision(other);
        RetrieveFriction(other);

        if (other.gameObject.layer == groundLayer)
        {

        }
    }

    // Not grounded 
    private void OnCollisionExit2D(Collision2D other) 
    {
        onGround = false;
        onWall = false;
        friction = 0;
    }

    private void RetrieveFriction(Collision2D collision)
    {
        PhysicsMaterial2D material = collision.collider.sharedMaterial;

        friction = 0;

        if (material != null)
            friction = material.friction;
    }
}
