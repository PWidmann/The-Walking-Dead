using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MapGenerator : MonoBehaviour
{
    public static MapGenerator Instance;

    [Header("Level Options")]
    public int bigRooms;
    public int startclosingRooms;

    [Header("Player Prefab")]
    public GameObject player;

    [Header("Zombie Prefab")]
    public GameObject zombiePrefab;
    public GameObject[] zombieSpawns;

    [Header("Key Prefab")]
    public GameObject key;

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
    [HideInInspector]
    public  NavMeshSurface levelMeshSurface;
    private GameObject tempRoomObject;

    public Room currentRoom;

    public List<Room> roomsToConnect = new List<Room>();
    GameObject startRoomObject;
    int rnd;
    public int roomsCreated = 0;
    private int bigRoomsCreated = 0;
    private int endRoomCount = 0;

    public List<Bounds> roomList = new List<Bounds>();
    public bool dungeonGenerated = false;
    private bool navMeshBuilt = false;
    private bool roomCollision = false;
    public bool levelStarted = false;
    public bool keySpawned = false;

    void Start()
    {
        if (Instance == null)
            Instance = this;

        levelMeshSurface = levelObject.GetComponent<NavMeshSurface>();
    }

    private void Update()
    {

        if (currentRoom)
        {
            if (dungeonGenerated)
            {
                if (!navMeshBuilt)
                {
                    
                    Debug.Log("Nav mesh built.");
                    navMeshBuilt = true;
                }
                
            }

            // Connect rooms
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

        //When Finished, check for valid dungeon
        if (roomsCreated > startclosingRooms && roomsToConnect.Count == 0)
        {
            if (dungeonGenerated == false)
            {
                Debug.Log("Dungeon finished generating!");
                dungeonGenerated = true;
                CheckForCollision();

                if (roomCollision)
                {
                    Debug.Log("Collision Detected, generating new Dungeon...");
                    ResetDungeon();
                }
                else
                {
                    if (endRoomCount > 0)
                    {
                        Debug.Log("Generated a valid Dungeon!");
                        dungeonGenerated = true;
                    }
                    else
                    {
                        Debug.Log("No end room found, generating new Dungeon...");
                        ResetDungeon();
                    }

                }
            }
        }

        if (Input.GetKeyDown(KeyCode.R) || Input.GetButtonDown("JoystickButtonY"))
        {
            if (!levelStarted)
            {
                ResetDungeon();
            }
        }
    }

    void StartGeneratingDungeon()
    {
        //// Create Start Room
        roomsToConnect.Clear();
        roomList.Clear();
        startRoomObject = Instantiate(startRoom, new Vector3(0, 0, 0), Quaternion.identity * Quaternion.Euler(0, 0, 0));
        startRoomObject.transform.parent = levelObject.transform;
        roomList.Add(startRoomObject.GetComponent<Collider>().bounds);
        currentRoom = startRoomObject.GetComponent<Room>();
        currentRoom.roomType = Room.RoomType.StartRoom;
        roomsToConnect.Add(currentRoom);
        roomsCreated = 1;
        endRoomCount = 0;
        bigRoomsCreated = 0;
        dungeonGenerated = false;
        roomCollision = false;
    }

    void SpawnEnemies()
    {
        zombieSpawns = GameObject.FindGameObjectsWithTag("ZombieSpawn");

        foreach (GameObject spawn in zombieSpawns)
        {
            Instantiate(zombiePrefab, spawn.transform.position, Quaternion.Euler(0, Random.Range(0, 360), 0));
        }
    }

    void CreateConnection()
    {
        rnd = Random.Range(0, 4);

        // East Connection
        if (!currentRoom.eastConnected)
        {
            // Create hallway in east direction
            if (currentRoom.roomType == Room.RoomType.StartRoom || currentRoom.roomType == Room.RoomType.BigRoom || currentRoom.roomType == Room.RoomType.ChamberHorizontal)
            {
                if (rnd > 1 && roomsCreated > startclosingRooms)
                {
                    if (roomsCreated > startclosingRooms + 5 && endRoomCount == 0)
                    {
                        SpawnRoom(Room.RoomType.EndRoomEast, "east");
                    }
                    else
                        SpawnRoom(Room.RoomType.EndEast, "east");
                }
                else
                {
                    SpawnRoom(Room.RoomType.HallwayHorizontal, "east");
                }


            }
            else if (currentRoom.roomType != Room.RoomType.StartRoom)
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
                            }
                            else
                                SpawnRoom(Room.RoomType.EndEast, "east");
                        }

                        break;
                    case 2:
                        //Check if place is free
                        SpawnRoom(Room.RoomType.ChamberHorizontal, "east");
                        break;
                    case 3:
                        if (bigRoomsCreated < bigRooms)
                        {
                            SpawnRoom(Room.RoomType.BigRoom, "east");
                        }
                        break;
                }
            }
        }

        // West Connection
        if (!currentRoom.westConnected)
        {
            // Create hallway in west direction
            if (currentRoom.roomType == Room.RoomType.BigRoom || currentRoom.roomType == Room.RoomType.ChamberHorizontal)
            {
                if (rnd > 1 && roomsCreated > startclosingRooms)
                {
                    if (roomsCreated > startclosingRooms + 5 && endRoomCount == 0)
                    {
                        SpawnRoom(Room.RoomType.EndRoomWest, "west");
                    }
                    else
                        SpawnRoom(Room.RoomType.EndWest, "west");
                }
                else
                {
                    SpawnRoom(Room.RoomType.HallwayHorizontal, "west");
                }
            }
            else if (currentRoom.roomType != Room.RoomType.StartRoom)
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

                            }
                            else
                                SpawnRoom(Room.RoomType.EndWest, "west");
                        }
                        break;
                    case 2:
                        SpawnRoom(Room.RoomType.ChamberHorizontal, "west");
                        break;
                    case 3:
                        if (bigRoomsCreated < bigRooms)
                        {
                            SpawnRoom(Room.RoomType.BigRoom, "west");
                        }
                        break;
                }
            }
        }

        //// North Connection
        if (!currentRoom.northConnected)
        {
            // Create hallway in north direction
            if (currentRoom.roomType == Room.RoomType.BigRoom || currentRoom.roomType == Room.RoomType.ChamberVertical)
            {
                if (rnd > 1 && roomsCreated > startclosingRooms)
                {
                    if (roomsCreated > startclosingRooms + 5 && endRoomCount == 0)
                    {
                        SpawnRoom(Room.RoomType.EndRoomNorth, "north");
                    }
                    else
                    {
                        SpawnRoom(Room.RoomType.EndNorth, "north");
                    }

                }
                else
                {
                    SpawnRoom(Room.RoomType.HallwayVertical, "north");
                }

            }
            else if (currentRoom.roomType != Room.RoomType.StartRoom)
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
                            }
                            else
                                SpawnRoom(Room.RoomType.EndNorth, "north");
                        }
                        break;
                    case 2:
                        SpawnRoom(Room.RoomType.ChamberVertical, "north");
                        break;
                    case 3:
                        if (bigRoomsCreated < bigRooms)
                        {
                            SpawnRoom(Room.RoomType.BigRoom, "north");
                        }
                        break;
                }
            }
        }

        // South Connection
        if (!currentRoom.southConnected)
        {
            // Create hallway in south direction
            if (currentRoom.roomType == Room.RoomType.BigRoom || currentRoom.roomType == Room.RoomType.ChamberVertical)
            {
                if (rnd > 1 && roomsCreated > startclosingRooms)
                {
                    if (roomsCreated > startclosingRooms + 5 && endRoomCount == 0)
                    {
                        SpawnRoom(Room.RoomType.EndRoomSouth, "south");
                    }
                    else
                    {
                        SpawnRoom(Room.RoomType.EndSouth, "south");
                    }

                }
                else
                {
                    SpawnRoom(Room.RoomType.HallwayVertical, "south");
                }

            }
            else if (currentRoom.roomType != Room.RoomType.StartRoom)
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
                            }
                            else
                                SpawnRoom(Room.RoomType.EndSouth, "south");
                        }
                        break;
                    case 2:
                        SpawnRoom(Room.RoomType.ChamberVertical, "south");
                        break;
                    case 3:
                        if (bigRoomsCreated < bigRooms)
                        {
                            SpawnRoom(Room.RoomType.BigRoom, "south");
                        }
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
                bigRoomsCreated++;
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
                endRoomCount++;
                break;
            case Room.RoomType.EndRoomWest:
                tempRoomObject = Instantiate(endRoomWest, new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0));
                endRoomCount++;
                break;
            case Room.RoomType.EndRoomNorth:
                tempRoomObject = Instantiate(endRoomNorth, new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0));
                endRoomCount++;
                break;
            case Room.RoomType.EndRoomSouth:
                tempRoomObject = Instantiate(endRoomSouth, new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0));
                endRoomCount++;
                break;
            default:
                break;
        }

        //Align room to connection points
        switch (direction)
        {
            case "east":
                offset = tempRoomObject.GetComponent<Room>().westConnection.transform.position - currentRoom.GetComponent<Room>().eastConnection.transform.position;
                tempRoomObject.transform.position -= offset;

                // Confirm connection
                currentRoom.eastConnected = true;
                tempRoomObject.GetComponent<Room>().westConnected = true;
                break;
            case "west":
                offset = tempRoomObject.GetComponent<Room>().eastConnection.transform.position - currentRoom.GetComponent<Room>().westConnection.transform.position;
                tempRoomObject.transform.position -= offset;

                // Confirm connection
                currentRoom.westConnected = true;
                tempRoomObject.GetComponent<Room>().eastConnected = true;
                break;
            case "north":
                offset = tempRoomObject.GetComponent<Room>().southConnection.transform.position - currentRoom.GetComponent<Room>().northConnection.transform.position;
                tempRoomObject.transform.position -= offset;

                // Confirm connection
                currentRoom.northConnected = true;
                tempRoomObject.GetComponent<Room>().southConnected = true;
                break;
            case "south":
                offset = tempRoomObject.GetComponent<Room>().northConnection.transform.position - currentRoom.GetComponent<Room>().southConnection.transform.position;
                tempRoomObject.transform.position -= offset;

                // Confirm connection
                currentRoom.southConnected = true;
                tempRoomObject.GetComponent<Room>().northConnected = true;
                break;
            case "start":
                break;
        }


        tempRoomObject.transform.parent = levelObject.transform;
        roomsToConnect.Add(tempRoomObject.GetComponent<Room>());


        roomsCreated++;
    }


    public void ResetDungeon()
    {
        int childCount = levelObject.transform.childCount;
        for (var i = childCount - 1; i >= 0; i--)
        {
            Destroy(levelObject.transform.GetChild(i).gameObject);
        }

        
        StartGeneratingDungeon();
    }

    public void SpawnPlayer()
    {
        if (navMeshBuilt)
        {
            Instantiate(player, GameObject.Find("PlayerSpawn").transform.position, Quaternion.Euler(0, 90f, 0));

            SpawnEnemies();

            SpawnKey();
        }
            
    }

    public void RespawnPlayer()
    {
        Instantiate(player, GameObject.Find("PlayerSpawn").transform.position, Quaternion.Euler(0, 90f, 0));
    }

    void CheckForCollision()
    {
        foreach (Transform child in levelObject.transform)
        {
            roomList.Add(child.GetComponent<Collider>().bounds);
        }

        foreach (Bounds room in roomList)
        {
            foreach (Bounds roomAgainst in roomList)
            {
                if (room != roomAgainst)
                {
                    if (room.Intersects(roomAgainst))
                    {
                        roomCollision = true;
                    }
                }
            }
        }

    }

    void SpawnKey()
    {
        if (!keySpawned)
        {
            GameObject[] keySpawns = GameObject.FindGameObjectsWithTag("KeySpawn");

            GameObject chosenSpawn = new GameObject();

            foreach (GameObject spawn in keySpawns)
            {
                if (chosenSpawn == null)
                {
                    chosenSpawn = spawn;
                }
                else
                {
                    if (Vector3.Distance(Vector3.zero, chosenSpawn.transform.position) < Vector3.Distance(Vector3.zero, spawn.transform.position))
                    {
                        chosenSpawn = spawn;
                    }
                }
            }

            GameObject keyObject = Instantiate(key, chosenSpawn.transform.position, Quaternion.Euler(0, 0, 90f));
            keyObject.transform.parent = levelObject.transform;

            keySpawned = true;
        }
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