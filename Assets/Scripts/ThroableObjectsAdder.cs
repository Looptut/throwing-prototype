using UnityEngine;
/// <summary>
/// Пример добавления объектов в круг цели
/// </summary>
public class ThroableObjectsAdder : MonoBehaviour
{
    [SerializeField] private Throarer throarer;
    [SerializeField] private TargetManager target;

    private Throable throableObject;
    private void Start()
    {
        if (throarer != null)
            throarer.OnFire += SaveObject;
        if (target != null)
            target.OnHitResult += ChooseActionOnResult;
    }

    private void ChooseActionOnResult(bool isSuccess)
    {
        if (isSuccess)
        {
            throableObject.transform.SetParent(transform, true);
        }
        else
        {
            if (throableObject != null)
                throableObject.gameObject.SetActive(false);
        }
    }

    private void SaveObject(Throable throableObject)
    {
        this.throableObject = throableObject;
    }

    private void OnDestroy()
    {
        if (throarer != null)
            throarer.OnFire -= SaveObject;
        if (target != null)
            target.OnHitResult -= ChooseActionOnResult;
    }
}
