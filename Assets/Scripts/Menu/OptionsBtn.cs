using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsBtn : ButtonAnimation
{
    public GameObject optionsMenu;
    public Menu menu;

    private Menu.MenuState previousMenuState;

    public void EnterOptions()
    {
        previousMenuState = menu.state;
        Debug.Log($"Entering options from {previousMenuState}");

        menu.switchState(Menu.MenuState.optionsMenu);
        optionsMenu.SetActive(true);
    }

  
}
