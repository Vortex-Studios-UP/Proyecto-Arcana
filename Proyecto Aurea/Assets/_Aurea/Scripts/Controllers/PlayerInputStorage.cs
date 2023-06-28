/* 
@Author: Christian Matos
@Date: 2023-06-27 16:14:31
@Last Modified by: Christian Matos
@Last Modified Date: 2023-06-27 16:14:31

* Functionality: Store player input.
* Approach: Store input from InputActions so other objects can retrieve it.
* To Use: Attach to a player object.
* Dependencies: Player Input component, Unity Input Package
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputStorage : MonoBehaviour
{
    private PlayerInputActions playerInputActions;

    private void Awake() 
    {
        playerInputActions = new PlayerInputActions();
    }

    void OnEnable()
    {
        playerInputActions.Player.Enable();
    }

    void OnDisable()
    {
        playerInputActions.Player.Disable();
    }

    public bool GetJumpInput()
    {
        return playerInputActions.Player.Jump.triggered;
    }
}
