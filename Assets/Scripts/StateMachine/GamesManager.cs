using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GamesManager : StateMachine
{
    public static GamesManager instance;
    public delegate void LevelUpEvent();
    public static event LevelUpEvent OnLevelUp;

    public PlayerMovement myPlayer;
    public PropRandomizer propRandomizer;
    public int level;
    public int kills;
    public int xp; 
    public int levelUpPoint = 60; 
    public TMP_Text xpNumber;      
    public TMP_Text killsNumber;   
    public TMP_Text levelNumber;

    public Button healthUpgradeButton;
    private int healthUpgradeCost = 80;  // Starting cost for health upgrade
    private bool hasPurchasedHealthUpgrade = false;
    public TMP_Text upgradeText;   // Description text for the upgrade
    public TMP_Text priceText;     // Text to display the cost of the upgrade
    public Button upgradeButton;
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

        SetupUpgradeUI();

        UpdateUI();
    }
    private void SetupUpgradeUI()
    {
        // Set static upgrade description
        upgradeText.text = "+ 5 Health";

        // Set initial price text
        priceText.text = $"{healthUpgradeCost} XP";

        // Add the listener for the upgrade button
        healthUpgradeButton.onClick.AddListener(HealthUpgrade);

        // Initial update of the upgrade button state
        UpdateUpgradeButtons();
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

        hasPurchasedHealthUpgrade = false;

        UpdateUpgradeButtons();

    }
    private void SpawnPropsAfterLevelUp()
    {
        propRandomizer.SpawnProps();
    }
    public void PrepareNextLevel()
    {
        int currentLevel = level;
        int currentKills = kills;
        int currentXP = xp;
        int currentLevelUpPoint = levelUpPoint;

        // Reset game state
        ResetEnemies();
        //ResetGameObjects();


        Debug.Log("Hello");
        // Restore the important stats
        level = currentLevel; // Keep current level
        kills = currentKills; // Keep current kills
        xp = currentXP;       // Keep current XP
        levelUpPoint = currentLevelUpPoint;       // Keep current XP

        myPlayer.transform.position = myPlayer.startingPosition;
        Debug.Log("Hello");
        propRandomizer.SpawnProps();

        UpdateUI();
    }
    private void ResetEnemies()
    {
        // Find all enemies and disable or destroy them
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        foreach (Enemy enemy in enemies)
        {
            ObjectPoolManager.RetrunObjectToPool(enemy.gameObject);
        }

    }
    public void HealthUpgrade()
    {
        if (xp >= healthUpgradeCost && !hasPurchasedHealthUpgrade)
        {
            xp -= healthUpgradeCost;  // Deduct XP
            xpNumber.text = xp.ToString();  // Update XP display

            myPlayer.IncreaseMaxHealth(5);  // Increase player's max health

            hasPurchasedHealthUpgrade = true;  // Mark as purchased for this level
            healthUpgradeCost *= 2;  // Double the price for the next level

            // Update price text and disable the button after purchase
            priceText.text = $"Cost: {healthUpgradeCost} XP";
            upgradeButton.interactable = false;
        }
    }

    public void UpdateUpgradeButtons()
    {
        // Enable/disable the upgrade button and update price text dynamically
        if (xp >= healthUpgradeCost && !hasPurchasedHealthUpgrade)
        {
            upgradeButton.interactable = true;  // Enable button if player can buy
        }
        else
        {
            upgradeButton.interactable = false;  // Disable button if player can't buy
        }
            priceText.text = $"Cost: {healthUpgradeCost} XP";  // Update price
    }

}
