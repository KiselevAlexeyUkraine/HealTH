using UnityEngine;
using UnityEngine.UI;

namespace UI.Game
{
    /// <summary>
    /// Класс отвечает за управление страницей "Неудачи", предоставляя кнопки для перезапуска уровня или выхода в меню.
    /// </summary>
    public class FailedPage : BasePage
    {
        [SerializeField]
        private Button _restart; // Кнопка для перезапуска текущего уровня.
        [SerializeField]
        private Button _menu; // Кнопка для перехода в главное меню.

        /// <summary>
        /// Подписываемся на события кнопок при инициализации объекта.
        /// </summary>
        private void Awake()
        {
            // Перезапуск текущей сцены (уровня).
            _restart.onClick.AddListener(() => { SceneSwitcher.Instance.LoadScene(SceneSwitcher.Instance.CurrentScene); });

            // Переход в главное меню.
            _menu.onClick.AddListener(() => { PageSwitcher.Open(PageName.Menu); });
        }

        /// <summary>
        /// Убираем подписки с событий кнопок при уничтожении объекта, чтобы избежать утечек памяти.
        /// </summary>
        private void OnDestroy()
        {
            _restart.onClick.RemoveAllListeners(); // Убираем все подписки с кнопки перезапуска.
            _menu.onClick.RemoveAllListeners(); // Убираем все подписки с кнопки выхода в меню.
        }
    }
}
