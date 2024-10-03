using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PropRandomizer : MonoBehaviour
{
    public List<GameObject> propSpawnPoints;
    public List<GameObject> propPrefabs;

    void Start()
    {
        SpawnProps();
    }

    public void SpawnProps()
    {
        foreach (GameObject sp in propSpawnPoints)
        {
            // Clear existing child props at the spawn point
            if (sp.transform.childCount > 0)
            {
                foreach (Transform child in sp.transform)
                {
                    Destroy(child.gameObject); // Remove existing props
                }
            }

            // Spawn a new prop
            int rand = Random.Range(0, propPrefabs.Count);
            GameObject prop = ObjectPoolManager.SpawnObject(propPrefabs[rand], sp.transform.position, sp.transform.rotation);

            // Make the new prop a child of the spawn point to keep the hierarchy clean
            prop.transform.parent = sp.transform;
        }
    }
}
