using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GamesManager : StateMachine
{
    public static GamesManager instance;

    public PlayerMovement myPlayer;
    public PropRandomizer propRandomizer;
    public int score;
    public int level;
    public int kills;
    public int xp; // Track XP separately
    public int levelUpPoint = 60; // Initial level-up point
    public TMP_Text xpNumber;      // Reference to the XP UI element
    public TMP_Text killsNumber;   // Reference to the Kills UI element
    public TMP_Text levelNumber;
    public List<Buildings> allBuildings;
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
        // Update Kills UI
        killsNumber.text = kills.ToString();

        // Update BuldingsXP UI
        xpNumber.text = xp.ToString();

        // Update Level UI
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
        Debug.Log("xp" + xp);
        Debug.Log("levelUpPoint" + levelUpPoint);
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
        Invoke("SpawnPropsAfterLevelUp", 3f);

    }
    private void SpawnPropsAfterLevelUp()
    {
        propRandomizer.SpawnProps();
    }
}
