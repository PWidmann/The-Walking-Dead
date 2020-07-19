using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public static MapGenerator Instance;
    [Header("Level Options")]
    public int bigRooms = 3;

    [Header("Room Prefabs")]
    public GameObject startRoom;
    public GameObject hallwayHorizontal;
    public GameObject hallwayVertical;
    public GameObject bigRoom;

    [Header("Room Prefabs")]
    public GameObject levelObject;

    private Room currentRoom;

    public List<Room> roomsToConnect = new List<Room>();


    void Start()
    {
        if (Instance == null)
            Instance = this;


        var startRoomObject = Instantiate(startRoom, new Vector3(0, 0, 0), Quaternion.identity * Quaternion.Euler(0, 0, 0));

        currentRoom = startRoomObject.GetComponent<Room>();

        startRoomObject.transform.parent = levelObject.transform;
    }

    private void Update()
    {
        if (currentRoom.roomConnected)
        {
            roomsToConnect.Remove(currentRoom);

            if (roomsToConnect.Count > 0)
                currentRoom = roomsToConnect[0];
        }
        else
        {
                CreateRoom();
        }
    }

    void CreateRoom()
    {
        if (!currentRoom.eastConnected)
        {
            var nextRoom = GetNextRoom("east");
            nextRoom.transform.parent = levelObject.transform;
            roomsToConnect.Add(nextRoom.GetComponent<Room>());

            // Align room to connection points
            Vector3 offset = nextRoom.GetComponent<Room>().westConnection.transform.position - currentRoom.GetComponent<Room>().eastConnection.transform.position;
            nextRoom.transform.position -= offset;

            currentRoom.eastConnected = true;
            nextRoom.GetComponent<Room>().westConnected = true;

        }

        if (!currentRoom.westConnected)
        {
            var nextRoom = GetNextRoom("west");
            nextRoom.transform.parent = levelObject.transform;
            roomsToConnect.Add(nextRoom.GetComponent<Room>());

            // Align room to connection points
            Vector3 offset = nextRoom.GetComponent<Room>().westConnection.transform.position - currentRoom.GetComponent<Room>().eastConnection.transform.position;
            nextRoom.transform.position -= offset;

            currentRoom.westConnected = true;
            nextRoom.GetComponent<Room>().westConnected = true;

        }

        if (!currentRoom.northConnected)
        {
            var nextRoom = GetNextRoom("north");
            nextRoom.transform.parent = levelObject.transform;
            roomsToConnect.Add(nextRoom.GetComponent<Room>());

            // Align room to connection points
            Vector3 offset = nextRoom.GetComponent<Room>().southConnection.transform.position - currentRoom.GetComponent<Room>().northConnection.transform.position;
            nextRoom.transform.position -= offset;

            currentRoom.northConnected = true;
            nextRoom.GetComponent<Room>().southConnected = true;

        }

        if (!currentRoom.southConnected)
        {
            var nextRoom = GetNextRoom("south");
            nextRoom.transform.parent = levelObject.transform;
            roomsToConnect.Add(nextRoom.GetComponent<Room>());

            // Align room to connection points
            Vector3 offset = nextRoom.GetComponent<Room>().northConnection.transform.position - currentRoom.GetComponent<Room>().southConnection.transform.position;
            nextRoom.transform.position -= offset;

            currentRoom.southConnected = true;
            nextRoom.GetComponent<Room>().northConnected = true;

        }
    }


    GameObject GetNextRoom(string direction)
    {
        switch (direction)
        {
            case "east":
                if (currentRoom.roomType == Room.RoomType.BigRoom || currentRoom.roomType == Room.RoomType.StartRoom)
                    return Instantiate(hallwayHorizontal, new Vector3(0, 0, 0), Quaternion.Euler(0, 0f, 0));
                else
                {
                    if (bigRooms > 0)
                    {
                        bigRooms--;
                        return Instantiate(bigRoom, new Vector3(0, 0, 0), Quaternion.Euler(0, 0f, 0));
                    }
                    return Instantiate(hallwayHorizontal, new Vector3(0, 0, 0), Quaternion.Euler(0, 0f, 0));
                }
                    
            case "west":
                if (currentRoom.roomType == Room.RoomType.BigRoom)
                    return Instantiate(hallwayHorizontal, new Vector3(0, 0, 0), Quaternion.Euler(0, 0f, 0));
                else
                {
                    if (bigRooms > 0)
                    {
                        bigRooms--;
                        return Instantiate(bigRoom, new Vector3(0, 0, 0), Quaternion.Euler(0, 0f, 0)); ;
                    }
                    return Instantiate(hallwayHorizontal, new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0));
                }
            case "north":
                if (currentRoom.roomType == Room.RoomType.BigRoom)
                    return Instantiate(hallwayVertical, new Vector3(0, 0, 0), Quaternion.Euler(0, 90f, 0));
                else
                {
                    if (bigRooms > 0)
                    {
                        bigRooms--;
                        return Instantiate(bigRoom, new Vector3(0, 0, 0), Quaternion.Euler(0, 0f, 0)); ;
                    }
                    return Instantiate(hallwayVertical, new Vector3(0, 0, 0), Quaternion.Euler(0, 90f, 0));
                }
            case "south":
                if (currentRoom.roomType == Room.RoomType.BigRoom)
                    return Instantiate(hallwayVertical, new Vector3(0, 0, 0), Quaternion.Euler(0, 90f, 0));
                else
                {
                    if (bigRooms > 0)
                    {
                        bigRooms--;
                        return Instantiate(bigRoom, new Vector3(0, 0, 0), Quaternion.Euler(0, 0f, 0));
                    }
                    return Instantiate(hallwayVertical, new Vector3(0, 0, 0), Quaternion.Euler(0, 90f, 0));
                }
            default:
                return Instantiate(hallwayHorizontal, new Vector3(0, 0, 0), Quaternion.Euler(0, 0f, 0));
        }
    }
}
