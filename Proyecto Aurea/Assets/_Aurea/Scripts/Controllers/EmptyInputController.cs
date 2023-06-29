/* 
@Author: Christian Matos
@Date: 2023-06-28 19:56:38
@Last Modified by: Christian Matos
@Last Modified Date: 2023-06-28 20:47

* Functionality: Placeholder for empty input.
* Approach: 
* To Use:
* Dependencies:
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyInputController : InputController
{
    public override float RetrieveMoveInput(GameObject gameObject)
    {
        return 0f;
    }

    public override bool RetrieveJumpInput(GameObject gameObject)
    {
        return false;
    }
}
