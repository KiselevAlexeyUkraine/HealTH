using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    [SerializeField] private GameObject objectPrefab; // ������ ������
    [SerializeField] private Transform spawnPoint; // ����� ������

    private void Start()
    {
        SpawnPlayers();
    }

    private void SpawnPlayers()
    {
        // ���������, ��� ������ ������ � ����� ������ ������
        if (objectPrefab != null && spawnPoint != null)
        {
            // ������� ������ � �������� �������
            Instantiate(objectPrefab, spawnPoint.position, spawnPoint.rotation);
            Debug.Log("������ ������� ��������� � �������: " + spawnPoint.position);
        }
        else
        {
            Debug.LogError("������ ������ ��� ����� ������ �� ������!");
        }
    }
}
