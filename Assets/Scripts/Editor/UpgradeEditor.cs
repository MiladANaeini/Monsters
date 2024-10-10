using UnityEngine;
using UnityEditor;

public class UpgradeEditor : EditorWindow
{
    private GameObject gunPrefab;
    string gunName = "New Gun";

    private Sprite gunSprite;
    private Vector2 colliderOffset;
    private float colliderRadius = 0.5f;
    private Vector3 gunPosition = Vector3.zero;
    private Vector3 gunRotation = Vector3.zero;
    private Vector3 gunScale = Vector3.one;

    string projectileName = "New Projectile";
    private Sprite projectileSprite;
    private int projectileDamage = 1;

    // For the Shooting Point
    private Vector3 shootingPointPosition = Vector3.zero;
    private Vector3 shootingPointRotation = Vector3.zero;
    private Vector3 shootingPointScale = Vector3.one;

    public float projectileSpeed = 10f; 
    public float shootingInterval = 0.5f; 

    [MenuItem("Window/Upgrade Editor")]
    public static void ShowWindow()
    {
        GetWindow<UpgradeEditor>("Upgrade Editor");
    }

    void OnGUI()
    {
        GUILayout.Label("Create Gun Prefab", EditorStyles.boldLabel);
        gunName = EditorGUILayout.TextField("Name", gunName);
        gunSprite = (Sprite)EditorGUILayout.ObjectField("Gun Sprite", gunSprite, typeof(Sprite), false);
        GUILayout.Label("Transform", EditorStyles.boldLabel);
        gunPosition = EditorGUILayout.Vector3Field("Position", gunPosition);
        gunRotation = EditorGUILayout.Vector3Field("Rotation", gunRotation);
        gunScale = EditorGUILayout.Vector3Field("Scale", gunScale);

        colliderOffset = EditorGUILayout.Vector2Field("Collider Offset", colliderOffset);
        colliderRadius = EditorGUILayout.FloatField("Collider Radius", colliderRadius);

        // Shooting Point Transform Fields
        GUILayout.Label("Shooting Point Transform", EditorStyles.boldLabel);
        shootingPointPosition = EditorGUILayout.Vector3Field("Position", shootingPointPosition);
        shootingPointRotation = EditorGUILayout.Vector3Field("Rotation", shootingPointRotation);
        shootingPointScale = EditorGUILayout.Vector3Field("Scale", shootingPointScale);

        // Bullet Prefab Creation
        GUILayout.Space(20);
        GUILayout.Label("Create Projectile Prefab", EditorStyles.boldLabel);
        projectileName = EditorGUILayout.TextField("Projectile Name", projectileName);
        projectileSprite = (Sprite)EditorGUILayout.ObjectField("Projectile Sprite", projectileSprite, typeof(Sprite), false);
        projectileDamage = EditorGUILayout.IntField("Projectile Damage", projectileDamage);

        // Add fields for projectile speed and shooting interval
        projectileSpeed = EditorGUILayout.FloatField("Projectile Speed", projectileSpeed);
        shootingInterval = EditorGUILayout.FloatField("Shooting Interval", shootingInterval);

        if (GUILayout.Button("Create Gun Prefab"))
        {
            GameObject projectilePrefab = CreateProjectilePrefab();
            CreateGunPrefab(projectilePrefab);
        }
    }

    void CreateGunPrefab(GameObject projectilePrefab)
    {
        // Create new GameObject
        GameObject gunObject = new GameObject(gunName);

        gunObject.transform.position = gunPosition;
        gunObject.transform.eulerAngles = gunRotation;
        gunObject.transform.localScale = gunScale;

        // Add to the Gun layer
        gunObject.layer = LayerMask.NameToLayer("Gun");

        // Add a SpriteRenderer and assign the sprite
        SpriteRenderer spriteRenderer = gunObject.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = gunSprite;

        // Add the GunController script
        GunController gunController = gunObject.AddComponent<GunController>();

        //string projectilePath = "Assets/Art/Projectile/" + projectilePrefab.name + ".prefab";
        //PrefabUtility.SaveAsPrefabAsset(projectilePrefab, projectilePath);
        //GameObject savedProjectilePrefab = AssetDatabase.LoadAssetAtPath<GameObject>(projectilePath);

        // Assign the loaded prefab to gunController
        gunController.projectilePrefab = projectilePrefab; // Directly use the prefab passed from CreateProjectilePrefab
        gunController.projectileSpeed = projectileSpeed;
        gunController.shootingInterval = shootingInterval;

        // Add CircleCollider2D and set it to trigger
        CircleCollider2D collider = gunObject.AddComponent<CircleCollider2D>();
        collider.isTrigger = true;
        collider.offset = colliderOffset;
        collider.radius = colliderRadius;

        // Create a child GameObject called "Shooting Point"
        GameObject shootingPoint = new GameObject("ShootingPoint");
        shootingPoint.layer = LayerMask.NameToLayer("Gun");
        shootingPoint.transform.parent = gunObject.transform;
        shootingPoint.transform.localPosition = shootingPointPosition;
        shootingPoint.transform.localEulerAngles = shootingPointRotation;
        shootingPoint.transform.localScale = shootingPointScale;

        gunController.shootPoint = shootingPoint.transform;

        // Create the prefab in the project folder
        string localPath = "Assets/Art/" + gunObject.name + ".prefab";
        localPath = AssetDatabase.GenerateUniqueAssetPath(localPath);
        PrefabUtility.SaveAsPrefabAsset(gunObject, localPath);

        GameObject gunInScene = (GameObject)PrefabUtility.InstantiatePrefab(gunObject);

        Debug.Log("Gun prefab created successfully at " + localPath);
    }
    private GameObject CreateProjectilePrefab()
    {
        // Create new GameObject for the bullet
        GameObject projectileObject = new GameObject(projectileName);

        projectileObject.layer = LayerMask.NameToLayer("Projectile");


        // Add a SpriteRenderer and assign the sprite
        SpriteRenderer spriteRenderer = projectileObject.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = projectileSprite;

        // Add Rigidbody2D with settings
        Rigidbody2D rb = projectileObject.AddComponent<Rigidbody2D>();
        rb.simulated = true; 
        rb.collisionDetectionMode = CollisionDetectionMode2D.Discrete;

        // Add BoxCollider2D and set size
        BoxCollider2D boxCollider = projectileObject.AddComponent<BoxCollider2D>();
        boxCollider.size = new Vector2(0.05f, 0.05f);

        // Add the Bullet projectile component (assuming it's defined elsewhere)
        BulletProjectile bulletProjectile = projectileObject.AddComponent<BulletProjectile>();
        bulletProjectile.damage = projectileDamage; // Assign damage value

        // Create the prefab in the project folder
        string localPath = "Assets/Art/Projectile/" + projectileObject.name + ".prefab";
        localPath = AssetDatabase.GenerateUniqueAssetPath(localPath);
        PrefabUtility.SaveAsPrefabAsset(projectileObject, localPath);

        // Clean up the scene by deleting the temporary GameObject
        DestroyImmediate(projectileObject);

        Debug.Log("projectile prefab created successfully at " + localPath);

        GameObject savedProjectilePrefab = AssetDatabase.LoadAssetAtPath<GameObject>(localPath);


        return AssetDatabase.LoadAssetAtPath<GameObject>(localPath); ; // Return the created projectile prefab
    }
}
