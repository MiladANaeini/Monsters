using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsBtn : ButtonAnimation
{
    public Menu menu;


    public void EnterOptions()
    {
        if (menu != null)
        {
            menu.SwitchMenuState(Menu.MenuState.optionsMenu);
        }
    }

  
}
