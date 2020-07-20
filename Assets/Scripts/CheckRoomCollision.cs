using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckRoomCollision : MonoBehaviour
{
    public bool isColliding = false;

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Room")
        {
            isColliding = true;
            Debug.Log("Room is colliding");
        }
    }
}
