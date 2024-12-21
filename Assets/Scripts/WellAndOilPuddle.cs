using UnityEngine;

public class WellAndOilPuddle : MonoBehaviour
{
    private int motivationLoss = 1; // Сколько мотивации теряет игрок
    [SerializeField] private float damageAmount = 5f; // Сколько урона получает игрок
    [SerializeField] private float damageInterval = 1f; // Интервал между нанесением урона в секундах

    private float lastDamageTime; // Время последнего нанесения урона


    private void OnTriggerStay(Collider other )
    {
        if (other.CompareTag("Player") && Time.time >= lastDamageTime + damageInterval) // Убедитесь, что у игрока установлен тег "Player"
        {
            PlayerStats playerStats = other.GetComponent<PlayerStats>();
            if (playerStats != null)
            {
                // Наносим урон игроку
                playerStats.DecreaseMotivationForEnemy(motivationLoss);
                lastDamageTime = Time.time; // Обновляем время последнего нанесения урона
            }
        }
    }

}