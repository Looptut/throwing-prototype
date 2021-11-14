using System.Collections;
using UnityEngine;
/// <summary>
/// Класс объекта для выстрела
/// </summary>
public class Throable : MonoBehaviour
{
    public float Width { get; private set; }
    public float Height { get; private set; }

    [SerializeField] private float timeStep = 0.05f;

    private void Awake()
    {
        var sprite = GetComponentInChildren<SpriteRenderer>();
        Width = 2 * sprite.bounds.extents.x;
        Height = 2 * sprite.bounds.extents.y;
    }
    /// <summary>
    /// Движение объекта к цели со скоростью
    /// </summary>
    /// <param name="endPosition"></param>
    /// <param name="speed"></param>
    /// <returns></returns>
    public IEnumerator MoveProjectile(Vector2 endPosition, float speed)
    {
        float coverage = 0;
        float time = 0;

        Vector2 startPosition = transform.position;
        float distance = Vector2.Distance(startPosition, endPosition);
        while (Vector2.Distance(transform.position, endPosition) > 0.005)
        {
            yield return new WaitForSeconds(timeStep);
            time += timeStep;
            coverage = Mathf.Clamp(time * speed / distance, 0f, 1f);

            var position = Vector2.Lerp(startPosition, endPosition, coverage);
            transform.position = position;

        }
    }
}
