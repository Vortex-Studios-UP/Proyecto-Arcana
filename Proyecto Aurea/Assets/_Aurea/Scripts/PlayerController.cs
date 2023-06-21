/* 
@Author: Christian Matos
@Date: 2023-06-18 10:41:42
@Last Modified by: Christian Matos
@Last Modified Date: 2023-06-18 10:41:42

* Functionality:
* Approach:
* To Use:
* Dependencies:
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Player Settings")]
    [SerializeField] private float speed = 0.2f;
    [SerializeField] private float jumpForce = 10f;
    [HideInInspector] public PlayerState currentPlayerState;

    // Player Components
    private Rigidbody2D _rigidbody2d;
    private PlayerInput _playerInput;
    private Animator _animator;
    private BoxCollider2D _boxCollider2d;
    public PlayerInputActions playerInputActions;

    // Private variables
    private bool isPaused = false;
    private Vector2 movementInput;


    private void Awake()
    {
        _rigidbody2d = GetComponent<Rigidbody2D>();
        _playerInput = GetComponent<PlayerInput>();
        _animator = GetComponent<Animator>();
        _boxCollider2d = GetComponent<BoxCollider2D>();
    
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();

        playerInputActions.Player.Jump.performed += Jump;
        playerInputActions.Player.Jump.canceled += Fall;
        playerInputActions.Player.Light.performed += LightAttack;
        playerInputActions.Player.Heavy.performed += HeavyAttack;

        playerInputActions.Player.Pause.performed += Pause;
        playerInputActions.UI.Unpause.performed += Unpause;
    }

    private void FixedUpdate()
    {
        CheckDirectionalInput();
        IsGroundedGizmo();

        Move();
        Crouch();
    }

    private bool IsGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(_boxCollider2d.bounds.center, _boxCollider2d.bounds.size, 0f, Vector2.down, 0.1f, LayerMask.GetMask("Ground"));
        _animator.SetBool("isGrounded", raycastHit.collider != null);
        return raycastHit.collider != null;
    }

    private void IsGroundedGizmo()
    {
        Color rayColor;
        if (IsGrounded())
            rayColor = Color.green;
        else
            rayColor = Color.red;
        
        Debug.DrawRay(_boxCollider2d.bounds.center, Vector2.down * (_boxCollider2d.bounds.extents.y), rayColor);
    }

    private void CheckDirectionalInput()
    {
        movementInput = playerInputActions.Player.Move.ReadValue<Vector2>();
    }

    private void Move()
    {
        _animator.SetFloat("horizontal", Mathf.Abs(movementInput.x));
        if (movementInput.x == 1)
            this.transform.localScale = new Vector3(-1, 1, 1);
        else if (movementInput.x == -1)
            this.transform.localScale = new Vector3(1, 1, 1);
        _rigidbody2d.AddForce(new Vector2(movementInput.x, 0.0f) * speed, ForceMode2D.Force);
    }

    private void Crouch()
    {
        float verticalInput = playerInputActions.Player.Move.ReadValue<Vector2>().y;
        if (verticalInput < 0)
        {
            _animator.SetBool("isCrouching", true);
        }
        else
        {
            _animator.SetBool("isCrouching", false);
        }
    }

    private void Jump(InputAction.CallbackContext context)
    {
        Debug.Log("Jump");
        if(IsGrounded())
        {
            _animator.SetTrigger("jump");
            _rigidbody2d.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
        else
            Debug.Log("Not Grounded");
        // _animator.SetBool("isJumping", true);
        // _animator.SetBool("isJumping", false);
    }

    private void Fall(InputAction.CallbackContext context)
    {
        Debug.Log("Fall");
        _rigidbody2d.velocity = Vector2.zero;

    }

    private void LightAttack(InputAction.CallbackContext context)
    {
        Debug.Log("Light Attack");
        _animator.SetTrigger("lightAttack");
    }

    private void HeavyAttack(InputAction.CallbackContext context)
    {
        Debug.Log("Heavy Attack");
        _animator.SetTrigger("heavyAttack");
    }

    private void Pause(InputAction.CallbackContext context)
    {
        Debug.Log("Pause");
        isPaused = true;
        _playerInput.SwitchCurrentActionMap("UI");
        playerInputActions.UI.Enable();
        playerInputActions.Player.Disable();
    }

    private void Unpause(InputAction.CallbackContext context)
    {
        Debug.Log("Unpause");
        isPaused = false;
        _playerInput.SwitchCurrentActionMap("Player");
        playerInputActions.Player.Enable();
        playerInputActions.UI.Disable();
    }
}
