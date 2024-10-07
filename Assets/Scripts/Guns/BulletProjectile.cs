using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletProjectile : MonoBehaviour
{
    public int damage = 1;

    private void Start()
    {
        Invoke("ReturnToPool", 5f);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {

            Health health = collision.gameObject.GetComponent<Health>();
            if (health != null)
            {
                health.TakeDamage(damage);
            }
            ReturnToPool();
        }
        else
        {
            ReturnToPool();
        }

        }
    private void ReturnToPool()
    {
        Rigidbody rb = GetComponent<Rigidbody>(); 
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
        }
        ObjectPoolManager.RetrunObjectToPool(gameObject);

    }
}
