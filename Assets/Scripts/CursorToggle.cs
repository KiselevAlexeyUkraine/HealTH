using UnityEngine;

public class CursorToggle : MonoBehaviour
{
    [SerializeField]
    private GameObject uiStatsPlayer;
    [SerializeField] 
    private GameObject _character;

    private PlayerMovementRidgitBody playerMovementRidgitBody;
    private PlayerStats _playerStats;
    private bool isCursorVisible;
    
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        MenuSwitcher.instance.OpenMenu(MenuNames.None);
        TimeManager.instance.ContinionGame();

        _playerStats = _character.GetComponent<PlayerStats>();
        playerMovementRidgitBody = _character.GetComponent<PlayerMovementRidgitBody>();
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
            uiStatsPlayer.SetActive(false);
        }
        else
        {
            uiStatsPlayer.SetActive(true);
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
            if (_playerStats.IsDeath == false)
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
            if (_playerStats.IsDeath)
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
        
        if (_playerStats.IsDeath == false && playerMovementRidgitBody.isFinishGame == false)
        {
            MenuSwitcher.instance.OpenMenu(MenuNames.ContinuationGame);
        }
        else if (_playerStats.IsDeath)
        {
            MenuSwitcher.instance.OpenMenu(MenuNames.FailedGame);
        }
        else if (playerMovementRidgitBody.isFinishGame)
        {
            MenuSwitcher.instance.OpenMenu(MenuNames.ResultGame);
        }
    }
}