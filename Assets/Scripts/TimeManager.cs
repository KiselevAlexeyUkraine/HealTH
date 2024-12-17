using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager instance;

    public float TimeScale { get => Time.timeScale; }

    public float TimeDeltaTime { get => Time.deltaTime; }

    public float TimeFixedTime { get => Time.fixedDeltaTime; }

    private bool isPauseGame = true;
    private bool isPauseFail = true;

    private void Awake()
    {
        instance = this;
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
    }

    public void ContinionGame()
    {
        Time.timeScale = 1;
    }

    public void StartAndContinuationGame()
    {
        if (isPauseGame == true)
        {
            MenuSwitcher.instance.OpenMenu(MenuNames.ContinuationGameMenu.ToString());
            PauseGame();
        }
        else
        {
            MenuSwitcher.instance.OpenMenu("");
            ContinionGame();  
        }
        Debug.Log("StartAndContinuationGame");
        isPauseGame = !isPauseGame;
    }

    public void StartAndContinuationFailGame()
    {
        if (isPauseFail == true)
        {
            MenuSwitcher.instance.OpenMenu("");
            PauseGame();
        }
        else
        {
            MenuSwitcher.instance.OpenMenu(MenuNames.FaildGameMenu.ToString());
        }

        isPauseFail = !isPauseFail;
        Debug.Log("StartAndContinuationFailGame");
    }
}
