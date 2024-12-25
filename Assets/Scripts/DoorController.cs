using UnityEngine;

public class DoorController : MonoBehaviour
{
    [SerializeField] private Animator doorAnimator; // Аниматор двери
    [SerializeField] private int requiredKeys = 1; // Количество ключей, необходимых для открытия двери
    private PlayerStats playerStats; // Ссылка на скрипт PlayerStats
    private bool IsOpen = true;

    private void Start()
    {
        // Получаем ссылку на скрипт PlayerStats
        playerStats = GameObject.Find("Character").GetComponent<PlayerStats>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (IsOpen == true)
        {
            if (other.CompareTag("Player"))
            {
                // Проверяем, есть ли у игрока достаточно ключей
                if (playerStats.ScoreKeys >= requiredKeys)
                {
                    OpenDoor();
                }
                else
                {
                    Debug.Log("У вас недостаточно ключей для открытия двери.");
                }
            }
        }
    }

    private void OpenDoor()
    {
        // Воспроизводим анимацию открытия двери
        if (doorAnimator != null)
        {
            doorAnimator.SetTrigger("Open"); // Предполагается, что у вас есть триггер "Open" в аниматоре
        }

        // Уменьшаем количество ключей у игрока
        playerStats.MinusKeys(); // Уменьшаем количество ключей
        IsOpen = false;
    }
}