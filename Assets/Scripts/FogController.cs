using UnityEngine;

public class FogController : MonoBehaviour
{
    [SerializeField] private float speed = 3f; // Скорость движения тумана

    void Update()
    {
        // Движение тумана вперед по оси Z
        transform.Translate(Vector3.forward * speed * TimeManager.instance.TimeDeltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Проверяем, если туман столкнулся с игроком
        if (other.CompareTag("Player"))
        {
            // Здесь можно добавить логику завершения игры
            Debug.Log("Game Over!");
            PlayerStats playerStats = other.gameObject.GetComponent<PlayerStats>();
            playerStats.DecreaseMotivationForEnemy(100f);
        }
    }


}