/* 
@Author: Christian Matos
@Date: 2023-06-27 15:01:37
@Last Modified by: Christian Matos
@Last Modified Date: 2023-06-27 15:01:37

* Functionality: Template to create objects that retrieve input from a source.
* Approach: Derive player and enemy input from a common base class.
* To Use: Inherit from this class and override the methods to retrieve input from a source. Players from InputActions and enemies from code, for example.
* Dependencies: None
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InputController : ScriptableObject
{
    public abstract float RetrieveMoveInput();
    public abstract bool RetrieveJumpInput();
}
