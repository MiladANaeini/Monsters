using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamesManager : StateMachine
{
    public static GamesManager instance;

    public PlayerMovement myPlayer;
    public Enemy myEnemy;
    private void Awake()
    {
        instance = this;// Singleton instance
    }

    private void Start()
    {
        if (!Menu.gameHasStarted)
        {
            switchState<PauseState>();
        }
    }

    private void Update()
    {
        updateStateMachine();
    }
}
