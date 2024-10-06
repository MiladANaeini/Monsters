using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Upgrades : MonoBehaviour
{
    public new string name;
    public int currentCost;
    public int level = 1;
    public int baseCost;
    public int healthIncrease;
    public Button button;

    public Upgrades(string upgradeName, int baseUpgradeCost, int healthIncreaseAmount, Button upgradeButton)
    {
        name = upgradeName;
        baseCost = baseUpgradeCost;
        currentCost = baseCost;  // Initial cost is set to the base cost
        healthIncrease = healthIncreaseAmount;
        button = upgradeButton;
    }

    // Method to calculate next cost of upgrade after purchase
    public void LevelUp()
    {
        level++;
        currentCost = baseCost * level; // Cost doubles each time
    }

    // Method to disable the upgrade button
    public void Disable()
    {
        button.interactable = false;
    }

    // Method to enable the upgrade button
    public void Enable()
    {
        button.interactable = true;
    }
}
