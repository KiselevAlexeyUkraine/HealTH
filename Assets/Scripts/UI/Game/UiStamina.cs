using Player;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Game
{
    /// <summary>
    /// Класс отвечает за отображение уровня стамины игрока в UI.
    /// Обновляет заполнение шкалы стамины при ее изменении.
    /// </summary>
    public class UiStamina : MonoBehaviour
    {
        [SerializeField]
        private PlayerStamina _stamina; // Ссылка на компонент стамины игрока.
        [SerializeField]
        private Image _staminaFill; // UI-элемент, отображающий уровень стамины.

        /// <summary>
        /// Подписываемся на события изменения стамины при инициализации объекта.
        /// </summary>
        private void Awake()
        {
            if (_stamina == null || _staminaFill == null)
            {
                Debug.LogError("Ссылки на компонент стамины или UI-элемент не назначены!");
                return;
            }

            // Обновляем шкалу стамины при увеличении.
            _stamina.OnIncrease += UpdateStaminaUI;

            // Обновляем шкалу стамины при уменьшении.
            _stamina.OnDecrease += UpdateStaminaUI;

            // Устанавливаем начальное значение шкалы.
            UpdateStaminaUI();
        }

        /// <summary>
        /// Убираем подписки с событий при уничтожении объекта, чтобы избежать утечек памяти.
        /// </summary>
        private void OnDestroy()
        {
            if (_stamina != null)
            {
                _stamina.OnIncrease -= UpdateStaminaUI;
                _stamina.OnDecrease -= UpdateStaminaUI;
            }
        }

        /// <summary>
        /// Обновляет отображение шкалы стамины.
        /// </summary>
        private void UpdateStaminaUI()
        {
            _staminaFill.fillAmount = _stamina.Stamina / _stamina.MaxStamina;
        }
    }
}
