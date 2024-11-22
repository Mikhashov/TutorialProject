using UnityEngine;

public class CharacterOnPlatform : MonoBehaviour
{
    private Transform originalParent;

    private void OnCollisionEnter(Collision collision)
    {
        // Проверяем, если объект имеет компонент MovingPlatform
        if (collision.gameObject.GetComponent<MovingPlatform>())
        {
            originalParent = transform.parent;
            transform.SetParent(collision.transform); // Делаем платформу родителем игрока
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        // Убираем игрока из платформы, если он перестал с ней соприкасаться
        if (collision.gameObject.GetComponent<MovingPlatform>())
        {
            transform.SetParent(originalParent);
        }
    }
}