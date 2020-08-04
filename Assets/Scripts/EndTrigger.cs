using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndTrigger : MonoBehaviour
{
    public float triggerRadius = 1f;

    private float distance = 100;

    private void Update()
    {
        if (PlayerController.Instance)
        {
            distance = Vector3.Distance(transform.position, PlayerController.Instance.position);
        }

        if (distance < triggerRadius)
        {
            GameManager.EndReached = true;
            Debug.Log("End Reached");
        }
    }
}
