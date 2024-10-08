using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class Buildings : Health
{
    public GameObject buildingHit;
    protected override void Start()
    {
        maxHealth = GamesManager.instance.buildingsHealth;
        base.Start();
        onHealthChanged += UpdateSprite;
    }

    public void UpdateSprite()
    {
        //the hit particles for buildings
        GameObject hitParticles = Instantiate(buildingHit, transform.position, Quaternion.identity);
        Destroy(hitParticles, 2f);

        if (health <= 0)
        {
            GamesManager.instance.OnBuildingDestroyed();
            ObjectPoolManager.RetrunObjectToPool(gameObject);

        }
    }

}

