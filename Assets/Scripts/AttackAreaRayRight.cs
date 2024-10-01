using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAreaRayRight : MonoBehaviour
{
    public int damage = 3;
    private void OnTriggerEnter2D(Collider2D collider)
    {
        Health health = collider.GetComponent<Health>();

        if (health != null)
        {
            health.TakeDamage(damage); 
        }
    }

}
