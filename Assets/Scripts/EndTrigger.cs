using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("You have escaped the dungeon!");
        PlayerController.Instance.canMove = false;
    }
}
