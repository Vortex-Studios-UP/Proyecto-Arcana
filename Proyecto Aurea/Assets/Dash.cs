/* 
@Author: Christian Matos
@Date: 2023-07-01 21:35:09
@Last Modified by: Christian Matos
@Last Modified Date: 2023-07-01 21:35:09

* Functionality: Applies a set force every fixed frame in the direction of the input.
* Approach: A coroutine is used to measure the time and a trail renderer to show the dash.
* To Use: Attach to a player object. Suscribe the OnDash method to the dash input action.
* Dependencies: Rigidbody2D, Move, TrailRenderer, CollisionCheck, SpriteRenderer, Input Package
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Move))]
[RequireComponent(typeof(TrailRenderer))]
[RequireComponent(typeof(CollisionCheck))]
[RequireComponent(typeof(SpriteRenderer))]

public class Dash : MonoBehaviour
{
    // Dash settings
    [Header("Dash Settings")]
    [SerializeField][Range(1f, 50f)] private float dashSpeed = 32f; // The force applied to the player when dashing.
    [SerializeField][Range(0.01f, 0.1f)] private float dashDuration = 0.05f; // The duration of the dash.

    [Header("Visuals")]
    [SerializeField] private float dashColorTime = 0.1f; // The duration of the dash color change.
    [SerializeField] private Color dashColor = Color.yellow; // The color of the dash trail.
    [SerializeField] private float cameraShakeIntensity = 1f; // The intensity of the camera shake.

    // Components
    private Rigidbody2D _rigidbody2D;
    private Move _move;
    private TrailRenderer _trailRenderer;
    private CollisionCheck _collisionCheck;
    private SpriteRenderer _spriteRenderer;

    // Public variables
    public bool isDashing { get; private set; }
    public bool canDash { get; set; }

    // Calculations
    private Vector2 direction; // Direction of movement from input

    // Input action
    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.performed && canDash)
            DashAction();
    }

    // Integrated functions
    private void Awake()
    {
        // Get components
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _move = GetComponent<Move>();
        _trailRenderer = GetComponent<TrailRenderer>();
        _collisionCheck = GetComponent<CollisionCheck>();
        _spriteRenderer = GetComponent<SpriteRenderer>();

        // Reset dash
        isDashing = false;
        canDash = true;
    }

    private void FixedUpdate()
    {
        // Resets the dash when the player lands.
        if (_collisionCheck.onGround)
            canDash = true;

        // Add continuous force while dashing.
        if (isDashing)
            _rigidbody2D.velocity = direction * dashSpeed;

        
    }

    private void DashAction()
    {
        direction = _move.direction;
        Debug.Log(direction);
        StartCoroutine(DashCoroutine(dashDuration));
    }

    private IEnumerator DashCoroutine(float time)
    {
        // Begin dash
        isDashing = true;
        canDash = false;
        _spriteRenderer.color = dashColor;
        CinemachineShake.Instance.ShakeCamera(cameraShakeIntensity, time);

        // Wait for dash to end
        yield return new WaitForSeconds(time);
        isDashing = false;

        // Reset dash color
        yield return new WaitForSeconds(dashColorTime);
        _spriteRenderer.color = Color.white;
    }

    
}
