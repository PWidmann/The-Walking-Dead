﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    
    void Update()
    {
        RotateKey();

        if (PlayerController.Instance)
        {
            if (Vector3.Distance(PlayerController.Instance.transform.position, transform.position) < 1f)
            {
                GameManager.HasKey = true;
                Destroy(gameObject);
                Debug.Log("Picked up key!");
            }
        }
    }

    void RotateKey()
    {
        transform.Rotate(Time.deltaTime * 25, 0, 0, Space.Self);
    }
}
