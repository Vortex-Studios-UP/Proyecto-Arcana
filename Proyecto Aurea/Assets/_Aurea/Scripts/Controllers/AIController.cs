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
    [SerializeField] private LayerMask layerMask = 0;
    [SerializeField] private float raycastBottomDistance = 1f;
    [SerializeField] private float raycastTopDistance = 1f;

    [SerializeField] private float raycastXOffset = 0.5f;


    private RaycastHit2D groundInfoTop;
    private RaycastHit2D groundInfoBottom;

    public override bool RetrieveJumpHoldInput(GameObject gameObject)
    {
        return false;
    }

    public override bool RetrieveJumpInput(GameObject gameObject)
    {
        return false;
    }

    public override float RetrieveMoveInput(GameObject gameObject)
    {
        groundInfoBottom = Physics2D.Raycast(new Vector2(gameObject.transform.position.x + (raycastXOffset * gameObject.transform.localScale.x), gameObject.transform.position.y - raycastBottomDistance), Vector2.down, raycastBottomDistance, layerMask);
        Debug.DrawRay(new Vector2(gameObject.transform.position.x + (raycastXOffset * gameObject.transform.localScale.x), gameObject.transform.position.y), Vector2.down * raycastBottomDistance, Color.green);

        groundInfoTop = Physics2D.Raycast(new Vector2(gameObject.transform.position.x + (raycastXOffset * gameObject.transform.localScale.x), gameObject.transform.position.y - raycastBottomDistance), Vector2.right * gameObject.transform.localScale.x, raycastTopDistance, layerMask);
        Debug.DrawRay(new Vector2(gameObject.transform.position.x + (raycastXOffset * gameObject.transform.localScale.x), gameObject.transform.position.y), Vector2.right * raycastTopDistance * gameObject.transform.localScale.x, Color.green);

        if(groundInfoTop.collider == true || groundInfoBottom.collider == false)
        {
            gameObject.transform.localScale = new Vector2(gameObject.transform.localScale.x * -1, gameObject.transform.localScale.y);
        }

        return gameObject.transform.localScale.x;
    }
}
