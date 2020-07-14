using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public static MapGenerator Instance;

    [Header("Level Size")]
    public Vector2 levelSize;

    [Header("Level Prefabs")]
    public GameObject floor_stone;
    public GameObject pillar;

    [Header("Level Object")]
    public GameObject levelObject;


    void Start()
    {
        if (Instance == null)
            Instance = this;

        Room startRoom = new Room(RoomType.type.StartRoom);
    }

    
    void Update()
    {
        
    }

    
}
