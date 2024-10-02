using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class Buildings : Health
{
    public GameObject buildingHit;
    public PlayerMovement myPlayer;
    public TMP_Text xpNumber;
    public GameObject pointsUI;

    protected override void Start()
    {
        base.Start();
        GameObject playerObject = GameObject.FindWithTag("Player");
        if (playerObject != null)
        {
            // Get the PlayerMovement component
            myPlayer = playerObject.GetComponent<PlayerMovement>();

            if (myPlayer == null)
            {
                Debug.LogError("PlayerMovement component not found on the player object!");
            }
        }
        else
        {
            Debug.LogError("Player object with tag 'Player' not found in the scene!");
        }
        pointsUI = GameObject.Find("Points");
        if (pointsUI != null)
        {
            xpNumber = pointsUI.transform.Find("XPNumber").GetComponent<TMP_Text>();
        }
        else
        {
            Debug.LogError("PointsUI GameObject not found in the scene!");
        }
        onHealthChanged += UpdateSprite;
    }

    public void UpdateSprite()
    {
        //the hit particles for buildings
        GameObject hitParticles = Instantiate(buildingHit, transform.position, Quaternion.identity);
        Destroy(hitParticles, 2f);

        if (health <= 0)
        {
            if (myPlayer != null)
            {
                myPlayer.score += 10;
                xpNumber.text = myPlayer.score.ToString();
                Debug.Log("Player score updated: " + myPlayer.score);
            }
            else
            {
                Debug.LogWarning("PlayerMovement component not found");
            }
            ObjectPoolManager.RetrunObjectToPool(gameObject);
        }
    }
}
