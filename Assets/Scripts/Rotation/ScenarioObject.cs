using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Scenario", menuName = "ScriptableObjects/ScenarioScriptableObject", order = 52)]
public class ScenarioObject : ScriptableObject
{
    [SerializeField]
    private List<Scenario> scenario;
    public List<Scenario> ScenarioSO => scenario;


}
