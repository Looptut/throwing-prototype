using UnityEngine;
/// <summary>
/// Базовый спавнер 
/// </summary>
public class Spawner : ISpawner
{
    private GameObject instanceObject;
    /// <summary>
    /// Создать объект без параметров
    /// </summary>
    /// <param name="prefab"></param>
    /// <returns></returns>
    public GameObject Create(GameObject prefab)
    {
        instanceObject = MonoBehaviour.Instantiate(prefab);
        return instanceObject;
    }
}
