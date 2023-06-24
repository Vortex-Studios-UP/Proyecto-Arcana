/* 
@Author: Christian Matos
@Date: 2023-06-21 15:36:59
@Last Modified by: Christian Matos
@Last Modified Date: 2023-06-21 15:36:59

* Functionality:
* Approach:
* To Use:
* Dependencies:
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Vector2 cameraOffset;
    private Transform playerTransform;

    private void Awake()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update() 
    {
        transform.position = new Vector3(playerTransform.position.x + cameraOffset.x, playerTransform.position.y + cameraOffset.y, transform.position.z);
    }
}
