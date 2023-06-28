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
    public InputActionAsset _inputActionAsset;

    public override bool RetrieveJumpInput()
    {
        // return Input.GetKeyDown(KeyCode.Space);
        return _inputActionAsset.FindActionMap("Player").FindAction("Jump").triggered;
    }

    public override float RetrieveMoveInput()
    {
        // return Input.GetAxisRaw("Horizontal");
        return _inputActionAsset.FindActionMap("Player").FindAction("Move").ReadValue<Vector2>().x;
    }
}
