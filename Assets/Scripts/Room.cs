using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    
    public enum RoomType{ StartRoom, HallwayHorizontal, HallwayVertical, BigRoom, SmallRoom, ChamberHorizontal, ChamberVertical, EndNorth, EndSouth, EndWest, EndEast, EndRoomEast, EndRoomWest, EndRoomNorth, EndRoomSouth};

    [Header("Room Type")]
    public RoomType roomType;

    [Header("Connection Positions")]
    public Transform northConnection;
    public Transform eastConnection;
    public Transform southConnection;
    public Transform westConnection;

    [Header("Connection validation")]
    public bool northConnected = false;
    public bool eastConnected = false;
    public bool southConnected = false;
    public bool westConnected = false;

    [Header("Room Connected")]
    public bool roomConnected = false;

    private void Start()
    {
        if (!northConnection)
            northConnected = true;

        if (!eastConnection)
            eastConnected = true;

        if (!southConnection)
            southConnected = true;

        if (!westConnection)
            westConnected = true;
    }
    private void Update()
    {
        if (northConnected && eastConnected && southConnected && westConnected)
        {
            roomConnected = true;
        }
    }
}
