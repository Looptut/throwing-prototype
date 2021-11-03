using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;
/// <summary>
/// Рассчитывает, на сколько вращать объект
/// </summary>
public class Rotator : MonoBehaviour
{
    public Vector2 CircleVector => _circleVector;
    private Vector2 _circleVector = new Vector2(0, 1);

    public event Action<float> OnChangeDegree = delegate { };

    [SerializeField] private ScenarioObject scenarioSO;
    [SerializeField] private bool beginOnStart = false;
    [SerializeField] private float timeStep = 0.1f;

    private Coroutine coroutineTimer;
    private List<Scenario> scenario;
    private CoroutineHandle scenarioCoroutine;

    [SerializeField] private bool doScenario = true;

    /// TODO: парсить из xml?
    public void Init(ScenarioObject scenarioSO)
    {
        scenario = new List<Scenario>();
        foreach (Scenario scenarioStep in scenarioSO.ScenarioSO)
        {
            scenario.Add(scenarioStep);
        }

        scenarioCoroutine = this.RunCoroutine(PerformScenario(scenario));
    }

    private void Start()
    {
        if (beginOnStart)
        {
            scenarioCoroutine = this.RunCoroutine(PerformScenario(scenarioSO));
        }
    }
    private IEnumerator PerformScenario(List<Scenario> scenario)
    {
        while (doScenario)
        {
            foreach (Scenario scenarioStep in scenario)
            {
                yield return DoScenarioStep(scenarioStep);
            }
        }
    }

    private IEnumerator PerformScenario(ScenarioObject scenario)
    {
        while (doScenario)
        {
            foreach (Scenario scenarioStep in scenario.ScenarioSO)
            {
                yield return DoScenarioStep(scenarioStep);
            }
        }
    }

    private IEnumerator DoScenarioStep(Scenario scenarioStep)
    {
        float time = 0;
        float speed = scenarioStep.acelerationTime == 0 ? scenarioStep.speed : 0;
        float degree = 0;
        float aceleration = scenarioStep.acelerationTime == 0 ? 0 : scenarioStep.speed / scenarioStep.acelerationTime;
        int direction = (scenarioStep.isClockwise ? 1 : -1);
        while (time < scenarioStep.time)
        {
            yield return new WaitForSeconds(timeStep);
            time += timeStep;
            degree = ChangeDegree(scenarioStep, time, ref speed, aceleration, direction);
            OnChangeDegree.Invoke(degree);
        }
        yield return null;
    }

    private float ChangeDegree(Scenario scenarioStep, float time, ref float speed, float aceleration, int direction)
    {
        if (scenarioStep.acelerationTime != 0)
        {
            if (time < scenarioStep.acelerationTime)
            {
                speed += direction * (aceleration * timeStep);
            }
            else if (time > scenarioStep.time - scenarioStep.acelerationTime)
            {
                speed -= direction * (aceleration * timeStep);
            }
        }
        return direction * speed * timeStep;
    }
}