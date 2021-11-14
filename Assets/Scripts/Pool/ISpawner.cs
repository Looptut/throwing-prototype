using UnityEngine;
/// <summary>
/// Интерфейс создателя объектов
/// </summary>
public interface ISpawner
{
    GameObject Create(GameObject prefab);
}
