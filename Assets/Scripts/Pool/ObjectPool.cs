using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Базовый пул
/// </summary>
public class ObjectPool : MonoBehaviour
{
    [SerializeField] protected PoolPrefs prefs;
    protected Queue<GameObject> pool;
    private GameObject concreteObject;
    protected GameObject parentPool;
    protected ISpawner spawner;

    private void Awake()
    {
        spawner = new Spawner();
        pool = CreatePool(out parentPool);
        AddObjectsToPool(parentPool.transform, pool);
    }

    protected virtual Queue<GameObject> CreatePool(out GameObject parentPool)
    {
        var pool = new Queue<GameObject>();
        parentPool = new GameObject(prefs.name);
        parentPool.transform.SetParent(transform, true);
        return pool;
    }

    protected virtual void AddObjectsToPool(Transform parent, Queue<GameObject> pool)
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
        if (pool.Count != 0)
        {
            GameObject nextObject = pool.Dequeue();
            nextObject.gameObject.SetActive(true);
            return nextObject;
        }
        else
        {
            return null;
        }
    }
    /// <summary>
    /// Вернуть GameObject в пул
    /// </summary>
    /// <param name="returnedObject"></param>
    public void ReturnToPool(GameObject returnedObject)
    {
        pool.Enqueue(returnedObject);
        returnedObject.SetActive(false);
    }
}