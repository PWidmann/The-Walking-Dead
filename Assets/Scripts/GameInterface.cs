using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInterface : MonoBehaviour
{
    public GameObject mapGeneratorPanel;

    public GameObject mapCam;
    public GameObject gameCam;


    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetButtonDown("JoystickButtonA") && !MapGenerator.Instance.levelStarted)
        {
            StartGame();
        }
    }

    public void StartGame()
    {
        mapGeneratorPanel.SetActive(false);
        gameCam.SetActive(true);
        mapCam.SetActive(false);
        
        MapGenerator.Instance.SpawnPlayer();
        MapGenerator.Instance.levelStarted = true;
    }
}
