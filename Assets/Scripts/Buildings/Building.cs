using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class Buildings : Health
{
    public GameObject buildingHit;
    private UpdateSpriteOnHit updateSpriteOnHit;

    protected override void Start()
    {
        base.Start();  
        updateSpriteOnHit = GetComponent<UpdateSpriteOnHit>();

        onHealthChanged += UpdateSprite;
    }

    public void UpdateSprite()
    {
        //the hit particles for buildings
        GameObject hitParticles = Instantiate(buildingHit, transform.position, Quaternion.identity);
        Destroy(hitParticles, 2f);

        if (health <= 0)
        {
            ObjectPoolManager.RetrunObjectToPool(gameObject);
        }
    }
}
