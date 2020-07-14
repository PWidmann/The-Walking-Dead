using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    int roomWidth;
    int roomHeight;


    public Room(RoomType.type type)
    {
        switch (type)
        {
            case RoomType.type.StartRoom:
                CreateStartRoom();
                break;
        }
    }

    void CreateStartRoom()
    {
        roomWidth = roomHeight = 5;

        // Create Floor
        for (int x = 0; x < roomWidth; x++)
        {
            for (int y = 0; y < roomHeight; y++)
            {
                var stoneFloor = Instantiate(MapGenerator.Instance.floor_stone, new Vector3(x * 2, 0, y * 2), Quaternion.identity * Quaternion.Euler(90f, RandomFloorRotation(), 0f));
                stoneFloor.transform.parent = MapGenerator.Instance.levelObject.transform;
            }
        }
    }

    float RandomFloorRotation()
    {
        float angle = 90f;

        int rnd = Random.Range(0, 3);

        angle *= rnd;

        return angle;
    }
}
