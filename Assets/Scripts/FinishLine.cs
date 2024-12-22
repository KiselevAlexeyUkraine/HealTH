using UnityEngine;
using TMPro;

public class FinishLine : MonoBehaviour
{
    [SerializeField] private PlayerMovementRidgitBody playerMovement;

    [SerializeField] private PlayerStats playerStats; 
    [SerializeField] private TMP_Text finalScoreTextCoin; 
    [SerializeField] private TMP_Text finalScoreTextDollar; 
    [SerializeField] private TMP_Text finalScoreTextTime; 
    [SerializeField] private CursorToggle cursorToggle;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ShowFinalScore();
        }
    }

    private void ShowFinalScore()
    {
        playerMovement.FinishGame();
        cursorToggle.ToggleCursorVisibility();

        finalScoreTextCoin.text = "Монеты: " + playerStats.ScoreCoins; 
        finalScoreTextDollar.text = "Доллары: " + playerStats.ScoreDollars; 
        finalScoreTextTime.text = "Время: " + playerStats.GetFormattedPlayTime(); 
    }
}