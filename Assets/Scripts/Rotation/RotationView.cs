using UnityEngine;
using System;
/// <summary>
/// View поворота 
/// </summary>
public class RotationView : MonoBehaviour
{
    [SerializeField] private Rotator rotator;
    private Vector3 rotation = Vector3.zero;

    private void Start()
    {
        rotator.OnChangeDegree += ChangeDegree;
    }
    private float diff;
    private void ChangeDegree(float degree)
    {
        rotation = transform.rotation.eulerAngles;
        transform.Rotate(Vector3.forward * degree);
    }
    private void OnDestroy()
    {
        rotator.OnChangeDegree -= ChangeDegree;
    }
}