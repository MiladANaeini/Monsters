using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Menu;

public class GamesManager : StateMachine
{
    public static GamesManager instance;
    public delegate void LevelUpEvent();
    public static event LevelUpEvent OnLevelUp;

    public PlayerMovement myPlayer;
    public PropRandomizer propRandomizer;
    public List<GunController> gunControllers;
    public Buildings buildings;
    public BulletProjectile bulletProjectile;
    public int level;
    public int kills;
    public int xp; 
    public int myLevelUpPonts; //check for required points
    public int levelUpPoint = 60; 
    public TMP_Text xpNumber;      
    public TMP_Text killsNumber;   
    public TMP_Text levelNumber;

    public int buildingsHealth;

    public Button healthUpgradeButton;
    public Button gunUpgradeButton;
    private int healthUpgradeCost = 80;  // Starting cost for health upgrade
    private int gunUpgradeCost = 80;  // Starting cost for health upgrade
    private bool hasPurchasedHealthUpgrade = false;
    private bool hasPurchasedGunUpgrade = false;

    public TMP_Text upgradeHealthText;   
    public TMP_Text upgradeHealthPriceText;
    
    public TMP_Text upgradeGunText;   
    public TMP_Text upgradeGunPriceText;
    public Menu menu;
    public Menu.MenuState previousState;

    private void Awake()
    {
        instance = this;// Singleton instance
    }

    private void Start()
    {
        bulletProjectile = FindObjectOfType<BulletProjectile>();
        propRandomizer = FindObjectOfType<PropRandomizer>();
        gunControllers = new List<GunController>(FindObjectsOfType<GunController>());
        buildings = FindObjectOfType<Buildings>();
        menu = FindObjectOfType<Menu>();

        level = 1;
        kills = 0;
        xp = 0;
        myLevelUpPonts = 0;
        levelUpPoint = 60;
        buildingsHealth = 1;
       
        previousState = Menu.MenuState.mainMenu; 

        SetupUpgradeUI();
        propRandomizer.SpawnProps();
        UpdateUI();
    }
    private void SetupUpgradeUI()
    {
        // Set static upgrade description
        upgradeHealthText.text = "+ 5 Health";
        upgradeGunText.text = "2X F Rate";

        // Set initial price text
        upgradeHealthPriceText.text = $"{healthUpgradeCost} XP";
        upgradeGunPriceText.text = $"{gunUpgradeCost} XP";

        // Add the listener for the upgrade button
        healthUpgradeButton.onClick.AddListener(HealthUpgrade);
        gunUpgradeButton.onClick.AddListener(GunUpgrade);
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
        myLevelUpPonts += 10;
        xpNumber.text = xp.ToString();
        if (myLevelUpPonts >= levelUpPoint)
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
        hasPurchasedGunUpgrade = false;

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
        int currentmyLevelUp = myLevelUpPonts;
        int currentLevelUpPoint = levelUpPoint;

        // Reset game state
        ResetEnemies();
        ResetProjectiles();

        // Restore the important stats
        level = currentLevel; // Keep current level
        kills = currentKills; // Keep current kills
        xp = currentXP;       // Keep current XP
        myLevelUpPonts = currentmyLevelUp;       // Keep current XP
        levelUpPoint = currentLevelUpPoint;       // Keep current XP

        myPlayer.transform.position = myPlayer.startingPosition;
        ResetGunControllersPositions();

        propRandomizer.SpawnProps();
        buildingsHealth = level * buildingsHealth;

        UpdateUI();
    }
    private void ResetGunControllersPositions()
    {
        foreach (GunController gunController in gunControllers)
        {
            if (gunController != null)
            {
                gunController.transform.position = gunController.startingPosition; // Reset to starting position
            }
        }
    }
    private void ResetEnemies()
    {
        // Find all enemies and disable or destroy them
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        foreach (Enemy enemy in enemies)
        {
            enemy.ResetHealth();
            ObjectPoolManager.RetrunObjectToPool(enemy.gameObject);
        }

    }
    private void ResetProjectiles()
    {
        BulletProjectile[] projectiles = FindObjectsOfType<BulletProjectile>();
        foreach (BulletProjectile projectile in projectiles)
        {
            ObjectPoolManager.RetrunObjectToPool(projectile.gameObject);
        }
    }
    public void HealthUpgrade()
    {
        if (xp >= healthUpgradeCost && !hasPurchasedHealthUpgrade)
        {
            xp -= healthUpgradeCost;  
            xpNumber.text = xp.ToString(); 

            myPlayer.IncreaseMaxHealth(5);  

            hasPurchasedHealthUpgrade = true;  
            healthUpgradeCost *= 2;

            // Update price text and disable the button after purchase
            upgradeHealthPriceText.text = $"Cost: {healthUpgradeCost} XP";
            healthUpgradeButton.interactable = false;
        }
    }    public void GunUpgrade()
    {
        if (xp >= gunUpgradeCost && !hasPurchasedGunUpgrade)
        {
            xp -= gunUpgradeCost;  
            xpNumber.text = xp.ToString();

            foreach (GunController gunController in gunControllers) // Update to loop through all GunControllers
            {
                gunController.IncreaseShootingRate(2);
            }
            hasPurchasedGunUpgrade = true;  
            gunUpgradeCost *= 2;

            // Update price text and disable the button after purchase
            upgradeGunPriceText.text = $"Cost: {gunUpgradeCost} XP";
            gunUpgradeButton.interactable = false;
        }
    }
    public void UpdateUpgradeButtons()
    {
       
        if (xp >= healthUpgradeCost && !hasPurchasedHealthUpgrade)
        {
            healthUpgradeButton.interactable = true; 
        }
        else
        {
            healthUpgradeButton.interactable = false;  
        }
        upgradeHealthPriceText.text = $"Cost: {healthUpgradeCost} XP"; 
        
        if (xp >= gunUpgradeCost && !hasPurchasedGunUpgrade)
        {
            gunUpgradeButton.interactable = true; 
        }
        else
        {
            gunUpgradeButton.interactable = false;  
        }
        upgradeGunPriceText.text = $"Cost: {gunUpgradeCost} XP";  
    }

}
