using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
    public GameObject miniMap;
    public Camera miniMapCam;
    public RawImage miniMapImage;
    public GameObject deathPanel;
    public GameObject winScreen;
    public GameObject pauseMenuPanel;
    public GameObject pauseText;
    public Slider miniMapAlphaSlider;
    public Text miniMapAlphaValueText;

    private bool welcomeScreenShown = false;
    private bool miniMapActive = false;
    private Vector3 respawnPosition;
    private GameObject player;



    void Start()
    {
        if (Instance == null)
            Instance = this;

        mapGeneratorPanel.SetActive(true);

        if (GameManager.LevelStarted && respawnPosition == null)
        {
            respawnPosition = GameObject.FindGameObjectWithTag("Respawn").transform.position;
        }
            

        if (PlayerPrefs.HasKey("MiniMapAlpha"))
        {
            miniMapAlphaSlider.value = PlayerPrefs.GetFloat("MiniMapAlpha");
        }
    }

    void Update()
    {
        if (Input.GetButtonDown("JoystickButtonA") && !MapGenerator.Instance.levelStarted)
        {
            StartGame();
        }

        // Escape Menu
        if (GameManager.LevelStarted && Input.GetKeyDown(KeyCode.Escape))
        {
            GameManager.IsInPauseMenu = !GameManager.IsInPauseMenu;
            
            if (!GameManager.IsInPauseMenu)
            {
                PlayerPrefs.SetFloat("SoundVolume", GameManager.SoundVolume);
                PlayerPrefs.SetFloat("MiniMapAlpha", GameManager.MiniMapAlpha);
            }
        }


        DeathScreen();
        WelcomeScreen();
        ShowKey();
        MiniMap();
        PauseMenu();
        WinScreen();
    }

    public void StartGame()
    {
        if (MapGenerator.Instance.dungeonGenerated)
        {
            mapGeneratorPanel.SetActive(false);
            gameCam.SetActive(true);
            mapCam.SetActive(false);

            MapGenerator.Instance.levelMeshSurface.BuildNavMesh();
            MapGenerator.Instance.SpawnPlayer();
            MapGenerator.Instance.SpawnEnemies();
            MapGenerator.Instance.SpawnKey();
            MapGenerator.Instance.levelStarted = true;
            GameManager.LevelStarted = true;
        }
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
            {
                miniMap.SetActive(true);
                miniMapCam.enabled = true;
            }
            else
            {
                miniMap.SetActive(false);
                miniMapCam.enabled = false;
            }
        }
    }

    public void WelcomeScreen()
    {
        if (PlayerController.Instance)
        {
            // Welcome Screen
            if (!welcomeScreenShown && PlayerController.Instance.controller.isGrounded)
            {
                welcomeScreen.SetActive(true);
                welcomeScreenShown = true;
                GameManager.WelcomeScreen = true;
            }

            if (GameManager.WelcomeScreen)
            {
                if (Input.GetKeyDown(KeyCode.Backspace) || Input.GetButtonDown("JoystickButtonA"))
                {
                    welcomeScreen.SetActive(false);
                    GameManager.WelcomeScreen = false;
                }
            }
        }
    }

    public void WinScreen()
    {
        if (GameManager.EndReached)
        {
            winScreen.SetActive(true);
            Time.timeScale = 0;

            if (Input.GetButtonDown("JoystickButtonA"))
            {
                NewGame();
            }

            if (Input.GetButtonDown("JoystickButtonX"))
            {
                QuitGame();
            }
        }
        else
        {
            winScreen.SetActive(false);
        }
    }

    public void DeathScreen()
    {
        // Death Screen
        if (PlayerController.Instance)
        {
            if (PlayerController.Instance.Alive == false)
            {
                deathPanel.SetActive(true);

                //Reset player to start
                if (Input.GetKeyDown(KeyCode.R) || Input.GetButtonDown("JoystickButtonB"))
                {
                    Debug.Log("Resetting player...");
                    GameManager.ResetEnemies = true;
                    player = GameObject.FindGameObjectWithTag("Player");
                    Destroy(player);
                    MapGenerator.Instance.RespawnPlayer();
                    MiniMapCam.Instance.FindPlayer();
                    deathPanel.SetActive(false);
                }
            }
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
        tempColor.a = GameManager.MiniMapAlpha;
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

    void PauseMenu()
    {
        if (GameManager.IsInPauseMenu)
        {
            GameManager.MiniMapAlpha = miniMapAlphaSlider.value;
            miniMapAlphaValueText.text = System.Math.Round(GameManager.MiniMapAlpha, 2).ToString();

            pauseMenuPanel.SetActive(true);
            pauseText.SetActive(true);
            Time.timeScale = 0;
        }
        else
        {
            pauseMenuPanel.SetActive(false);
            pauseText.SetActive(false);
            Time.timeScale = 1;
        }
    }

    public void NewGame()
    {
        GameManager.EndReached = false;
        GameManager.LevelStarted = false;
        Time.timeScale = 1;
        winScreen.SetActive(false);
        GameManager.IsInPauseMenu = false;
        SceneManager.LoadScene(0);
    }

    public void ContinueGame()
    {
        GameManager.IsInPauseMenu = false;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
