using System;
using TMPro;
using UnityEngine;

namespace Player
{
    /// <summary>
    /// Класс для управления статистикой игрока, включая монеты, ключи и здоровье.
    /// </summary>
    public class PlayerStats : MonoBehaviour
    {
        private PlayerHealth _health; // Ссылка на компонент здоровья игрока.

        [SerializeField] private TextMeshProUGUI _textMeshProCoin; // Текстовое поле для отображения количества монет.
        [SerializeField] private TextMeshProUGUI _textMeshProKeys; // Текстовое поле для отображения количества ключей.

        [SerializeField]
        private int coin = 0; // Текущее количество монет.
        [SerializeField]
        private int key = 0; // Текущее количество ключей.

        // События, которые уведомляют о изменении монет и ключей.
        public event Action<int> OnCoinChanged;
        public event Action<int> OnKeyChanged;

        // Свойство для получения количества ключей.
        public int Key => key;

        /// <summary>
        /// Инициализация компонента здоровья при запуске.
        /// </summary>
        private void Awake()
        {
            _health = GetComponent<PlayerHealth>();
        }

        /// <summary>
        /// Универсальный метод для изменения значений ресурсов (монеты, ключи, здоровье).
        /// </summary>
        /// <param name="type">Тип ресурса, который нужно изменить.</param>
        /// <param name="amount">Количество для изменения (может быть отрицательным).</param>
        public void ChangeScore(ScoreType type, int amount)
        {
            switch (type)
            {
                case ScoreType.Coin:
                    // Увеличиваем или уменьшаем монеты, не допуская отрицательных значений.
                    coin = Mathf.Max(0, coin + amount);
                    OnCoinChanged?.Invoke(coin); // Уведомляем слушателей об изменении монет.
                    break;

                case ScoreType.Key:
                    // Увеличиваем или уменьшаем ключи, не допуская отрицательных значений.
                    key = Mathf.Max(0, key + amount);
                    OnKeyChanged?.Invoke(key); // Уведомляем слушателей об изменении ключей.
                    break;

                case ScoreType.Health:
                    // Увеличиваем здоровье только положительным значением.
                    if (amount > 0) _health.IncreaseHealth();
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }

            UpdateScore(); // Обновляем отображение UI.
        }

        /// <summary>
        /// Обновляет текстовые поля для монет и ключей.
        /// </summary>
        private void UpdateScore()
        {
            _textMeshProCoin.text = coin.ToString(); // Обновляем текст для монет.
            _textMeshProKeys.text = key.ToString(); // Обновляем текст для ключей.
        }
    }
}
