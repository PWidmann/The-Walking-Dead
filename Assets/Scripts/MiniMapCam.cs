﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapCam : MonoBehaviour
{
    public static MiniMapCam Instance;

    public Transform target;

    private void Start()
    {
        if (Instance == null)
            Instance = this;
    }

    private void Update()
    {
        
        if (target == null)
        {
            FindPlayer();
        }
    }

    void LateUpdate()
    {
        if (target)
            transform.position = target.position - transform.forward * 20;
    }

    public void FindPlayer()
    {
        if (GameObject.FindGameObjectWithTag("Player"))
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }
}
