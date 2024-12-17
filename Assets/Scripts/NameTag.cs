using UnityEngine;
using TMPro;

public class NameTag : MonoBehaviour
{
    [SerializeField] private string objectName; // Имя объекта
    private TextMeshPro textMeshPro;

    private void Start()
    {
        // Получаем компонент TextMeshPro
        textMeshPro = GetComponentInChildren<TextMeshPro>();
        // Устанавливаем текст
        textMeshPro.text = objectName;

        // Запускаем метод для обновления позиции текста
        InvokeRepeating(nameof(UpdatePosition), 0f, 0.3f); // Обновляем позицию каждые 0.1 секунды
    }

    private void UpdatePosition()
    {
        // Обновляем позицию текста, чтобы он всегда смотрел на камеру
        if (Camera.main != null)
        {
            transform.LookAt(Camera.main.transform);
        }
    }

    private void OnDestroy()
    {
        // Останавливаем InvokeRepeating, когда объект уничтожается
        CancelInvoke(nameof(UpdatePosition));
    }
}