using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextLevelBtn : ButtonAnimation
{
    public void NextLevel()
    {
        // Call method in GamesManager to reset enemies and prepare for the next level
        GamesManager.instance.PrepareNextLevel();
    }
}
