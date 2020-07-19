using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public static MapGenerator Instance;
    [Header("Level Options")]
    public int bigRooms;
    public int maxRooms;
    public int startclosingRooms;

    [Header("Room Prefabs")]
    public GameObject startRoom;
    public GameObject hallwayHorizontal;
    public GameObject hallwayVertical;
    public GameObject[] bigRoom;
    public GameObject chamberHorizontal;
    public GameObject chamberVertical;
    public GameObject endEast;
    public GameObject endWest;
    public GameObject endNorth;
    public GameObject endSouth;
    public GameObject endRoomEast;
    public GameObject endRoomWest;
    public GameObject endRoomNorth;
    public GameObject endRoomSouth;
    

    [Header("Level Object")]
    public GameObject levelObject;
    private GameObject tempRoomObject;

    public Room currentRoom;

    public List<Room> roomsToConnect = new List<Room>();
    int rnd;
    public int roomsCreated = 0;
    private int endRoomCount = 0;


    void Start()
    {
        if (Instance == null)
            Instance = this;

        // Create Start Room
        var startRoomObject = Instantiate(startRoom, new Vector3(0, 0, 0), Quaternion.identity * Quaternion.Euler(0, 0, 0));
        startRoomObject.transform.parent = levelObject.transform;
        currentRoom = startRoomObject.GetComponent<Room>();
        roomsCreated++;
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
            CreateConnection();
        }
    }

    void CreateConnection()
    {
        rnd = Random.Range(0, 4);

        // East Connection
        if (!currentRoom.eastConnected && roomsCreated < maxRooms)
        {
            // Create hallway in east direction
            if (currentRoom.roomType == Room.RoomType.StartRoom || currentRoom.roomType == Room.RoomType.BigRoom || currentRoom.roomType == Room.RoomType.ChamberHorizontal)
            {
                if (rnd > 1 && roomsCreated > startclosingRooms)
                {
                    if (roomsCreated > startclosingRooms + 5 && endRoomCount == 0)
                    {
                        SpawnRoom(Room.RoomType.EndRoomEast, "east");
                        endRoomCount++;
                    }
                    else
                        SpawnRoom(Room.RoomType.EndEast, "east");
                }
                else
                    SpawnRoom(Room.RoomType.HallwayHorizontal, "east");

            }
            else
            {
                switch (rnd)
                {
                    case 0:
                        SpawnRoom(Room.RoomType.HallwayHorizontal, "east");
                        break;
                    case 1:
                        if (roomsCreated > startclosingRooms)
                        {
                            if (roomsCreated > startclosingRooms + 5 && endRoomCount == 0)
                            {
                                SpawnRoom(Room.RoomType.EndRoomEast, "east");
                                endRoomCount++;
                            }
                            else
                                SpawnRoom(Room.RoomType.EndEast, "east");
                        }
                            
                        break;
                    case 2:
                        SpawnRoom(Room.RoomType.ChamberHorizontal, "east");
                        break;
                    case 3:
                        if(bigRooms > 0)
                            SpawnRoom(Room.RoomType.BigRoom, "east");
                        break;
                }
            }
        }

        // West Connection
        if (!currentRoom.westConnected && roomsCreated < maxRooms)
        {
            // Create hallway in west direction
            if (currentRoom.roomType == Room.RoomType.BigRoom || currentRoom.roomType == Room.RoomType.ChamberHorizontal)
            {
                if (rnd > 1 && roomsCreated > startclosingRooms)
                {
                    if (roomsCreated > startclosingRooms + 5 && endRoomCount == 0)
                    {
                        SpawnRoom(Room.RoomType.EndRoomWest, "west");
                        endRoomCount++;
                    }
                    else
                        SpawnRoom(Room.RoomType.EndWest, "west");
                }
                    
                else
                    SpawnRoom(Room.RoomType.HallwayHorizontal, "west");
                
            }
            else
            {
                
                switch (rnd)
                {
                    case 0:
                        SpawnRoom(Room.RoomType.HallwayHorizontal, "west");
                        break;
                    case 1:
                        if (roomsCreated > startclosingRooms)
                        {
                            if (roomsCreated > startclosingRooms + 5 && endRoomCount == 0)
                            {
                                SpawnRoom(Room.RoomType.EndRoomWest, "west");
                                endRoomCount++;
                            }
                            else
                                SpawnRoom(Room.RoomType.EndWest, "west");
                        }
                        break;
                    case 2:
                        SpawnRoom(Room.RoomType.ChamberHorizontal, "west");
                        break;
                    case 3:
                        if (bigRooms > 0)
                            SpawnRoom(Room.RoomType.BigRoom, "west");
                        break;
                }
            }
        }

        //// North Connection
        if (!currentRoom.northConnected && roomsCreated < maxRooms)
        {
            // Create hallway in north direction
            if (currentRoom.roomType == Room.RoomType.BigRoom || currentRoom.roomType == Room.RoomType.ChamberVertical)
            {
                if (rnd > 1 && roomsCreated > startclosingRooms)
                {
                    if (roomsCreated > startclosingRooms + 5 && endRoomCount == 0)
                    {
                        SpawnRoom(Room.RoomType.EndRoomNorth, "north");
                        endRoomCount++;
                    }
                    else
                        SpawnRoom(Room.RoomType.EndNorth, "north");
                }
                    
                else
                    SpawnRoom(Room.RoomType.HallwayVertical, "north");
            }
            else
            {
                switch (rnd)
                {
                    case 0:
                        SpawnRoom(Room.RoomType.HallwayVertical, "north");
                        break;
                    case 1:
                        if (roomsCreated > startclosingRooms)
                        {
                            if (roomsCreated > startclosingRooms + 5 && endRoomCount == 0)
                            {
                                SpawnRoom(Room.RoomType.EndRoomNorth, "north");
                                endRoomCount++;
                            } 
                            else
                                SpawnRoom(Room.RoomType.EndNorth, "north");
                        }
                        break;
                    case 2:
                        SpawnRoom(Room.RoomType.ChamberVertical, "north");
                        break;
                    case 3:
                        if (bigRooms > 0)
                            SpawnRoom(Room.RoomType.BigRoom, "north");
                        break;
                }
            }
        }

        // South Connection
        if (!currentRoom.southConnected && roomsCreated < maxRooms)
        {
            // Create hallway in south direction
            if (currentRoom.roomType == Room.RoomType.BigRoom || currentRoom.roomType == Room.RoomType.ChamberVertical)
            {
                if (rnd > 1 && roomsCreated > startclosingRooms)
                {
                    if (roomsCreated > startclosingRooms + 5 && endRoomCount == 0)
                    {
                        SpawnRoom(Room.RoomType.EndRoomSouth, "south");
                        endRoomCount++;
                    }
                    else
                        SpawnRoom(Room.RoomType.EndSouth, "south");
                }
                else
                    SpawnRoom(Room.RoomType.HallwayVertical, "south");
            }
            else
            {
                switch (rnd)
                {
                    case 0:
                        SpawnRoom(Room.RoomType.HallwayVertical, "south");
                        break;
                    case 1:
                        if (roomsCreated > startclosingRooms)
                        {
                            if (roomsCreated > startclosingRooms + 5 && endRoomCount == 0)
                            {
                                SpawnRoom(Room.RoomType.EndRoomSouth, "south");
                                endRoomCount++;
                            }
                            else
                                SpawnRoom(Room.RoomType.EndSouth, "south");
                        }
                        break;
                    case 2:
                        SpawnRoom(Room.RoomType.ChamberVertical, "south");
                        break;
                    case 3:
                        if (bigRooms > 0)
                            SpawnRoom(Room.RoomType.BigRoom, "south");
                        break;
                }
            }
        }
    }

    void SpawnRoom(Room.RoomType type, string direction)
    {
        
        Vector3 offset = new Vector3();

        switch (type)
        {
            case Room.RoomType.HallwayHorizontal:
                tempRoomObject = Instantiate(hallwayHorizontal, new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0));
                break;
            case Room.RoomType.HallwayVertical:
                tempRoomObject = Instantiate(hallwayVertical, new Vector3(0, 0, 0), Quaternion.Euler(0, 90f, 0));
                break;
            case Room.RoomType.BigRoom:
                rnd = Random.Range(0, 4);
                tempRoomObject = Instantiate(bigRoom[rnd], new Vector3(0, 0, 0), Quaternion.Euler(0, GetRoomRotation(rnd), 0));
                bigRooms--;
                break;
            case Room.RoomType.ChamberHorizontal:
                tempRoomObject = Instantiate(chamberHorizontal, new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0));
                break;
            case Room.RoomType.ChamberVertical:
                tempRoomObject = Instantiate(chamberVertical, new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0));
                break;
            case Room.RoomType.EndEast:
                tempRoomObject = Instantiate(endEast, new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0));
                break;
            case Room.RoomType.EndWest:
                tempRoomObject = Instantiate(endWest, new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0));
                break;
            case Room.RoomType.EndNorth:
                tempRoomObject = Instantiate(endNorth, new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0));
                break;
            case Room.RoomType.EndSouth:
                tempRoomObject = Instantiate(endSouth, new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0));
                break;
            case Room.RoomType.EndRoomEast:
                tempRoomObject = Instantiate(endRoomEast, new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0));
                break;
            case Room.RoomType.EndRoomWest:
                tempRoomObject = Instantiate(endRoomWest, new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0));
                break;
            case Room.RoomType.EndRoomNorth:
                tempRoomObject = Instantiate(endRoomNorth, new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0));
                break;
            case Room.RoomType.EndRoomSouth:
                tempRoomObject = Instantiate(endRoomSouth, new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0));
                break;
            default:
                break;
        
        }

        //Align room to connection points
        switch (direction)
        {
            case "east":
                offset = tempRoomObject.GetComponent<Room>().westConnection.transform.position - currentRoom.GetComponent<Room>().eastConnection.transform.position;
                currentRoom.eastConnected = true;
                tempRoomObject.GetComponent<Room>().westConnected = true;
                break;
            case "west":
                offset = tempRoomObject.GetComponent<Room>().eastConnection.transform.position - currentRoom.GetComponent<Room>().westConnection.transform.position;
                currentRoom.westConnected = true;
                tempRoomObject.GetComponent<Room>().eastConnected = true;
                break;
            case "north":
                offset = tempRoomObject.GetComponent<Room>().southConnection.transform.position - currentRoom.GetComponent<Room>().northConnection.transform.position;
                currentRoom.northConnected = true;
                tempRoomObject.GetComponent<Room>().southConnected = true;
                break;
            case "south":
                offset = tempRoomObject.GetComponent<Room>().northConnection.transform.position - currentRoom.GetComponent<Room>().southConnection.transform.position;
                currentRoom.southConnected = true;
                tempRoomObject.GetComponent<Room>().northConnected = true;
                break;
        }

        tempRoomObject.transform.position -= offset;
        tempRoomObject.transform.parent = levelObject.transform;
        roomsToConnect.Add(tempRoomObject.GetComponent<Room>());

        roomsCreated++;
    }

    float GetRoomRotation(int rnd)
    {
        switch (rnd)
        {
            case 0:
                return 0;
            case 1:
                return 90f;
            case 2:
                return 180f;
            case 3:
                return 270f;
            default:
                return 0;
        }
    }
}
