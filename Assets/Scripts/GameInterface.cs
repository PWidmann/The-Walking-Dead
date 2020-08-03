using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;

public class GameInterface : MonoBehaviour
{
    public static GameInterface Instance;

    [Range(0, 1)]
    public float miniMapAlpha;

    public GameObject mapGeneratorPanel;
    public GameObject welcomeScreen;
    public GameObject titleImage;
    public GameObject mapCam;
    public GameObject gameCam;
    public GameObject keyImage;
    public GameObject messagePanel;
    public GameObject miniMap;
    public RawImage miniMapImage;
    public GameObject deathPanel;

    private bool welcomeScreenShown = false;
    private bool miniMapActive = false;


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
            // Death Screen
            if (PlayerController.Instance.Alive == false)
                deathPanel.SetActive(true);

            if (!welcomeScreenShown && PlayerController.Instance.controller.isGrounded)
            {
                welcomeScreen.SetActive(true);
                welcomeScreenShown = true;
            }
        }

        ShowKey();
        MiniMap();
    }

    public void StartGame()
    {
        mapGeneratorPanel.SetActive(false);
        gameCam.SetActive(true);
        mapCam.SetActive(false);

        MapGenerator.Instance.levelMeshSurface.BuildNavMesh();
        MapGenerator.Instance.SpawnPlayer();
        MapGenerator.Instance.levelStarted = true;
        GameManager.LevelStarted = true;
    }

    public void SetCanMove()
    {
        welcomeScreen.SetActive(false);
    }

    public void ShowWelcomeScreen()
    {
        welcomeScreen.SetActive(true);
    }

    public void MiniMap()
    {
        SetImageAlpha();

        if (GameManager.LevelStarted)
        {
            if (Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyDown(KeyCode.M) || Input.GetButtonDown("JoystickButtonY"))
                miniMapActive = !miniMapActive;

            if (miniMapActive)
                miniMap.SetActive(true);
            else
                miniMap.SetActive(false);
        }
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

    public void SetImageAlpha()
    {
        var tempColor = miniMapImage.color;
        tempColor.a = miniMapAlpha;
        miniMapImage.color = tempColor;
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
