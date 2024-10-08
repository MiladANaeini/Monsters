using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Menu;

public class RestartBtn : ButtonAnimation
{
    public Menu menu;

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        menu.SwitchMenuState(MenuState.None);
        Menu.gameHasStarted = true;
        GamesManager.instance.switchState<PlayingState>();

    }
}
