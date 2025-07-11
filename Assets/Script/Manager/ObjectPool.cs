using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : Singleton<ObjectPool>
{
    private Dictionary<string, Queue<GameObject>> poolDictionary = new Dictionary<string, Queue<GameObject>>();
    private Dictionary<string, GameObject> prefabDictionary = new Dictionary<string, GameObject>();


    public void RegisterPrefab(string key, GameObject prefab)
    {
        if (!prefabDictionary.ContainsKey(key))
        {
            prefabDictionary[key] = prefab;
            poolDictionary[key] = new Queue<GameObject>();
        }
    }


    public T GetObject<T>(string key, Transform parent = null) where T : Component
    {
        GameObject obj;

        if (poolDictionary.ContainsKey(key) && poolDictionary[key].Count > 0)
        {
            obj = poolDictionary[key].Dequeue();
            obj.SetActive(true);
            Debug.Log($"Reusing object from pool with key: {key}"+"//"+ poolDictionary[key].Count);
        }
        else
        {
            Debug.Log($"Reusing object from pool with key: {key}" + "//" +"null");


            obj = Instantiate(prefabDictionary[key]);
        }

        if (parent != null)
        {
            obj.transform.SetParent(parent);
        }
        else
        {
            Debug.LogWarning($"No parent specified for object with key");
        }
            Entity entity = obj.GetComponent<Entity>();
        if (entity != null)
            entity.ResetStatEntity();
        return obj.GetComponent<T>();
    }

    public void ReturnObject(string key, GameObject obj)
    {
        if (!poolDictionary.ContainsKey(key))
        {
            poolDictionary[key] = new Queue<GameObject>();
        }

        obj.SetActive(false);
        obj.transform.SetParent(transform); 

        poolDictionary[key].Enqueue(obj);
    }

    public void PreloadObjects(string key, int count, Transform parent = null)
    {
        if (!prefabDictionary.ContainsKey(key))
        {
            Debug.LogError($"No prefab registered with key: {key}");
            return;
        }

        for (int i = 0; i < count; i++)
        {
            GameObject obj = Instantiate(prefabDictionary[key]);
            obj.SetActive(false);
            
            if (parent != null)
                obj.transform.SetParent(parent);
            else
                obj.transform.SetParent(transform);
                
            poolDictionary[key].Enqueue(obj);
        }
    }


    public void ClearPool(string key)
    {
        if (!poolDictionary.ContainsKey(key))
            return;

        while (poolDictionary[key].Count > 0)
        {
            GameObject obj = poolDictionary[key].Dequeue();
            Destroy(obj);
        }
    }
}
