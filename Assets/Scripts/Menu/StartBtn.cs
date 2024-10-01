using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Menu;


public class StartBtn : ButtonAnimation
{
    public Menu menu;


    public void StartGame()
    {
        menu.switchState(MenuState.None);
        GamesManager.instance.switchState<PlayingState>();
    }
}
