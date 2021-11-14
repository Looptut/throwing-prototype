
using System;
/// <summary>
/// Базовое наполнение сценария
/// </summary>
[Serializable]
public struct Scenario
{
    public float time;
    public float acelerationTime;
    public bool isClockwise;
    /// <summary>
    /// Скорость в degree/s
    /// </summary>
    public float speed;
}
