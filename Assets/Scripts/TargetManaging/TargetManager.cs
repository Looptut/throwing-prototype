using System;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Класс цели с обработкой попадания
/// </summary>
public class TargetManager : MonoBehaviour
{
    /// <summary>
    /// Событие результата попадания
    /// </summary>
    public event Action<bool> OnHitResult;

    [SerializeField] private Rotator rotator;
    [SerializeField] private Throarer throarer;

    private float frontDegree = 0f;
    private float radius = 1.5f;
    private List<bool> degreeOccupancyList = new List<bool>();

    private Throable throable;

    private void Awake()
    {
        for (int i = 0; i < 360; i++)
        {
            degreeOccupancyList.Add(false);
        }

        radius = GetComponentInChildren<SpriteRenderer>().bounds.extents.x;
    }

    private void Start()
    {
        if (rotator == null)
        {
            if (!(rotator = GetComponent<Rotator>()) || !(rotator = FindObjectOfType<Rotator>()))
            {
                Debug.LogError("There's no rotator found");
                Destroy(this);
                return;
            }
        }
        rotator.OnChangeDegree += ChangeCurrentFacedDegree;

        if (throarer == null)
        {
            if ((throarer = rotator.GetComponent<Throarer>()) == null)
            {
                Debug.LogError("There's no IThroarer found");
                Destroy(this);
                return;
            }
        }
        throarer.EndPosition = (Vector2)transform.position - Vector2.up * (radius + 0.05f);

        throarer.OnFire += SaveThroableObject;
        throarer.OnHit += CheckPositionAndOccupancy;
    }
    private void SaveThroableObject(Throable obj)
    {
        throable = obj;
    }

    private void CheckPositionAndOccupancy()
    {
        var width = throable.Width;

        var alphaStep = (int)Mathf.Floor(Mathf.Rad2Deg * Mathf.Atan2(width / 2, radius));
        int index;
        for (int i = (int)frontDegree - alphaStep - 1; i < (int)frontDegree - 1 + alphaStep; i++)
        {
            index = (i < 0 ? 360 + i : i) % 360;

            var isOccupied = degreeOccupancyList[index];
            if (!isOccupied)
            {
                degreeOccupancyList[index] = !isOccupied;
            }
            else
            {
                OnHitResult.Invoke(false);
                return;
            }

        }
        OnHitResult.Invoke(true);
    }

    private void ChangeCurrentFacedDegree(float degree)
    {
        frontDegree = (frontDegree + degree) % 360;
        if (frontDegree < 0)
        {
            frontDegree += 360f;
        }
    }

    private void OnDestroy()
    {
        if (rotator != null)
        {
            rotator.OnChangeDegree -= ChangeCurrentFacedDegree;
        }
        if (throarer != null)
        {
            throarer.OnFire -= SaveThroableObject;
            throarer.OnHit -= CheckPositionAndOccupancy;
        }
    }
}