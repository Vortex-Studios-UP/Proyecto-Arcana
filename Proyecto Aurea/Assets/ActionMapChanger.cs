/* 
@Author: Christian Matos
@Date: 2023-07-02 09:32:56
@Last Modified by: Christian Matos
@Last Modified Date: 2023-07-02 09:47

* Functionality: Changes the player's input action map during runtime,
* Approach: Defines methods that change the player's input action map.
* To Use: Call the included methods from Unity Events to change the player's input action map.
* Dependencies: PlayerInput
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class ActionMapChanger : MonoBehaviour
{
    private PlayerInput _playerInput;

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
    }

    public void SwitchToPlayer()
    {
        _playerInput.SwitchCurrentActionMap("Player");
    }

    public void SwitchToUI()
    {
        _playerInput.SwitchCurrentActionMap("UI");
    }
}
