using UnityEngine;

public class CursorToggle : MonoBehaviour
{

    [SerializeField] private GameObject UiStatsPlayer;

    private PlayerMovementRidgitBody playerMovementRidgitBody;
    private PlayerStats playerStats;
    private bool isCursorVisible;
    
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        MenuSwitcher.instance.OpenMenu(MenuNames.None);
        TimeManager.instance.ContinionGame();
        playerStats = GetComponent<PlayerStats>();
        playerMovementRidgitBody = GetComponent<PlayerMovementRidgitBody>();
        CurrentSceneForStats();
    }

    private void Update()
    {
        // Проверяем, была ли нажата клавиша ESC
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleCursorVisibility();
        }
    }

    private void CurrentSceneForStats()
    {
        if (SceneSwitcher.instance.CurrentScene == 0)
        {
            UiStatsPlayer.SetActive(false);
        }
        else
        {
            UiStatsPlayer.SetActive(true);
        }
    }

    public void ToggleCursorVisibility()
    {
        // Переключаем состояние видимости курсора
        isCursorVisible = !isCursorVisible;

        // Устанавливаем видимость курсора
        Cursor.visible = isCursorVisible;

        if (isCursorVisible)
        {
            Cursor.lockState = CursorLockMode.None; // Разблокируем курсор
            HandleMenuVisibility();
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked; // Блокируем курсор в центре экрана
            if (playerStats.IsDeath == false)
            {
                TimeManager.instance.ContinionGame(); // Продолжаем игру
            }
            MenuSwitcher.instance.OpenMenu(MenuNames.None); // Закрываем меню
        }
    }

    private void HandleMenuVisibility()
    {
        if (SceneSwitcher.instance.CurrentScene == 0)
        {
            MenuSwitcher.instance.OpenMenu(MenuNames.MainMenu); // Открываем главное меню
            TimeManager.instance.PauseGame(); // Пауза игры
        }
        else if (SceneSwitcher.instance.CurrentScene > 0)
        {
            if (playerStats.IsDeath)
            {
                MenuSwitcher.instance.OpenMenu(MenuNames.FailedGame);
            }
            else
            {
                MenuSwitcher.instance.OpenMenu(MenuNames.ContinuationGame);
                
            }
            
            if (playerMovementRidgitBody.isMove == false)
            {
                MenuSwitcher.instance.OpenMenu(MenuNames.ResultGame);
            }

            TimeManager.instance.PauseGame();
        }
    }

    public void IsGameOrContinionGameMenu() // для нашей кнопки выхода с меню громкости
    {
        if (SceneSwitcher.instance.CurrentScene == 0)
        {
            MenuSwitcher.instance.OpenMenu(MenuNames.MainMenu); // Открываем главное меню
            return;
        }
        
        if (playerStats.IsDeath == false && playerMovementRidgitBody.isFinishGame == false)
        {
            MenuSwitcher.instance.OpenMenu(MenuNames.ContinuationGame);
        }
        else if (playerStats.IsDeath)
        {
            MenuSwitcher.instance.OpenMenu(MenuNames.FailedGame);
        }
        else if (playerMovementRidgitBody.isFinishGame)
        {
            MenuSwitcher.instance.OpenMenu(MenuNames.ResultGame);
        }
    }
}