using UnityEngine;

public class CoinCollector : MonoBehaviour
{
    [SerializeField] private PlayerStats playerStats;


    void OnTriggerEnter(Collider other)
    {
        // Проверяем, является ли объект монетой
        if (other.CompareTag("Coin"))
        {
            playerStats.AddScore((int)ScoreValutes.scoreCoins);
            Destroy(other.gameObject); // Уничтожаем
        }
        if (other.CompareTag("Dollars"))
        {
            playerStats.AddScore((int)ScoreValutes.scoreDollars);
            Destroy(other.gameObject); // Уничтожаем
        }

        if (other.CompareTag("Bright"))
        {
            playerStats.AddScore((int)ScoreValutes.scoreMotivations);
            Destroy(other.gameObject); // Уничтожаем
        }

    }


}