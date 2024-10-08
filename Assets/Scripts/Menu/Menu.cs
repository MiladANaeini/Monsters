using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class Menu : MonoBehaviour
{
  public enum MenuState
    {
        None = 0,
        mainMenu = 1,
        pauseMenu = 2,
        optionsMenu = 3,
        gameOverMenu = 4,
        levelUpMenu = 5,
    }

    public MenuState state;
    private bool isPaused = false;
    public GameObject mainMenuUI;
    public GameObject pauseMenuUI;
    public GameObject optionsMenuUI;
    public GameObject gameOverMenuUI;
    public GameObject levelUpMenuUI;
    public TMP_Text killsText;
    public TMP_Text levelText;
    public static bool gameHasStarted = false;
    private MenuState previousState; // Added variable
    private Dictionary<MenuState, GameObject> menuUIs;
    private void Awake()
    {
        // Initialize the dictionary with all your menus
  
    }
    private void Start()
    {
        menuUIs = new Dictionary<MenuState, GameObject>
    {
        { MenuState.mainMenu, mainMenuUI },
        { MenuState.pauseMenu, pauseMenuUI },
        { MenuState.optionsMenu, optionsMenuUI },
        { MenuState.gameOverMenu, gameOverMenuUI },
        { MenuState.levelUpMenu, levelUpMenuUI },
    };
        GamesManager.OnLevelUp += HandleLevelUp;

        if (!Menu.gameHasStarted) // Access the static variable
        {
            SwitchMenuState(MenuState.mainMenu);
            GamesManager.instance.switchState<PauseState>();
        }
        else
        {
            SwitchMenuState(MenuState.None); // No menu shown if game has started
            GamesManager.instance.switchState<PlayingState>();
        }

    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (state != MenuState.optionsMenu)
            {
                isPaused = !isPaused;

                if (isPaused)
                {
                    PauseGame();  
                }
                else
                {
                    ResumeGame();  
                }
            }
        }
    }

    public void PauseGame()
    {
        isPaused = true;
        SwitchMenuState(MenuState.pauseMenu);
        //Time.timeScale = 0f;
    }
    public void ResumeGame()
    {
        //Time.timeScale = 1f;
        isPaused = false;
        SwitchMenuState(MenuState.None);
    }
    public void HandleLevelUp()
    {
        SwitchMenuState(MenuState.levelUpMenu);
        GamesManager.instance.switchState<PauseState>();

        //Time.timeScale = 0f;
    }
    public void GameOver()
    {
        isPaused = true;
        SwitchMenuState(MenuState.gameOverMenu);
        UpdateGameOverUI();
        //Time.timeScale = 0f;
    }

    public void SwitchMenuState(MenuState aState)
    {
        GamesManager.instance.previousState = state;

        foreach (var menu in menuUIs)
        {
            menu.Value.SetActive(menu.Key == aState);
        }
        state = aState;
        Debug.Log("Current state: " + state);
    }

    private void UpdateGameOverUI()
    {
        killsText.text = "Kills: " + GamesManager.instance.kills.ToString(); 
        levelText.text = "Level: " + GamesManager.instance.level.ToString(); 
    }
}
