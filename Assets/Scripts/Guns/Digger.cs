using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiggerZone : MonoBehaviour
{
    public int damage = 3;
    private void OnTriggerEnter2D(Collider2D collider)
    {
            Debug.Log("trigger");
        if (collider.gameObject.layer == LayerMask.NameToLayer("Building"))
        {

            Health health = collider.GetComponent<Health>();

        if (health != null)
        {
            health.TakeDamage(damage);
        }
        }
    }
}
