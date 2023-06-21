/* 
@Author: Christian Matos
@Date: 2023-06-19 20:45:38
@Last Modified by: Christian Matos
@Last Modified Date: 2023-06-19 20:45:38

* Functionality:
* Approach:
* To Use:
* Dependencies:
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AbilityHolder : MonoBehaviour
{

    public List<Ability> abilities; // This is the list of abilities that the player has. This is populated in the inspector.
    // To manage ability progression, simply add or remove abilities from this list during runtime.
    public Animator _animator; // This is the animator component of the player. Animations are played directly from this script.

    private PlayerInputActions playerInputActions; // This is the input manager that is used to get input from the player.

    private void Awake()
    {
        _animator = GetComponent<Animator>();

        // Initializes the input manager
        playerInputActions = new PlayerInputActions(); 
        playerInputActions.Player.Enable();

        // Subscribes the input actions to the appropriate methods
        playerInputActions.Player.Jump.performed += Jump;
        playerInputActions.Player.Light.performed += Light;
        playerInputActions.Player.Heavy.performed += Heavy;
        playerInputActions.Player.Magic.performed += Magic;
        playerInputActions.Player.Dodge.performed += Dodge;
        playerInputActions.Player.Grab.performed += Grab;
        playerInputActions.Player.Special.performed += Special;
    }

    private void Update()
    {
        //Debug.Log(playerInputActions.Player.Move.ReadValue<Vector2>());
    }

    // Returns the direction of the current directional input in a (-90, 90) degree format. Returns 180 if neutral.
    private float GetInputAngle()
    {
        Vector2 directionalInput = playerInputActions.Player.Move.ReadValue<Vector2>();

        if (directionalInput.x == 0 && directionalInput.y == 0)
            return 180.0f;

        directionalInput.x = Mathf.Abs(directionalInput.x);
        float angle = Mathf.Atan2(directionalInput.y, directionalInput.x) * Mathf.Rad2Deg;

        return angle;
    }

    // Returns the direction of the current directional input in an 8 directional format.
    private InputDirection AngleToEightInputDirection(float angle)
    {
        // Values from -90 to 90 return a direction.
        if (angle >= 67.5f && angle <= 90.0f)
            return InputDirection.Up;
        else if (angle >= 22.5f && angle < 67.5f)
            return InputDirection.UpSide;
        else if (angle >= -22.5f && angle < 22.5f)
            return InputDirection.Side;
        else if (angle >= -67.5f && angle < -22.5f)
            return InputDirection.DownSide;
        else if (angle >= -90.0f && angle < -67.5f)
            return InputDirection.Down;
        
        // Values outside of -90 to 90 return neutral. GetInputAngle() returns 180 if neutral.
        return InputDirection.Neutral;
    }

    // Returns the direction of the current directional input in a 4 directional format.
    private InputDirection AngleToFourInputDirection(float angle)
    {
        // Values from -90 to 90 return a direction.
        if (angle >= 45.0f && angle <= 90.0f)
            return InputDirection.Up;
        else if (angle >= -45.0f && angle < 45.0f)
            return InputDirection.Side;
        else if (angle >= -90.0f && angle < -45.0f)
            return InputDirection.Down;
        
        // Values outside of -90 to 90 return neutral. GetInputAngle() returns 180 if neutral.
        return InputDirection.Neutral;
    }

    private PlayerState CheckPlayerState()
    {
        return PlayerState.Grounded;
    }

    // Executes the ability that matches the input button.
    private void ExecuteAbility(InputButton inputButton, bool eightDirectional = false)
    {
        // Initially gets the input direction in 8 directional format.
        float angle = GetInputAngle();
        InputDirection inputDirection;

        if(eightDirectional)
        {
            // If the ability is eight-directional, gets the input direction in 8 directional format and executes the ability.
            inputDirection = AngleToEightInputDirection(angle);
            Debug.Log(inputButton + " " + inputDirection + " " + CheckPlayerState());
            if (ExecuteAbility(inputButton, inputDirection, CheckPlayerState()))
                return;
        }

        // If no eight-directional ability is found, gets the input direction in 4 directional format.
        inputDirection = AngleToFourInputDirection(angle);
        Debug.Log(inputButton + " " + inputDirection + " " + CheckPlayerState());
        if (ExecuteAbility(inputButton, inputDirection, CheckPlayerState()))
            return;

        Debug.Log("No ability found");
    }

    // Executes the ability that matches the input button, input direction and player state.
    private bool ExecuteAbility(InputButton inputButton, InputDirection inputDirection, PlayerState playerState)
    {
        // Looks for the ability that matches the input button, input direction and player state.
        for (int i = 0; i < abilities.Count; i++) // Iterates for every ability.
        {
            for (int j = 0; j < abilities[i].inputCombinations.Length; j++) // Iterates for every input combination within the ability.
            {
                if (abilities[i].inputCombinations[j].inputButton == inputButton && abilities[i].inputCombinations[j].inputDirection == inputDirection && abilities[i].inputCombinations[j].playerState == playerState)
                {
                    // If the ability is found, it is activated and the animation is played.
                    abilities[i].Activate(gameObject);
                    _animator.Play(abilities[i].animation.name);
                    return true;
                }
            }
        }

        // If no ability is found, returns false.
        return false;
    }

    // Input Action Methods
    #region Input Action Methods
    private void Jump(InputAction.CallbackContext context)
    {
        ExecuteAbility(InputButton.Jump);
    }

    private void Light(InputAction.CallbackContext context)
    {
        // Light attack is eight-directional because of the dash.
        ExecuteAbility(InputButton.Light, true);
    }

    private void Heavy(InputAction.CallbackContext context)
    {
        ExecuteAbility(InputButton.Heavy);
    }

    private void Magic(InputAction.CallbackContext context)
    {
        ExecuteAbility(InputButton.Magic);
    }

    private void Dodge(InputAction.CallbackContext context)
    {
        ExecuteAbility(InputButton.Dodge);
    }

    private void Grab(InputAction.CallbackContext context)
    {
        ExecuteAbility(InputButton.Grab);
    }

    private void Special(InputAction.CallbackContext context)
    {
        ExecuteAbility(InputButton.Special);
    }   
    #endregion
}
