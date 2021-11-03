using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] protected PoolPrefs prefs;
    private Queue<GameObject> pool;
    private GameObject concreteObject;
    private GameObject parentPool;
    private ISpawner spawner;

    private void Start()
    {
        spawner = new Spawner();
        CreatePool();
    }

    private void CreatePool()
    {
        pool = new Queue<GameObject>();
        parentPool = new GameObject(prefs.name);
        parentPool.transform.SetParent(transform, true);
        AddObjectsToPool(parentPool.transform);
    }

    private void AddObjectsToPool(Transform parent)
    {
        for (int i = 0; i < prefs.objectsCount; i++)
        {
            concreteObject = spawner.Create(prefs.prefab);
            concreteObject.transform.position = parent.position;
            concreteObject.transform.SetParent(parent, true);
            concreteObject.SetActive(false);
            pool.Enqueue(concreteObject);
        }
    }
    /// <summary>
    /// Вернет объект из пула
    /// </summary>
    /// <returns></returns>
    public GameObject GetNextFromPool()
    {
        GameObject nextObject = pool.Dequeue();
        nextObject.gameObject.SetActive(true);
        pool.Enqueue(nextObject);
        return nextObject;
    }
}
