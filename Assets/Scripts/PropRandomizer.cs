using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropRandomizer : MonoBehaviour
{
    public List<GameObject> propSpawnPoints;
    public List<GameObject> propPrefabs;
    void Start()
    {
        SpawnProps();
    }

    void SpawnProps()
    {
        foreach (GameObject sp in propSpawnPoints)
        {
            int rand = Random.Range(0, propPrefabs.Count);
            GameObject prop = ObjectPoolManager.SpawnObject(propPrefabs[rand], sp.transform.position, sp.transform.rotation);

            // make random object a child of the PropLocation
            prop.transform.parent = sp.transform;
        }
    }
}
