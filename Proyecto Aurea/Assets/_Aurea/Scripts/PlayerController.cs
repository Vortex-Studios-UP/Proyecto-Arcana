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

    // Player Components
    private Rigidbody2D _rigidbody2d;
    private PlayerInput _playerInput;
    public PlayerInputActions playerInputActions;

    // Private variables
    private bool isPaused = false;

    private void Awake()
    {
        _rigidbody2d = GetComponent<Rigidbody2D>();
        _playerInput = GetComponent<PlayerInput>();
    
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();

        playerInputActions.Player.Jump.performed += Jump;
        playerInputActions.Player.Pause.performed += Pause;
        playerInputActions.UI.Unpause.performed += Unpause;
    }

    private void Update()
    {

    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        float horizontalInput = playerInputActions.Player.Move.ReadValue<Vector2>().x;
        _rigidbody2d.AddForce(new Vector2(horizontalInput, 0.0f) * speed, ForceMode2D.Force);
    }

    private void Jump(InputAction.CallbackContext context)
    {
        Debug.Log("Jump");
        _rigidbody2d.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
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
