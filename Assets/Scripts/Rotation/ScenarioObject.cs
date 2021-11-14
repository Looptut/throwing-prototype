using UnityEngine;
using System.Collections.Generic;
/// <summary>
/// Сериализованный объект сценария
/// </summary>
[CreateAssetMenu(fileName = "Scenario", menuName = "ScriptableObjects/ScenarioScriptableObject", order = 52)]
public class ScenarioObject : ScriptableObject
{
    [SerializeField]
    private List<Scenario> scenario;
    public List<Scenario> ScenarioSO => scenario;
}
