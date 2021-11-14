using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;
/// <summary>
/// Рассчитывает, на сколько вращать объект
/// </summary>
public class Rotator : MonoBehaviour
{
    /// <summary>
    /// Событие смены угла
    /// </summary>
    public event Action<float> OnChangeDegree;
    /// <summary>
    /// Значение, при котором работает вращение
    /// </summary>
    public bool DoPerformScenario = true;

    [SerializeField] private ScenarioObject scenarioSO;
    [SerializeField] private bool beginOnStart = false;
    [SerializeField] protected float timeStep = 0.1f;

    protected List<Scenario> scenario;
    private CoroutineHandle scenarioCoroutine;
    /// TODO: парсить из xml?

    private void Start()
    {
        if (beginOnStart)
        {
            scenarioCoroutine = this.RunCoroutine(PerformScenario(scenarioSO));
        }
    }
    protected virtual IEnumerator PerformScenario(List<Scenario> scenario)
    {
        while (DoPerformScenario)
        {
            foreach (Scenario scenarioStep in scenario)
            {
                yield return DoScenarioStep(scenarioStep);
            }
        }
    }

    protected virtual IEnumerator PerformScenario(ScenarioObject scenario)
    {
        while (DoPerformScenario)
        {
            foreach (Scenario scenarioStep in scenario.ScenarioSO)
            {
                yield return DoScenarioStep(scenarioStep);
            }
        }
        scenarioCoroutine = null;
    }

    private IEnumerator DoScenarioStep(Scenario scenarioStep)
    {
        float time = 0;
        float speed = scenarioStep.acelerationTime == 0 ? scenarioStep.speed : 0;
        float degree = 0;
        float aceleration = scenarioStep.acelerationTime == 0 ? 0 : scenarioStep.speed / scenarioStep.acelerationTime;
        int direction = (scenarioStep.isClockwise ? -1 : 1);
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
                speed += (aceleration * timeStep);
            }
            else if (time > scenarioStep.time - scenarioStep.acelerationTime)
            {
                speed -= (aceleration * timeStep);
            }
        }
        return direction * speed * timeStep;
    }
}