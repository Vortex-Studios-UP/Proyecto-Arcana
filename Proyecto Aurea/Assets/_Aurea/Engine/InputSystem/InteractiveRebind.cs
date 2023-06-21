/* 
@Author: Christian Matos
@Date: 2023-06-18 13:52:45
@Last Modified by: Christian Matos
@Last Modified Date: 2023-06-18 13:52:45

* Functionality:
* Approach:
* To Use:
* Dependencies:
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractiveRebind : MonoBehaviour
{
    PlayerController playerController;
    PlayerInputActions playerInputActions;
    PlayerInput playerInput;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        playerInputActions = playerController.playerInputActions;
        playerInput = GetComponent<PlayerInput>();
    }

    public void StartRebind()
    {
        playerInputActions.Player.Disable();
    }

    public void EndRebind()
    {
        playerInputActions.Player.Enable();
    }

    public void RebindJump()
    {
        StartRebind();
        playerInputActions.Player.Jump.PerformInteractiveRebinding()
            .OnComplete(calllback =>
            {
                Debug.Log("Rebound Jump to " + calllback.action.bindings[0].overridePath);
                calllback.Dispose();
                EndRebind();
            })
            .Start();
    }

    // void SaveUserRebinds(PlayerInput playerInput)
    // {
    //     var rebinds = playerInput.actions.SaveBindingOverridesAsJson();
    //     PlayerPrefs.SetString("rebinds", rebinds);
    // }

    // void LoadUserRebinds(PlayerInput playerInput)
    // {
    //     var rebinds = PlayerPrefs.GetString("rebinds");
    //     playerInput.Player.loadBindingOverridesFromJson(rebinds);
    // }

}
