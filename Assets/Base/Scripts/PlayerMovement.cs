using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpHeight = 2f;
    [SerializeField] private float gravity = -9.81f;

    [Header("Camera Reference")]
    [SerializeField] private Transform cameraTransform; // Ссылка на камеру

    private CharacterController characterController;
    private Vector3 velocity;
    private bool isGrounded;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();

        // Если камера не указана, найти основную
        if (!cameraTransform)
        {
            cameraTransform = Camera.main.transform;
        }
    }

    public void Move(Vector2 input, bool jump)
    {
        // Проверка на землю
        isGrounded = characterController.isGrounded;
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // "Заземляем" персонажа
        }

        // Горизонтальное движение относительно направления камеры
        if (input.magnitude > 0)
        {
            Vector3 forward = cameraTransform.forward;
            Vector3 right = cameraTransform.right;

            // Игнорируем вертикальную составляющую
            forward.y = 0f;
            right.y = 0f;

            // Нормализуем, чтобы движение было равномерным
            forward.Normalize();
            right.Normalize();

            Vector3 moveDirection = (forward * input.y + right * input.x).normalized;
            characterController.Move(moveDirection * moveSpeed * Time.deltaTime);

            // Поворачиваем игрока в направлении движения
            if (moveDirection != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
            }
        }

        // Прыжок
        if (jump && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // Применяем гравитацию
        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
    }
}
