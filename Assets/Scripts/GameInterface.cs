using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInterface : MonoBehaviour
{
    public static GameInterface Instance;

    public GameObject mapGeneratorPanel;
    public GameObject welcomeScreen;
    public GameObject titleImage;
    public GameObject mapCam;
    public GameObject gameCam;


    public bool welcomeScreenShown = false;

    void Start()
    {
        if (Instance == null)
            Instance = this;

        mapGeneratorPanel.SetActive(true);
    }

    void Update()
    {
        if (Input.GetButtonDown("JoystickButtonA") && !MapGenerator.Instance.levelStarted)
        {
            StartGame();
        }

        if (PlayerController.Instance)
        {
            if (!welcomeScreenShown && PlayerController.Instance.controller.isGrounded)
            {
                welcomeScreen.SetActive(true);
                welcomeScreenShown = true;
            }
        }
        
    }

    public void StartGame()
    {
        mapGeneratorPanel.SetActive(false);
        gameCam.SetActive(true);
        mapCam.SetActive(false);

        MapGenerator.Instance.levelMeshSurface.BuildNavMesh();
        MapGenerator.Instance.SpawnPlayer();
        MapGenerator.Instance.levelStarted = true;
    }

    public void SetCanMove()
    {
        welcomeScreen.SetActive(false);
    }

    public void ShowWelcomeScreen()
    {
        welcomeScreen.SetActive(true);
    }
}
