/* 
@Author: Christian Matos
@Date: 2023-06-27 15:12:35
@Last Modified by: Christian Matos
@Last Modified Date: 2023-06-28 16:17:05

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

    public override bool RetrieveJumpInput(GameObject gameObject)
    {
        return _inputActionAsset.FindActionMap("Player").FindAction("Jump").triggered;
    }

    public override float RetrieveMoveInput(GameObject gameObject)
    {
        return _inputActionAsset.FindActionMap("Player").FindAction("Move").ReadValue<Vector2>().x;
    }

    public override bool RetrieveJumpHoldInput(GameObject gameObject)
    {
        return Input.GetButton("Jump");
    }
}
