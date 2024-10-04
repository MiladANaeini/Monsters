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
    public void OnBuildingDestroyed()
    {
        AddXP(10); // Add 10 XP per building destroyed
    } 
    public void OnEnemyDestroyed()
    {
        kills++; // Increment the kill count

        killsNumber.text = kills.ToString(); // Update UI
    }
    public void AddXP(int points)
    {
        xp += points;
        xpNumber.text = xp.ToString(); // Update XP UI
        CheckForLevelUp();
    }
    public void AddKill()
    {
        kills++;

        // Update the kills number UI
        killsNumber.text = kills.ToString();
    }

    private void CheckForLevelUp()
    {
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
        // Optionally, update building health or difficulty after level-up
        //UpdateBuildingHealth();

        Debug.Log("Player leveled up to: " + level);
        Debug.Log("Next level-up point at: " + levelUpPoint);
    }
    private void SpawnPropsAfterLevelUp()
    {
        propRandomizer.SpawnProps();
    }
    //private void UpdateBuildingHealth()
    //{
    //    foreach (Buildings building in allBuildings)
    //    {
    //        if (building != null)
    //        {
    //            building.IncreaseHealth(level); // Example: Scale building health with player level
    //        }
    //    }
    //}
}
