using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolSystem : MonoBehaviour
{
    public static List<PooledObjectInfo> ObjectPools = new List<PooledObjectInfo>();
    public static GameObject SpawnObject(GameObject objectToSpawn, Vector3 spawnPosition, Quaternion spawnRotation)
    {
        PooledObjectInfo pool = ObjectPools.Find(item => item.LookupString == objectToSpawn.name);

        if (pool == null)
        {
            pool = new PooledObjectInfo() { LookupString = objectToSpawn.name };
            ObjectPools.Add(pool);
        }

        GameObject spawnableObj = null;
        foreach (GameObject obj in pool.InactiveObjects)
        {
            if (obj != null)
            {
                spawnableObj = obj;
                break;
            }
        }
        if (spawnableObj == null)
        {
            spawnableObj = Instantiate(objectToSpawn, spawnPosition, spawnRotation);
        }
        else
        {
            spawnableObj.transform.position = spawnPosition;
            spawnableObj.transform.rotation = spawnRotation;
            pool.InactiveObjects.Remove(spawnableObj);
            spawnableObj.SetActive(true);
        }
        return spawnableObj;

    }
    public static void ReturnObjectToPool(GameObject objToDeactivate)
    {
        string goName = objToDeactivate.name.Substring(0, objToDeactivate.name.Length - 7);
        PooledObjectInfo pool = ObjectPools.Find(item => item.LookupString == goName);
        if (pool == null)
        {
            Debug.LogWarning("Trying to release an object that is not pooled:" + objToDeactivate.name);
        }
        else
        {
            objToDeactivate.SetActive(false);
            pool.InactiveObjects.Add(objToDeactivate);
        }
    }

}

