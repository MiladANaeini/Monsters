using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseState : State
{
    public override void enterState()
    {
        base.enterState();
        //Time.timeScale = 0f; // Will be removed 

    }

    public override void updateState()
    {
        base.updateState();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GamesManager.instance.switchState<PlayingState>();
        }
    }

}
