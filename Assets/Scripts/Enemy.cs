using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Enemy : Health
{
    private GameObject player;
    public float moveSpeed;
    private float distance;
    private UpdateSpriteOnHit updateSpriteOnHit;
    public bool chaseFreeRoam = true;
    public float chaseInRange = 5f;
    public bool useSinMovement = false; 
    public float waveAmplitude = 0.02f;  
    public float waveFrequency = 0.5f;
    public GameObject enemyBlood;
    public GameObject enemyXp;
    public int damageAmount = 1;
        protected override void Start()
    {
        base.Start();

        if (player == null)
        {
           player = FindObjectOfType<PlayerMovement>()?.gameObject;
        }
        updateSpriteOnHit = GetComponent<UpdateSpriteOnHit>();
        onHealthChanged += UpdateSprite;
    }
    public void UpdateSprite()
    {
        updateSpriteOnHit.ChangeSprite();
        if (health <= 0)
        {
            GamesManager.instance.OnEnemyDestroyed();

            GameObject blood = Instantiate(enemyBlood, transform.position, Quaternion.identity);

            Destroy(blood, 2f);

            ObjectPoolManager.RetrunObjectToPool(gameObject);
            Instantiate(enemyXp, transform.position, Quaternion.identity);

        }
    }

   public void UpdateEnemy()
    {
        if (Time.timeScale > 0 && player != null)
        {
        distance = Vector2.Distance(transform.position, player.transform.position);
        Vector2 direction = (player.transform.position - transform.position).normalized;

        if (chaseFreeRoam)
        {
            transform.position = Vector2.MoveTowards(this.transform.position, player.transform.position, moveSpeed * Time.unscaledDeltaTime);

        } else if (!chaseFreeRoam && !useSinMovement && distance < chaseInRange)
        {
            transform.position = Vector2.MoveTowards(this.transform.position, player.transform.position, moveSpeed * Time.unscaledDeltaTime);

        } else if (useSinMovement)
        {
            MoveInSinPattern(direction);
        }
            EnemyAnimation();
        }

        Debug.Log($"MoveSpeed: {moveSpeed}, DeltaTime: {Time.deltaTime}, TimeScale: {Time.timeScale}");

    }
    private void EnemyAnimation()
    {
        float scaleFactor = Mathf.Lerp(1f, 1.1f, Mathf.PingPong(Time.deltaTime * 2, 1));
        transform.localScale = new Vector3(scaleFactor, scaleFactor, transform.localScale.z);
    }
    void MoveInSinPattern(Vector2 direction)
    {
        Vector2 baseMovement = direction * moveSpeed * Time.deltaTime;

       
        Vector2 perpendicular = new Vector2(-direction.y, direction.x);

        float waveOffset = Mathf.Sin(Time.unscaledTime * waveFrequency) * waveAmplitude;

        Vector2 waveMovement = perpendicular * waveOffset;

       
        Vector2 finalPosition = (Vector2)transform.position + baseMovement + waveMovement;

        transform.position = finalPosition;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerMovement playerMovement = collision.gameObject.GetComponent<PlayerMovement>();
        if (playerMovement != null)
        {
            playerMovement.TakeDamage(damageAmount);
        }
    }
}
