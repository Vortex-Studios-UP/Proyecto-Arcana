/* 
@Author: Christian Matos
@Date: 2023-07-02 10:47:54
@Last Modified by: Christian Matos
@Last Modified Date: 2023-07-02 10:47:54

* Functionality:
* Approach:
* To Use:
* Dependencies:
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class RoomManager : MonoBehaviour
{
    public static RoomManager Instance { get; private set; }

    private CinemachineConfiner _cinemachineConfiner;

    private void Awake()
    {
        Instance = this;
        _cinemachineConfiner = GetComponent<CinemachineConfiner>();
    }

    public void SwitchRoom(PolygonCollider2D roomBoundaries)
    {
        _cinemachineConfiner.m_BoundingShape2D = roomBoundaries;
    }


}
