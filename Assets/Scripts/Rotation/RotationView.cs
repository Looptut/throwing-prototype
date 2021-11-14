using UnityEngine;
/// <summary>
/// View поворота 
/// </summary>
public class RotationView : MonoBehaviour
{
    [SerializeField] private Rotator rotator;

    private void Start()
    {
        rotator.OnChangeDegree += ChangeDegree;
    }

    private void ChangeDegree(float degree)
    {
        transform.Rotate(Vector3.forward * degree);
    }

    private void OnDestroy()
    {
        rotator.OnChangeDegree -= ChangeDegree;
    }
}