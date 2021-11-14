using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
/// <summary>
/// Берет объект из пула и запускает в конечную точку
/// </summary>
public class Throarer : MonoBehaviour
{
    /// <summary>
    /// Событие по достижении цели
    /// </summary>
    public event Action OnHit;
    /// <summary>
    /// Событие на начале выстрела
    /// </summary>
    public event Action<Throable> OnFire;
    /// <summary>
    /// Пул объектов
    /// </summary>
    public ObjectPool pool;
    /// <summary>
    /// Точка запуска и постановки объектов для выстрела
    /// </summary>
    public Transform StartPosition;
    /// <summary>
    /// Конечная точка у цели
    /// </summary>
    public Vector2 EndPosition
    {
        get
        {
            return endPosition;
        }
        set
        {
            if (value.x < Screen.width && value.y < Screen.height)
            {
                endPosition = value;
            }
        }
    }

    [SerializeField] private float speed;
    [Header("Необязательно для заполнения")]
    [SerializeField] private TargetManager target;

    private Vector2 endPosition;

    private InputControls controls;
    private Coroutine coroutine;
    private Throable throable;

    private void Awake()
    {
        controls = new InputControls();
        controls.Player.Shoot.performed += Shoot;

        if (target != null)
            target.OnHitResult += ChooseActionOnResult;
    }

    private void ChooseActionOnResult(bool isSuccess)
    {
        if (!isSuccess)
        {
            if (coroutine == null)
            {
                coroutine = StartCoroutine(WaitInputDelay());
            }
        }
        throable = PlaceThroable(StartPosition.position);
    }

    private IEnumerator WaitInputDelay()
    {
        controls.Player.Disable();
        yield return new WaitForSecondsRealtime(0.5f);
        controls.Player.Enable();
        coroutine = null;
    }

    private void OnEnable()
    {
        controls.Player.Enable();
    }
    private void Start()
    {
        throable = PlaceThroable(StartPosition.position);
    }
    CoroutineHandle moveCoroutine;
    private void Shoot(InputAction.CallbackContext ctx)
    {
        if (throable != null && throable.gameObject.activeSelf && moveCoroutine == null)
        {
            OnFire.Invoke(throable);
            moveCoroutine = this.RunCoroutine(throable.MoveProjectile(EndPosition, speed));
            moveCoroutine.OnComplete += OnMoveComplete;
        }
    }

    private Throable PlaceThroable(Vector2 position)
    {
        var newObject = pool.GetNextFromPool();

        if (newObject != null && newObject.TryGetComponent(out Throable throable))
        {
            throable.transform.position = position;
            throable.transform.rotation = Quaternion.identity;
            return throable;
        }
        else
        {
            Debug.LogError("There's no Throable Component on object from pool");
            return null;
        }
    }

    private void OnMoveComplete(CoroutineHandle handle)
    {
        moveCoroutine.OnComplete -= OnMoveComplete;
        StopCoroutine(handle);
        moveCoroutine = null;
        OnHit.Invoke();
    }

    private void OnDisable()
    {
        controls.Player.Disable();
    }

    private void OnDestroy()
    {
        if (controls != null)
            controls.Player.Shoot.performed -= Shoot;
        if (target != null)
            target.OnHitResult -= ChooseActionOnResult;
    }
}