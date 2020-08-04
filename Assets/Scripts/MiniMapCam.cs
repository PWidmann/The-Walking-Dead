using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapCam : MonoBehaviour
{
    Transform target;
    bool targetSet = false;

    private void Update()
    {
        // Waiting for the player at the start, won't be called after that
        if (!targetSet)
        {
            if (GameObject.FindGameObjectWithTag("Player"))
            {
                target = GameObject.FindGameObjectWithTag("Player").transform;
                targetSet = true;
            }
        }
    }

    void LateUpdate()
    {
        if (target)
            transform.position = target.position - transform.forward * 20;
    }
}
