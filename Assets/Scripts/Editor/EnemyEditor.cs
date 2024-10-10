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
    GameObject selectedXP;
    string[] xpPrefabNames;
    GameObject[] xpPrefabs;
    int selectedXpIndex = 0;

    float minimumSpawnTime = 1f; 
    float maximumSpawnTime = 3f; 

    enum MovementType { FreeRoam, ChaseInRange, SinMovement}
    MovementType selectedMovement = MovementType.FreeRoam;

    bool showChaseInRangeOptions = false;
    float chaseInRangeDistance = 5f;

    [MenuItem("Window/Enemy Editor")]
    public static void ShowWindow()
    {
        GetWindow<EnemyEditor>("Enemy Editor");
    }

    void OnEnable()
    {
        if (enemyBloodPrefab == null)
        {
            enemyBloodPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Scripts/Enemy/Blood/EnemyBlood.prefab");
        }
        // Load XP prefabs dynamically from the folder
        string xpFolderPath = "Assets/Scripts/Enemy/XP";
        string[] guids = AssetDatabase.FindAssets("t:Prefab", new[] { xpFolderPath });

        xpPrefabNames = new string[guids.Length];
        xpPrefabs = new GameObject[guids.Length];

        for (int i = 0; i < guids.Length; i++)
        {
            string path = AssetDatabase.GUIDToAssetPath(guids[i]);
            xpPrefabs[i] = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            xpPrefabNames[i] = xpPrefabs[i].name;
        }
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

        selectedXpIndex = EditorGUILayout.Popup("Enemy XP Prefab", selectedXpIndex, xpPrefabNames);
        selectedXP = xpPrefabs[selectedXpIndex];

        minimumSpawnTime = EditorGUILayout.FloatField("Minimum Spawn Time", minimumSpawnTime);
        maximumSpawnTime = EditorGUILayout.FloatField("Maximum Spawn Time", maximumSpawnTime);

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
        GameObject enemy = new GameObject(enemyName);
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
        enemyScript.enemyXp = selectedXP;

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

        // Create and save the enemy prefab to the specified path
        string prefabPath = $"Assets/Scripts/Enemy/{enemyName}.prefab";
        PrefabUtility.SaveAsPrefabAsset(enemy, prefabPath);
        Debug.Log("Enemy prefab created at: " + prefabPath);

        // Position the enemy at the origin (you can customize this)
        SetupSpawner(prefabPath);
        Debug.Log("Enemy created: " + enemyName);
    }

    void SetupSpawner(string enemyPrefabPath)
    {
        GameObject enemySpawnerPF = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Scripts/Enemy/EnemySpawnerPF.prefab");
        if (enemySpawnerPF == null)
        {
            Debug.LogError("Please assign the Enemy Spawner PF prefab.");
            return;
        }

        // Instantiate the EnemySpawnerPF in the scene
        GameObject spawnerInstance = Instantiate(enemySpawnerPF, Vector3.zero, Quaternion.identity);

        // Rename the instantiated GameObject to "EnemySpawnerGO"
        spawnerInstance.name = "EnemySpawnerGO";

        // Set the tag and layer for the spawner instance
        spawnerInstance.tag = "Enemy"; // Set the tag to Enemy
        spawnerInstance.layer = LayerMask.NameToLayer("Enemy"); // Set the layer to Enemy

        // Get the EnemySpawner component and assign the created enemy prefab and spawn times
        EnemySpawner enemySpawner = spawnerInstance.GetComponent<EnemySpawner>();
        if (enemySpawner != null)
        {
            enemySpawner.EnemyPreFab = AssetDatabase.LoadAssetAtPath<GameObject>(enemyPrefabPath); // Use the created enemy prefab path
            enemySpawner.minimumSpawnTime = minimumSpawnTime;
            enemySpawner.maximumSpawnTime = maximumSpawnTime;

            Debug.Log("Spawner set up with enemy prefab: " + enemySpawner.EnemyPreFab.name);
        }
        else
        {
            Debug.LogError("The EnemySpawner component is missing on the prefab.");
        }
    }
}
