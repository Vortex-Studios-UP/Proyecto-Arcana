/* 
@Author: Christian Matos
@Date: 2023-06-27 15:12:32
@Last Modified by: Christian Matos
@Last Modified Date: 2023-06-27 15:12:32

* Functionality: Handle NPC input.
* Approach: Override InputController methods to retrieve input from code.
* To Use: Attach to an NPC object.
* Dependencies: None
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AIController", menuName = "Controllers/AIController")]
public class AIController : InputController
{
    public override bool RetrieveJumpInput()
    {
        return true;
    }

    public override float RetrieveMoveInput()
    {
        return 2f;
    }
}
