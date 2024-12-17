using UnityEngine;
using TMPro;

public class CursorToggle : MonoBehaviour
{
    private bool isCursorVisible = false; // Состояние видимости курсора
    [SerializeField] private TMP_Text textTips;

    private PlayerStats playerStats;
    private PlayerMovementRidgitBody playerMovementRidgitBody;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        MenuSwitcher.instance.OpenMenu("");
        textTips.text = "НАЖМИ \"Esc\"\r\nМеню";
        playerStats = GetComponent<PlayerStats>();
        playerMovementRidgitBody = GetComponent<PlayerMovementRidgitBody>();
    }

    void Update()
    {
        // Проверяем, была ли нажата клавиша ESC
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleCursorVisibility();
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
            MenuSwitcher.instance.OpenMenu(""); // Закрываем меню
            if (textTips != null)
            {
                textTips.text = "НАЖМИ \"Esc\"\r\nМеню";
            }

        }
    }

    private void HandleMenuVisibility()
    {
        if (SceneSwitcher.instance.CurrentScene == 0)
        {
            MenuSwitcher.instance.OpenMenu("MainMenu"); // Открываем главное меню
            TimeManager.instance.PauseGame(); // Пауза игры
            if (textTips != null)
            {
                textTips.text = "НАЖМИ \"Esc\"\r\nОбзор";
            }
        }
        else if (SceneSwitcher.instance.CurrentScene > 0)
        {
            if (playerStats.IsDeath == true)
            {
                MenuSwitcher.instance.OpenMenu(MenuNames.FaildGameMenu.ToString());
            }
            else
            {
                MenuSwitcher.instance.OpenMenu(MenuNames.ContinuationGameMenu.ToString());
                
            }

            if (textTips != null)
            {
                textTips.text = "НАЖМИ \"Esc\"\r\nОбзор";
            }
            if (playerMovementRidgitBody.isMove == false)
            {
                MenuSwitcher.instance.OpenMenu("ResultGameMenu");
            }

            TimeManager.instance.PauseGame();
        }
    }

    public void IsGameOrContinionGameMenu() 
    {
        if (playerStats.IsDeath == false && playerMovementRidgitBody.isFinishGame == false)
        {
            MenuSwitcher.instance.OpenMenu("ContinuationGameMenu");
            Debug.Log("Продолжить игру выводит");
        }
        else if (playerStats.IsDeath == true)
        {
            MenuSwitcher.instance.OpenMenu("FaildGameMenu");
        }
        else if (playerMovementRidgitBody.isFinishGame == true)
        {
            MenuSwitcher.instance.OpenMenu("ResultGameMenu");
            Debug.Log("Результат игру выводит");
        }
    }
}