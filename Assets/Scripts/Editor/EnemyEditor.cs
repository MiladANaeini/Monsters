using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EnemyEditor : EditorWindow
{
    string enemyName = "Monster";
    float moveSpeed = 5f;
    int maxHealth = 5;
    int damageAmount = 1;

    Sprite originalSprite;
    Sprite hitSprite;

    GameObject enemyBloodPrefab;
    GameObject enemyXpPrefab;

    enum MovementType { FreeRoam, ChaseInRange, SinMovement}
    MovementType selectedMovement = MovementType.FreeRoam;

    bool showChaseInRangeOptions = false;
    float chaseInRangeDistance = 5f;

    [MenuItem("Window/Enemy Editor")]
    public static void ShowWindow()
    {
        GetWindow<EnemyEditor>("Enemy Editor");
    }
     void OnGUI()
    {
        GUILayout.Label("Create Enemy", EditorStyles.boldLabel);

        enemyName = EditorGUILayout.TextField("Name",enemyName);
        maxHealth = EditorGUILayout.IntField("Max Health", maxHealth);
        damageAmount = EditorGUILayout.IntField("Damage Amount", damageAmount);
        moveSpeed = EditorGUILayout.FloatField("Move Speed", moveSpeed);

        GUILayout.Label("Original Sprite", EditorStyles.label);
        originalSprite = (Sprite)EditorGUILayout.ObjectField(originalSprite, typeof(Sprite), false);
        GUILayout.Label("Hit Sprite", EditorStyles.label);
        hitSprite = (Sprite)EditorGUILayout.ObjectField(hitSprite, typeof(Sprite), false);

        enemyBloodPrefab = (GameObject)EditorGUILayout.ObjectField("Enemy Blood Prefab", enemyBloodPrefab, typeof(GameObject), false);
        enemyXpPrefab = (GameObject)EditorGUILayout.ObjectField("Enemy XP Prefab", enemyXpPrefab, typeof(GameObject), false);

        selectedMovement = (MovementType)EditorGUILayout.EnumPopup("Movement Type", selectedMovement);

        if (selectedMovement == MovementType.ChaseInRange)
        {
            showChaseInRangeOptions = true;
            chaseInRangeDistance = EditorGUILayout.FloatField("Chase In Range Distance", chaseInRangeDistance);
        }
        else
        {
            showChaseInRangeOptions = false;
        }

        if (GUILayout.Button("Create"))
        {
            CreateEnemy();
        }
    }
    void CreateEnemy()
    {
        // Create a new GameObject for the enemy
        GameObject enemy = new(enemyName);
        enemy.AddComponent<Enemy>();

        // Assign the components and properties to the enemy
        Enemy enemyScript = enemy.GetComponent<Enemy>();

        SpriteRenderer spriteRenderer = enemy.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = originalSprite;
        BoxCollider2D boxCollider = enemy.AddComponent<BoxCollider2D>();

        enemy.layer = LayerMask.NameToLayer("Enemy");
        enemy.tag = "Enemy";

        enemyScript.moveSpeed = moveSpeed;
        enemyScript.maxHealth = maxHealth;
        enemyScript.damageAmount = damageAmount;
        enemyScript.enemyBlood = enemyBloodPrefab;
        enemyScript.enemyXp = enemyXpPrefab;

        // Assign movement type based on the selected dropdown value
        if (selectedMovement == MovementType.FreeRoam)
        {
            enemyScript.chaseFreeRoam = true;
        }
        else if (selectedMovement == MovementType.ChaseInRange)
        {
            enemyScript.chaseFreeRoam = false;
            enemyScript.chaseInRange = chaseInRangeDistance;
        }
        else if (selectedMovement == MovementType.SinMovement)
        {
            enemyScript.useSinMovement = true;
        }

        // Assign sprites for UpdateSpriteOnHit component
        UpdateSpriteOnHit updateSpriteOnHit = enemy.AddComponent<UpdateSpriteOnHit>();
        updateSpriteOnHit.originalSprite = originalSprite;
        updateSpriteOnHit.hitSprite = hitSprite;

        // Position the enemy at the origin (you can customize this)
        enemy.transform.position = Vector3.zero;

        Debug.Log("Enemy created: " + enemyName);
    }
}
