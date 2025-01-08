using Player;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    private Animator doorAnimator; // Аниматор двери
    [SerializeField] private int requiredKeys = 1; // Количество ключей, необходимых для открытия двери
    private PlayerStats playerStats; // Ссылка на скрипт PlayerStats
    private bool isOpen = true; // Флаг, указывающий, открыта ли дверь

    private void Start()
    {
        // Получаем ссылку на скрипт PlayerStats
        playerStats = GameObject.Find("Character").GetComponent<PlayerStats>();
        doorAnimator = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isOpen)
        {
            if (other.CompareTag("Player"))
            {
                // Проверяем, есть ли у игрока достаточно ключей
                if (playerStats.Key >= requiredKeys)
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
        playerStats.ChangeScore(ScoreType.Key, -requiredKeys); // Уменьшаем количество ключей
        isOpen = false; // Дверь теперь считается закрытой
    }
}
