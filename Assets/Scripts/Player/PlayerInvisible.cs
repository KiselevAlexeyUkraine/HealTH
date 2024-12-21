using UnityEngine;

public class PlayerInvisible : MonoBehaviour
{
    [SerializeField] private KeyCode invisibilityKey = KeyCode.X; // Кнопка для активации невидимости
    [SerializeField] private float invisibilityDuration = 5f; // Длительность невидимости
    [SerializeField] private float invisibilityCooldown = 10f; // Время перезарядки невидимости
    [SerializeField] private GameObject invisibilityPrefab = null; // Объект, который будет скрыт
    private bool canBecomeInvisible = true; // Флаг, указывающий, может ли персонаж стать невидимым
    private float invisibilityTimer = 0f; // Таймер для отслеживания времени невидимости
    private float cooldownTimer = 0f; // Таймер для отслеживания времени перезарядки
    private PlayerStats playerStats;

    void Start()
    {
        playerStats = GetComponent<PlayerStats>(); // Получаем компонент PlayerStats
    }

    void Update()
    {
        // Проверяем, может ли игрок стать невидимым и нажата ли клавиша
        if (Input.GetKeyDown(invisibilityKey) && canBecomeInvisible && playerStats.Helth > 0)
        {
            BecomeInvisible();
        }

        // Если игрок невидим, обновляем таймер
        if (!canBecomeInvisible)
        {
            invisibilityTimer += Time.deltaTime;

            // Проверяем, истекло ли время невидимости
            if (invisibilityTimer >= invisibilityDuration)
            {
                BecomeVisible();
            }
        }

        // Если игрок не может стать невидимым, обновляем таймер перезарядки
        if (!canBecomeInvisible)
        {
            cooldownTimer += Time.deltaTime;

            // Проверяем, истекло ли время перезарядки
            if (cooldownTimer >= invisibilityCooldown)
            {
                canBecomeInvisible = true; // Разрешить следующий вызов
                cooldownTimer = 0f; // Сбрасываем таймер
            }
        }
    }

    private void BecomeInvisible()
    {
        canBecomeInvisible = false; // Запретить повторное использование
        invisibilityPrefab.SetActive(false); // Делаем объект невидимым
        playerStats.DecreaseMotivationForEnemy(1); // Забираем одну единицу здоровья за инвиз
        invisibilityTimer = 0f; // Сбрасываем таймер

        // Уведомляем врагов о том, что игрок стал невидимым
        NotifyEnemiesInvisible();
    }

    private void BecomeVisible()
    {
        invisibilityPrefab.SetActive(true); // Делаем объект видимым
        invisibilityTimer = 0f; // Сбрасываем таймер для невидимости

        // Уведомляем врагов о том, что игрок стал видимым
        NotifyEnemiesVisible();
    }

    private void NotifyEnemiesInvisible()
    {
        // Здесь вы можете уведомить всех врагов о том, что игрок стал невидимым
        EnemyBehavior[] enemies = FindObjectsOfType<EnemyBehavior>();
        foreach (var enemy in enemies)
        {
            enemy.OnPlayerInvisible();
        }
    }

    private void NotifyEnemiesVisible()
    {
        // Здесь вы можете уведомить всех врагов о том, что игрок стал видимым
        EnemyBehavior[] enemies = FindObjectsOfType<EnemyBehavior>();
        foreach (var enemy in enemies)
        {
            enemy.OnPlayerVisible();
        }
    }
}