using UnityEngine;

public class PlayerCameraController : MonoBehaviour
{
    [Header("Camera Settings")]
    [SerializeField] private Transform target; // Объект, за которым следует камера
    [SerializeField] private float distance = 5f; // Максимальное расстояние от персонажа
    [SerializeField] private float sensitivity = 100f; // Чувствительность вращения
    [SerializeField] private float verticalClamp = 80f; // Ограничение вертикального угла
    [SerializeField] private Vector3 offset = Vector3.up; // Смещение относительно центра персонажа
    [SerializeField] private LayerMask collisionLayers; // Слои, с которыми камера взаимодействует

    private float currentYaw; // Угол вращения по горизонтали
    private float currentPitch; // Угол вращения по вертикали

    public void Look(Vector2 lookInput)
    {
        // Управление углами
        currentYaw += lookInput.x * sensitivity * Time.deltaTime;
        currentPitch -= lookInput.y * sensitivity * Time.deltaTime;

        // Ограничиваем вертикальный угол
        currentPitch = Mathf.Clamp(currentPitch, -verticalClamp, verticalClamp);
    }

    private void LateUpdate()
    {
        if (!target) return;

        // Рассчитываем целевую позицию камеры
        Quaternion rotation = Quaternion.Euler(currentPitch, currentYaw, 0);
        Vector3 desiredPosition = target.position + offset - (rotation * Vector3.forward * distance);

        // Проверка на препятствия
        Vector3 adjustedPosition = HandleCollision(target.position + offset, desiredPosition);

        // Перемещаем камеру
        transform.position = adjustedPosition;

        // Направляем камеру на цель
        transform.LookAt(target.position + offset);
    }

    private Vector3 HandleCollision(Vector3 targetPosition, Vector3 desiredPosition)
    {
        // Проверяем луч от персонажа к целевой позиции камеры
        if (Physics.Raycast(targetPosition, desiredPosition - targetPosition, out RaycastHit hit, distance, collisionLayers))
        {
            // Если луч пересекает объект, устанавливаем позицию камеры рядом с точкой пересечения
            return hit.point + hit.normal * 0.2f; // Смещение от поверхности
        }

        // Если пересечений нет, возвращаем целевую позицию
        return desiredPosition;
    }
}