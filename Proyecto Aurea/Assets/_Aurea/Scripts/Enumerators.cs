/* 
@Author: Christian Matos
@Date: 2023-06-19 20:37:02
@Last Modified by: Christian Matos
@Last Modified Date: 2023-06-19 20:37:02

* Functionality:
* Approach:
* To Use:
* Dependencies:
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InputDirection
{
    Up, UpSide, Side, DownSide, Down, Neutral
}

public enum InputButton
{
    Jump, Light, Heavy, Magic, Dodge, Grab, Special
}

public enum PlayerState
{
    Grounded, Airborne, Attacking, Stunned, OnWall
}

public enum Stat
{
    Vitality, Defense, Strength, Magic, None
}