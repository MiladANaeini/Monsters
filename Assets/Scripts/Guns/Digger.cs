using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiggerZone : MonoBehaviour
{
    public int damage = 3;
    private Coroutine attackCoroutine;
    public event Action<int> OnBuildingDestroyed;
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer("Building"))
        {
            Health health = collider.GetComponent<Health>();

            if (health != null)
            {
                if (attackCoroutine == null)
                {
                    attackCoroutine = StartCoroutine(AttackSequence(health));
                }
            }
            if (health.health <= 0)
            {
                OnBuildingDestroyed?.Invoke(10);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer("Building"))
        {
            if (attackCoroutine != null)
            {
                StopCoroutine(attackCoroutine);
                attackCoroutine = null; 
            }
        }
    }

    private IEnumerator AttackSequence(Health health)
    {
        while (health != null && health.health > 0)
        {
            health.TakeDamage(damage);
            yield return new WaitForSeconds(1f);
        }
   

        attackCoroutine = null;
    }
}
