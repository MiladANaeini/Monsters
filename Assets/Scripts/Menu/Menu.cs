using System.Collections;
using System.Collections.Generic;
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
    public static bool gameHasStarted = false;
    private MenuState previousState; // Added variable
    private Dictionary<MenuState, GameObject> menuUIs;
    private void Awake()
    {
        // Initialize the dictionary with all your menus
        menuUIs = new Dictionary<MenuState, GameObject>
    {
        { MenuState.mainMenu, mainMenuUI },
        { MenuState.pauseMenu, pauseMenuUI },
        { MenuState.optionsMenu, optionsMenuUI },
        { MenuState.gameOverMenu, gameOverMenuUI },
        { MenuState.levelUpMenu, levelUpMenuUI },
    };
    }
    private void Start()
    {
        GamesManager.OnLevelUp += HandleLevelUp;
        if (!gameHasStarted)
        {
            switchState(MenuState.mainMenu);
            GamesManager.instance.switchState<PauseState>();
        }
        else
        {
            switchState(MenuState.pauseMenu);
            GamesManager.instance.switchState<PlayingState>();
            pauseMenuUI.SetActive(false);
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
        switchState(MenuState.pauseMenu);
        Time.timeScale = 0f;
    }
    public void ResumeGame()
    {
        Time.timeScale = 1f;
        isPaused = false;
        switchState(MenuState.None);
    }
    public void HandleLevelUp()
    {
        isPaused = true;
        switchState(MenuState.levelUpMenu);
        Time.timeScale = 0f;
    }


    public void switchState(MenuState aState)
    {
        foreach (var menu in menuUIs)
        {
            menu.Value.SetActive(menu.Key == aState);
        }
        state = aState;
        Debug.Log("Current state: " + state);
    }
    public void Back()
    {
        optionsMenuUI.SetActive(false);

        Debug.Log($"Going back to previous state: {previousState}");

        if (state != Menu.MenuState.None)
        {
            switchState(state);
        }
        else
        {
            Debug.LogError("Previous state is invalid!");
            return;
        }

        if (state == MenuState.mainMenu)
        {
            mainMenuUI.SetActive(true);
        }
        else if (state == MenuState.pauseMenu)
        {
            pauseMenuUI.SetActive(true);
        }

        Debug.Log($"Current state after back: {state}");
    }
}
