using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Menu;

public class RestartBtn : ButtonAnimation
{
    public Menu menu;
    public GameObject pauseMenuUI;


    public void RestartGame()
    {
        Menu.gameHasStarted = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    }
}
