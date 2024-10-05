using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GamesManager : StateMachine
{
    public static GamesManager instance;
    public delegate void LevelUpEvent();
    public static event LevelUpEvent OnLevelUp;

    public PlayerMovement myPlayer;
    public PropRandomizer propRandomizer;
    public int score;
    public int level;
    public int kills;
    public int xp; 
    public int levelUpPoint = 60; 
    public TMP_Text xpNumber;      
    public TMP_Text killsNumber;   
    public TMP_Text levelNumber;

    private void Awake()
    {
        instance = this;// Singleton instance
    }

    private void Start()
    {
        propRandomizer = FindObjectOfType<PropRandomizer>();
        if (!Menu.gameHasStarted)
        {
            switchState<PauseState>();
        }
        level = 1;
        kills = 0;
        xp = 0;
        levelUpPoint = 60;

        UpdateUI();
    }
    private void UpdateUI()
    {
        killsNumber.text = kills.ToString();

        xpNumber.text = xp.ToString();

        levelNumber.text = level.ToString();
    }
    private void Update()
    {
        updateStateMachine();
    }
    public void OnEnemyDestroyed()
    {
        kills++; 

        killsNumber.text = kills.ToString(); 
    }
    public void OnBuildingDestroyed()
    {
        xp += 10;
        xpNumber.text = xp.ToString();
        if (xp >= levelUpPoint)
        {
            LevelUp();
        }
    } 
    private void LevelUp()
    {
        level++;
        levelUpPoint += 60; 

        levelNumber.text = level.ToString();
        OnLevelUp?.Invoke();

        Invoke("SpawnPropsAfterLevelUp", 3f);

    }
    private void SpawnPropsAfterLevelUp()
    {
        propRandomizer.SpawnProps();
    }


}
