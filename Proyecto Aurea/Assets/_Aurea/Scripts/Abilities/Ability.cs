/* 
@Author: Christian Matos
@Date: 2023-06-19 20:31:19
@Last Modified by: Christian Matos
@Last Modified Date: 2023-06-19 20:31:19

* Functionality:
* Approach:
* To Use:
* Dependencies:
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ability", menuName = "ScriptableObjects/Ability", order = 1)]
public class Ability : ScriptableObject
{
    // private float duration;

    [Header("Behavior")]
    public new string name;
    public AnimationClip animation;
    public InputCombination[] inputCombinations;


    [Header("Damage")]
    public bool causesDamage = false;
    [Tooltip("The force applied on the player in the input direction. Input direction by default.")]
    public InputDirection movementDirection;
    public float movementForce = 0;
    [Tooltip("The force and direction applied on the contacting entity. Neutral by default.")]
    public InputDirection launchDirection = InputDirection.Neutral;
    public float launchForce = 0;
    public float power = 0;
    public Stat multiplierStat = Stat.None;
    // public bool canStun;
    public ContactFilter2D contactFilter2D;

    public virtual void Awake() 
    {
        movementDirection = inputCombinations[0].inputDirection;
        // if (animation != null)
        //     duration = animation.length;
        
    }

    // Activates the ability
    public virtual void Activate(GameObject player) 
    {
        Debug.Log(name + " Ability Activated");

        // Execute animation
        if (animation != null)
            player.GetComponent<Animator>().Play(animation.name);

        if (causesDamage)
            Attack(player);
    }

    // Handles movement, enemy damage and knockback
    public virtual void Attack(GameObject player) 
    {
        // Empty array of colliders
        Collider2D[] hitColliders = new Collider2D[0];

        // Get all colliders that are hit by the hitbox and store them in hitColliders
        Collider2D playerHitboxCollider = player.GetComponentInChildren<BoxCollider2D>();
        Physics2D.OverlapCollider(playerHitboxCollider, contactFilter2D, hitColliders);

        // Apply movement force to player
        player.GetComponent<Rigidbody2D>().AddForce(DirectionToVector2(movementDirection) * movementForce, ForceMode2D.Impulse);

        // Apply launch force and damage to all hit entities
        foreach (Collider2D colliderObject in hitColliders)
        {
            int damage = (int)(1 * power * player.GetComponent<Stats>().GetStat(multiplierStat));
            colliderObject.GetComponent<Stats>().TakeDamage(damage);
            colliderObject.GetComponent<Rigidbody2D>().AddForce(DirectionToVector2(launchDirection) * launchForce, ForceMode2D.Impulse);
        }
    }

    private Vector2 DirectionToVector2(InputDirection direction)
    {
        switch (direction)
        {
            case InputDirection.Up:
                return Vector2.up;
            case InputDirection.UpSide:
                return new Vector2(1, 1).normalized;
            case InputDirection.Side:
                return Vector2.right;
            case InputDirection.DownSide:
                return new Vector2(1, -1).normalized;
            case InputDirection.Down:
                return Vector2.down;
            default:
                return Vector2.zero;
        }
    }
}

// Struct to store input combinations
[System.Serializable]
public struct InputCombination
{
    public InputDirection inputDirection;
    public InputButton inputButton;
    public PlayerState playerState;
}