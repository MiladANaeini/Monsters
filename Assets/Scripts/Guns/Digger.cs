using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiggerZone : MonoBehaviour
{
    public int damage = 3;
    private Health targetHealth;
    private Coroutine attackCoroutine;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer("Building"))
        {
            targetHealth = collider.GetComponent<Health>();

            if (targetHealth != null)
            {
                Debug.Log("Building entered: " + collider.name);  

                if (attackCoroutine == null)
                {
                    attackCoroutine = StartCoroutine(AttackSequence());
                }
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
                Debug.Log("Building exited: " + collider.name);  
            }
        }
    }

    private IEnumerator AttackSequence()
    {
        while (targetHealth != null && targetHealth.health > 0)
        {
            Debug.Log("Damaging building: " + targetHealth.name + " Health: " + targetHealth.health);
            targetHealth.TakeDamage(damage);  
            yield return new WaitForSeconds(1f); 
        }

        attackCoroutine = null;
    }
}
