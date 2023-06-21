/* 
@Author: Christian Matos
@Date: 2023-06-19 20:31:19
@Last Modified by: Christian Matos
@Last Modified Date: 2023-06-19 20:31:19

* Functionality:
* Approach:
* To Use:
* Dependencies:
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Ability", menuName = "ScriptableObjects/Ability", order = 1)]
public class Ability : ScriptableObject
{
    [Header("Behavior")]
    public new string name;
    public AnimationClip animation;

    [Header("Input Info")]
    public InputCombination[] inputCombinations;

    [Header("Stats")]
    public float power = 0;
    public Stat multiplierStat = Stat.None;
    public bool canStun;
    public virtual void Activate(GameObject player) 
    {
        Debug.Log(name + " Ability Activated");
    }
}

[System.Serializable]
public struct InputCombination
{
    public InputDirection inputDirection;
    public InputButton inputButton;
    public PlayerState playerState;
}