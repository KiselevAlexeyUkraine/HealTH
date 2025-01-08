using UnityEngine;
using UnityEngine.UI;

namespace UI.Game
{
    /// <summary>
    /// Класс отвечает за управление страницей завершения уровня, предоставляя кнопки для перехода к следующему уровню, перезапуска или выхода в меню.
    /// </summary>
    public class CompletePage : BasePage
    {
        [SerializeField]
        private Button _nextLevel; // Кнопка для перехода к следующему уровню.
        [SerializeField]
        private Button _restart; // Кнопка для перезапуска текущего уровня.
        [SerializeField]
        private Button _exit; // Кнопка для выхода в главное меню.

        /// <summary>
        /// Подписываемся на события кнопок при инициализации объекта.
        /// </summary>
        private void Awake()
        {
            // Переход к следующей сцене (уровню).
            _nextLevel.onClick.AddListener(() => { SceneSwitcher.Instance.LoadNextScene(); });

            // Перезапуск текущей сцены.
            _restart.onClick.AddListener(() => { SceneSwitcher.Instance.LoadScene(SceneSwitcher.Instance.CurrentScene); });

            // Переход в главное меню.
            _exit.onClick.AddListener(() => { PageSwitcher.Open(PageName.Menu); });
        }

        /// <summary>
        /// Убираем подписки с событий кнопок при уничтожении объекта, чтобы избежать утечек памяти.
        /// </summary>
        private void OnDestroy()
        {
            _nextLevel.onClick.RemoveAllListeners(); // Убираем все подписки с кнопки перехода на следующий уровень.
            _restart.onClick.RemoveAllListeners(); // Убираем все подписки с кнопки перезапуска.
            _exit.onClick.RemoveAllListeners(); // Убираем все подписки с кнопки выхода в меню.
        }
    }
}
