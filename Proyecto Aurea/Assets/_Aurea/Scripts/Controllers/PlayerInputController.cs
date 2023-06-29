/* 
@Author: Christian Matos
@Date: 2023-06-27 15:12:35
@Last Modified by: Christian Matos
@Last Modified Date: 2023-06-28 20:47

* Functionality: Handle player input.
* Approach: Override InputController methods to retrieve input from InputActions.
* To Use: Attach to a player object.
* Dependencies: Player Input component, Unity Input Package
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "PlayerInputController", menuName = "Controllers/PlayerInputController")]
public class PlayerInputController : InputController
{
    public PlayerInputActions _playerInputActions;
    private bool isJumping;

    private void OnEnable()
    {
        _playerInputActions = new PlayerInputActions();
        _playerInputActions.Enable();
        _playerInputActions.Player.Jump.started += JumpStarted;
        _playerInputActions.Player.Jump.canceled += JumpCanceled;
    }

    private void OnDisable()
    {
        _playerInputActions.Disable();
        _playerInputActions.Player.Jump.started -= JumpStarted;
        _playerInputActions.Player.Jump.canceled -= JumpCanceled;
        _playerInputActions = null;
    }

    public override bool RetrieveJumpInput(GameObject gameObject)
    {
        return isJumping;
    }

    public override float RetrieveMoveInput(GameObject gameObject)
    {
        return _playerInputActions.Player.Move.ReadValue<Vector2>().x;
    }

    private void JumpStarted(InputAction.CallbackContext context)
    {
        isJumping = true;
    }

    private void JumpCanceled(InputAction.CallbackContext context)
    {
        isJumping = false;
    }
}
