/* 
@Author: Christian Matos
@Date: 2023-06-27 15:12:35
@Last Modified by: Christian Matos
@Last Modified Date: 2023-06-27 15:12:35

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
    public PlayerInput playerInputStorage;

    public override bool RetrieveJumpInput()
    {
        // return playerInputActions.Player.Jump.triggered;
        return Input.GetKeyDown(KeyCode.Space);
    }

    public override float RetrieveMoveInput()
    {
        // return playerInputActions.Player.Move.ReadValue<float>();
        return Input.GetAxisRaw("Horizontal");
    }
}
