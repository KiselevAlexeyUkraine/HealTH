using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    [SerializeField] private GameObject objectPrefab; // Префаб игрока
    [SerializeField] private Transform spawnPoint; // Точка спавна

    private void Start()
    {
        SpawnPlayers();
    }

    private void SpawnPlayers()
    {
        // Проверяем, что префаб игрока и точка спавна заданы
        if (objectPrefab != null && spawnPoint != null)
        {
            // Спавним игрока в заданной позиции
            Instantiate(objectPrefab, spawnPoint.position, spawnPoint.rotation);
            Debug.Log("Обьект успешно заспавнен в позиции: " + spawnPoint.position);
        }
        else
        {
            Debug.LogError("Префаб игрока или точка спавна не заданы!");
        }
    }
}
