using UnityEngine;
using TMPro;

public class FinishLine : MonoBehaviour
{
    [SerializeField] private PlayerMovementRidgitBody playerMovement; // Ссылка на скрипт движения игрока
    [SerializeField] private GameObject cameraController; // Ссылка на контроллер камеры
    [SerializeField] private GameObject UIPlayerStatsResult; // Ссылка на UI для отображения результатов
    [SerializeField] private PlayerStats playerStats; // Ссылка на скрипт статистики игрока
    [SerializeField] private TMP_Text finalScoreTextCoin; // Текст для отображения итогового счета монет
    [SerializeField] private TMP_Text finalScoreTextDollar; // Текст для отображения итогового счета долларов
    [SerializeField] private TMP_Text finalScoreTextTime; // Текст для отображения времени
    [SerializeField] private CursorToggle cursorToggle;

    private void Start()
    {
        UIPlayerStatsResult.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Выводим итоговый счет
            ShowFinalScore();
        }
    }

    private void ShowFinalScore()
    {
        // Отключаем движение игрока
        playerMovement.FinishGame();

        // Включаем камеру для вращения
        cursorToggle.ToggleCursorVisibility();
        cameraController.SetActive(true);
        UIPlayerStatsResult.SetActive(true);

        // Отображаем результаты
        finalScoreTextCoin.text = "Монеты: " + playerStats.ScoreCoins; // Измените на нужные значения
        finalScoreTextDollar.text = "Доллары: " + playerStats.ScoreDollars; // Измените на нужные значения
        finalScoreTextTime.text = "Время: " + playerStats.GetFormattedPlayTime(); // Отображаем время
    }
}