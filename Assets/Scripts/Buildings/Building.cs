using System.Collections;
using System.Collections.Generic;
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

            GameObject hit = Instantiate(buildingHit, transform.position, Quaternion.identity);
            Destroy(hit, 2f);
        if (health <= 0)
        {
            Destroy(gameObject);  
        }
    }
}
