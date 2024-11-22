using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float speed = 2f; // Скорость движения
    [SerializeField] private float waitTime = 1f; // Ожидание на точке
    public Vector3[] waypoints; // Глобальные точки перемещения

    private int currentWaypointIndex = 0;
    private float waitTimer;

    private void Start()
    {
        // Убедимся, что есть хотя бы одна точка
        if (waypoints.Length == 0)
        {
            waypoints = new Vector3[] { transform.position };
        }
    }

    private void Update()
    {
        if (waypoints.Length < 2) return; // Нужны как минимум две точки

        // Движение к текущей глобальной точке
        Vector3 targetWaypoint = waypoints[currentWaypointIndex];
        transform.position = Vector3.MoveTowards(transform.position, targetWaypoint, speed * Time.deltaTime);

        // Если достигли текущей точки
        if (Vector3.Distance(transform.position, targetWaypoint) < 0.1f)
        {
            if (waitTimer <= 0f)
            {
                // Переход к следующей точке
                currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
                waitTimer = waitTime;
            }
            else
            {
                waitTimer -= Time.deltaTime;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        // Рисуем точки и линии
        if (waypoints != null && waypoints.Length > 0)
        {
            for (int i = 0; i < waypoints.Length; i++)
            {
                Vector3 globalWaypoint = waypoints[i];
                Gizmos.DrawSphere(globalWaypoint, 0.2f);

                // Соединяем текущую точку с следующей
                if (i < waypoints.Length - 1)
                {
                    Vector3 nextWaypoint = waypoints[i + 1];
                    Gizmos.DrawLine(globalWaypoint, nextWaypoint);
                }
                else
                {
                    // Соединяем последнюю точку с первой
                    Vector3 firstWaypoint = waypoints[0];
                    Gizmos.DrawLine(globalWaypoint, firstWaypoint);
                }
            }
        }
    }
}
