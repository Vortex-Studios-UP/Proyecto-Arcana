/* 
@Author: Christian Matos
@Date: 2023-07-02 11:00:28
@Last Modified by: Christian Matos
@Last Modified Date: 2023-07-02 11:00:28

* Functionality:
* Approach:
* To Use:
* Dependencies:
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    private PolygonCollider2D _roomBoundaries;

    private void Awake() 
    {
        _roomBoundaries = GetComponent<PolygonCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.CompareTag("Player") && !other.isTrigger)
        {
            RoomManager.Instance.SwitchRoom(_roomBoundaries);
        }
    }
}
