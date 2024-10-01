using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    private List<Transform> enemiesInRange = new List<Transform>();
    public GameObject projectilePrefab;
    public float projectileSpeed= 10f;
    public Transform shootPoint;
    private Transform closestEnemy;
    public float timer;


    private void OnTriggerExit2D(Collider2D collider)
    {

        Health health = collider.GetComponent<Health>();

        if (health != null)
        {
            enemiesInRange.Remove(collider.transform);

        }
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {

            Health health = collider.GetComponent<Health>();
        if (health != null)
        {
            enemiesInRange.Add(collider.transform);
        }
        }
    }
    public void UpdateGunController()
    {
        if (enemiesInRange.Count > 0)
        {
            UpdateClosestEnemy();
            if (closestEnemy != null)
            {
                RoatateGunTowardsEnemy();
                timer += Time.deltaTime;
                if (timer > 0.5)
                {
                    timer = 0;
                    ShootProjectile();
                }
            }
        }
    }
    void UpdateClosestEnemy()
    {
        float closestDistance = Mathf.Infinity;
        closestEnemy = null;

        foreach (Transform enemy in enemiesInRange)
        {
            float distanceToEnemy = Vector2.Distance(transform.position, enemy.position);
            if (distanceToEnemy < closestDistance)
            {
                closestDistance = distanceToEnemy;
                closestEnemy = enemy;

            }
        }
    }
    void RoatateGunTowardsEnemy()
    {
        Vector2 direction = closestEnemy.position - transform.position;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 45;

        transform.rotation = Quaternion.Euler(new Vector3(0,0,angle));

     
    }

    void ShootProjectile()
    {

        GameObject projectile = Instantiate(projectilePrefab, shootPoint.position, shootPoint.rotation);
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        rb.velocity = shootPoint.right * projectileSpeed;
    }
}

