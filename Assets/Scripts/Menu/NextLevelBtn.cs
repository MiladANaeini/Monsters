using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static Menu;

public class NextLevelBtn : ButtonAnimation
{
    public Menu menu;

    public void NextLevel()
    {
        // Call method in GamesManager to reset enemies and prepare for the next level
        GamesManager.instance.PrepareNextLevel();
        menu.SwitchMenuState(MenuState.None);
        GamesManager.instance.switchState<PlayingState>();

    }
}
