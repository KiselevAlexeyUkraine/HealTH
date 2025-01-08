using Services;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Game
{
    /// <summary>
    /// Класс отвечает за управление страницей паузы, предоставляя кнопки для продолжения игры, перезапуска, настроек и выхода.
    /// </summary>
    public class PausePage : BasePage
    {
        [SerializeField]
        private Button _continue; // Кнопка для продолжения игры.
        [SerializeField]
        private Button _restart; // Кнопка для перезапуска текущего уровня.
        [SerializeField]
        private Button _settings; // Кнопка для перехода в меню настроек.
        [SerializeField]
        private Button _exit; // Кнопка для выхода в главное меню.

        [SerializeField]
        private PauseManager _pauseManager; // Менеджер паузы для управления состоянием игры.

        /// <summary>
        /// Подписываемся на события кнопок при инициализации объекта.
        /// </summary>
        private void Awake()
        {
            // Продолжение игры.
            _continue.onClick.AddListener(() => { _pauseManager.SwitchState(); });

            // Перезапуск текущей сцены.
            _restart.onClick.AddListener(() => { SceneSwitcher.Instance.RestartCurrentScene(); });

            // Переход в меню настроек.
            _settings.onClick.AddListener(() => { PageSwitcher.Open(PageName.GameSettings); });

            // Выход в главное меню (предполагается, что сцена 0 - главное меню).
            _exit.onClick.AddListener(() => { SceneSwitcher.Instance.LoadScene(0); });
        }

        /// <summary>
        /// Убираем подписки с событий кнопок при уничтожении объекта, чтобы избежать утечек памяти.
        /// </summary>
        private void OnDestroy()
        {
            _continue.onClick.RemoveAllListeners(); // Убираем подписки с кнопки продолжения игры.
            _restart.onClick.RemoveAllListeners(); // Убираем подписки с кнопки перезапуска.
            _settings.onClick.RemoveAllListeners(); // Убираем подписки с кнопки настроек.
            _exit.onClick.RemoveAllListeners(); // Убираем подписки с кнопки выхода.
        }
    }
}
