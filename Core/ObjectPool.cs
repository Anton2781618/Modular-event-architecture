using UnityEngine;
using System.Collections.Generic;

public class ObjectPool : MonoBehaviour
{
    private Dictionary<string, Queue<GameObject>> pools = new Dictionary<string, Queue<GameObject>>();

    public GameObject GetObject(string prefabName, Vector3 position, Quaternion rotation)
    {
        if (!pools.ContainsKey(prefabName))
        {
            pools[prefabName] = new Queue<GameObject>();
        }

        GameObject obj;
        if (pools[prefabName].Count > 0)
        {
            obj = pools[prefabName].Dequeue();
            obj.transform.position = position;
            obj.transform.rotation = rotation;
        }
        else
        {
            obj = CreateNewObject(prefabName, position, rotation);
        }

        obj.SetActive(true);
        return obj;
    }

    public void ReturnObject(GameObject obj, string prefabName)
    {
        obj.SetActive(false);
        pools[prefabName].Enqueue(obj);
    }

    private GameObject CreateNewObject(string prefabName, Vector3 position, Quaternion rotation)
    {
        GameObject prefab = Resources.Load<GameObject>(prefabName);
        return Instantiate(prefab, position, rotation);
    }
}
