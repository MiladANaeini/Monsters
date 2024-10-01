using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    public static List<PooledObjectInfo> objectPools = new List<PooledObjectInfo>();
    public static GameObject SpawnObject(GameObject objectToSpawn, Vector3 spawnPosition, Quaternion spawnRotation)
    {
        PooledObjectInfo pool = objectPools.Find(p => p.lookupString == objectToSpawn.name);

        // if the pool is empty 
        if (pool == null) {
            pool = new PooledObjectInfo() { lookupString = objectToSpawn.name };
            objectPools.Add(pool);
        }
        // Check if there are any inactive object in the poool
        GameObject spawnableObj = null;
        foreach (GameObject obj in pool.InactiveObjects) {
            if (obj != null)
            {
                spawnableObj = obj;
                break;
            }
        }

        if (spawnableObj == null)
        {
            //if there are no inactive objects we create new one
            spawnableObj = Instantiate(objectToSpawn, spawnPosition, spawnRotation);
        }else
        {
            spawnableObj.transform.position = spawnPosition;
            spawnableObj.transform.rotation = spawnRotation;
            pool.InactiveObjects.Remove(spawnableObj);
            spawnableObj?.SetActive(false);
        }
        return spawnableObj;
  
    }

    public static void RetrunObjectToPool(GameObject obj)
    {
        string goName = obj.name.Substring(0, obj.name.Length - 7); //remove the (Clone) from the name
        PooledObjectInfo pool = objectPools.find(pool => p.lookupString == goName);

        if (pool == null)
        {
            Debug.LogWarning("Trying to release an object that is not pooled: " + obj.name);
        }else
        {
            obj.SetActive(false);
            pool.InactiveObjects.Add(obj);
        }

    }


}

public class PooledObjectInfo
{
    public string lookupString;
    public List<GameObject> InactiveObjects = new List<GameObject> ();
}
