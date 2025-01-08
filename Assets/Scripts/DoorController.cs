using Player;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    private Animator doorAnimator; // �������� �����
    [SerializeField] private int requiredKeys = 1; // ���������� ������, ����������� ��� �������� �����
    private PlayerStats playerStats; // ������ �� ������ PlayerStats
    private bool isOpen = true; // ����, �����������, ������� �� �����

    private void Start()
    {
        // �������� ������ �� ������ PlayerStats
        playerStats = GameObject.Find("Character").GetComponent<PlayerStats>();
        doorAnimator = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isOpen)
        {
            if (other.CompareTag("Player"))
            {
                // ���������, ���� �� � ������ ���������� ������
                if (playerStats.Key >= requiredKeys)
                {
                    OpenDoor();
                }
                else
                {
                    Debug.Log("� ��� ������������ ������ ��� �������� �����.");
                }
            }
        }
    }

    private void OpenDoor()
    {
        // ������������� �������� �������� �����
        if (doorAnimator != null)
        {
            doorAnimator.SetTrigger("Open"); // ��������������, ��� � ��� ���� ������� "Open" � ���������
        }

        // ��������� ���������� ������ � ������
        playerStats.ChangeScore(ScoreType.Key, -requiredKeys); // ��������� ���������� ������
        isOpen = false; // ����� ������ ��������� ��������
    }
}
