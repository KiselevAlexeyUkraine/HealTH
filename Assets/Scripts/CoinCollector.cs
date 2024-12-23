using UnityEngine;

public class CoinCollector : MonoBehaviour
{
    PlayerStats playerStats;

    void Start()
    {
        playerStats = GetComponent<PlayerStats>();
    }

    void OnTriggerEnter(Collider other)
    {
        // Проверяем, является ли объект монетой
        if (other.CompareTag("Coin"))
        {
            playerStats.AddScore((int)ScoreValutes.scoreCoins);
            Destroy(other.gameObject); // Уничтожаем
        }
        if (other.CompareTag("Keys"))
        {
            playerStats.AddScore((int)ScoreValutes.scoreKeys);
            Destroy(other.gameObject); // Уничтожаем
        }

        if (other.CompareTag("Bright"))
        {
            playerStats.AddScore((int)ScoreValutes.scoreMotivations);
            Destroy(other.gameObject); // Уничтожаем
        }
    }
}