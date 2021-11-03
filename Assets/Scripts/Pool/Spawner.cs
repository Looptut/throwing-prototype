using UnityEngine;

public class Spawner : ISpawner
{
    private GameObject instanceObject;
    public GameObject Create(GameObject prefab)
    {
        instanceObject = MonoBehaviour.Instantiate(prefab);
        return instanceObject;
    }
}
