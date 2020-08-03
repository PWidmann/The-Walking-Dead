using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameInterface : MonoBehaviour
{
    public static GameInterface Instance;

    public GameObject mapGeneratorPanel;
    public GameObject welcomeScreen;
    public GameObject titleImage;
    public GameObject mapCam;
    public GameObject gameCam;
    public GameObject keyImage;
    public GameObject messagePanel;
    

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

        ShowKey();

        if (Input.GetKeyDown(KeyCode.M))
        {
            ShowMessage("You need a key!");
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

    public void ShowKey()
    {
        if (GameManager.HasKey)
        {
            keyImage.SetActive(true);
        }
        else
        {
            keyImage.SetActive(false);
        }
    }

    public void ShowMessage(string message)
    {
        if (message != null)
        {
            messagePanel.SetActive(true);
            messagePanel.GetComponent<Message>().ShowMessage(message);
        }
    }
}
