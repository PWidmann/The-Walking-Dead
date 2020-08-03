using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomDiscoverSphere : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Room")
        {
            Debug.Log("Collided with a room");
            MoveToLayer(other.gameObject.transform, 9);
        }          
    }

    void MoveToLayer(Transform root, int layer)
    {
        root.gameObject.layer = layer;
        foreach (Transform child in root)
            MoveToLayer(child, layer);
    }
}
