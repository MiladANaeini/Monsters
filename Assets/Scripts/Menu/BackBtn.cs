using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackBtn : MonoBehaviour
{
    public Menu menu;
  public void OnEventBack()
    {
        if (menu != null) {
            menu.SwitchMenuState(GamesManager.instance.previousState);
            Debug.Log($"Going back to previous state: {GamesManager.instance.previousState}");
        }
    }
}
