using System.Collections.Generic;
using UnityEngine;

//Inspired from Brackey's tutorial on the Object Pooling System

[DisallowMultipleComponent]
public class ObjectPooler : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public ProjectilePrefabName tag;
        public GameObject prefab;
        public int size;
    }

    public Transform parent;
    public List<Pool> pools;
    public Dictionary<ProjectilePrefabName, Queue<GameObject>> poolDictionary;

    public static ObjectPooler Instance;

    void Awake()
    {
        #region Singleton
        if (!Instance) Instance = this;
        #endregion

        InstantiateAllPools();
    }

    public GameObject SpawnFromPool(ProjectilePrefabName tag, ProjectileController proj, Vector3 position)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("Pool with tag " + tag + " doesn't exist.", this);
            return null;
        }
        if (poolDictionary[tag].Count == 0)
        {
            Debug.LogWarning("Pool with tag " + tag + " is empty.",this);
            return null;
        }

        GameObject objectToSpawn = poolDictionary[tag].Dequeue();
        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;

        if (proj.TryGetComponent(out BulletController bullet))
        {
            ImplementPooledObject(objectToSpawn, bullet);
        }
        else if (proj.TryGetComponent(out SpawnerController spawner))
        {
            ImplementPooledObject(objectToSpawn, spawner);
        } else
        {
            Debug.LogWarning("The pooled object doesn't have any components of type BulletController or SpawnerController.",objectToSpawn);
            return null;
        }

        poolDictionary[tag].Enqueue(objectToSpawn);
        return objectToSpawn;
    }

    void ImplementPooledObject<T>(GameObject objectToSpawn, T proj) where T : MonoBehaviour
    {
        IPooledObject<T> pooledObj = objectToSpawn.GetComponent<IPooledObject<T>>();
        if (pooledObj == null) return;
        pooledObj.OnObjectSpawn(proj);
    }

    public void DespawnFromPool(ProjectilePrefabName tag)
    {
        if (!poolDictionary.ContainsKey(tag)){
            Debug.LogWarning("Pool with tag " + tag + " doesn't exist.", this);
            return;
        }

        GameObject objectToSpawn = poolDictionary[tag].Dequeue();
        objectToSpawn.SetActive(false);
        poolDictionary[tag].Enqueue(objectToSpawn);
    }

    public void DespawnAllFromPool(ProjectilePrefabName tag){

        if (!poolDictionary.ContainsKey(tag)) { 
            Debug.LogWarning("Pool with tag " + tag + " doesn't exist.", this); 
            return;
        }

        for (int i = 0; i < poolDictionary[tag].Count; i++)
            DespawnFromPool(tag);
    }

    void InstantiateAllPools()
    {
        poolDictionary = new Dictionary<ProjectilePrefabName, Queue<GameObject>>();
        foreach (Pool pool in pools)
        {
            GameObject poolParent = new ("Pool_" + pool.tag);
            poolParent.transform.SetParent(parent);
            Queue<GameObject> objectPool = new Queue<GameObject>();
            if (pool.prefab != null)
                for (int i = 0; i < pool.size; i++)
                {
                    GameObject obj = Instantiate(pool.prefab);
                    obj.SetActive(false);
                    obj.name = pool.tag.ToString();
                    obj.transform.SetParent(poolParent.transform);
                    objectPool.Enqueue(obj);
                }
            else
                Debug.LogWarning("The pool with tag " + pool.tag + "doesn't have a prefab reference.", this);
            poolDictionary.Add(pool.tag, objectPool);
        }
    }

    void InstantiatePools(List<ProjectilePrefabName> projectiles)
    {
        poolDictionary = new Dictionary<ProjectilePrefabName, Queue<GameObject>>();
        foreach (Pool pool in pools)
        {
            if (!projectiles.Contains(pool.tag)) return;
            GameObject poolParent = new("Pool_" + pool.tag);
            poolParent.transform.SetParent(parent);
            Queue<GameObject> objectPool = new Queue<GameObject>();
            if (pool.prefab != null)
                for (int i = 0; i < pool.size; i++)
                {
                    GameObject obj = Instantiate(pool.prefab);
                    obj.SetActive(false);
                    obj.name = pool.tag.ToString();
                    obj.transform.SetParent(poolParent.transform);
                    objectPool.Enqueue(obj);
                }
            else
                Debug.LogWarning("The pool with tag " + pool.tag + "doesn't have a prefab reference.", this);
            poolDictionary.Add(pool.tag, objectPool);
        }
    }

    void DeleteAllInstances()
    {
        if (poolDictionary == null) return;
        poolDictionary.Clear();
        foreach(Transform child in parent)
            Destroy(child.gameObject);
    }
}

