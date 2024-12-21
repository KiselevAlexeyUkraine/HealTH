using UnityEngine;

public class FogController : MonoBehaviour
{
    [SerializeField] private float speed = 3f; // Скорость движения тумана

    void Update()
    {
        // Движение тумана вперед по оси Z
        transform.Translate(Vector3.forward * speed * TimeManager.instance.TimeDeltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Проверяем, если туман столкнулся с игроком
        if (collision.gameObject.CompareTag("Player"))
        {
            // Логика завершения игры
            Debug.Log("Game Over!");
            PlayerStats playerStats = collision.gameObject.GetComponent<PlayerStats>();
            if (playerStats != null)
            {
                playerStats.FaildGame();
            }
        }
    }
}