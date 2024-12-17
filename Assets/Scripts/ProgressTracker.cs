using UnityEngine;
using UnityEngine.UI;

public class ProgressTracker : MonoBehaviour
{
    public Transform player; // Ссылка на игрока
    [SerializeField] private Transform finishPoint; // Ссылка на финишную точку
    [SerializeField] private Transform startPoint; // Ссылка на стартовую точку
    public Slider distanceSlider; // UI слайдер для отображения прогресса
    public float updateInterval = 1f; // Интервал обновления

    private float lastUpdateTime;
    private float distanceCovered;

    private void Start()
    {
        if (player == null || finishPoint == null || distanceSlider == null || startPoint == null)
        {
            Debug.LogError("Не все ссылки установлены в ProgressTracker.");
            enabled = false; // Отключаем скрипт, если ссылки не установлены
            return;
        }


        // Устанавливаем максимальное значение слайдера
        distanceSlider.maxValue = finishPoint.position.z - startPoint.position.z; // Устанавливаем максимальное значение с учетом стартовой точки
        distanceSlider.value = 0; // Начальное значение слайдера
    }

    private void Update()
    {
        // Проверяем, прошло ли достаточно времени для обновления
        if (Time.time - lastUpdateTime >= updateInterval)
        {
            UpdateDistance();
            lastUpdateTime = Time.time;
        }
    }

    private void UpdateDistance()
    {
        // Вычисляем пройденное расстояние от стартовой точки
        distanceCovered = player.position.z - startPoint.position.z;

        // Обновляем значение слайдера
        //Debug.Log("Пройденное расстояние: " + distanceCovered);
        //Debug.Log("Игрок по координате z: " + player.position.z);
        distanceSlider.value = Mathf.Clamp(distanceCovered, 0, distanceSlider.maxValue);
    }
}