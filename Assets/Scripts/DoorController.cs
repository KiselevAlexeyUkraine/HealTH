using UnityEngine;

public class DoorController : MonoBehaviour
{
    [SerializeField] private Animator doorAnimator; // �������� �����
    [SerializeField] private int requiredKeys = 1; // ���������� ������, ����������� ��� �������� �����
    private PlayerStats playerStats; // ������ �� ������ PlayerStats
    private bool IsOpen = true;

    private void Start()
    {
        // �������� ������ �� ������ PlayerStats
        playerStats = GameObject.Find("Character").GetComponent<PlayerStats>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (IsOpen == true)
        {
            if (other.CompareTag("Player"))
            {
                // ���������, ���� �� � ������ ���������� ������
                if (playerStats.ScoreKeys >= requiredKeys)
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
        playerStats.MinusKeys(); // ��������� ���������� ������
        IsOpen = false;
    }
}