using System;
using UnityEngine;

namespace Player
{
    public class PlayerStamina : MonoBehaviour
    {
        public Action OnIncrease; // Событие для уведомления об увеличении стамины
        public Action OnDecrease; // Событие для уведомления об уменьшении стамины

        [SerializeField]
        private PlayerMovement _movement; // Ссылка на скрипт управления движением игрока

        [SerializeField]
        private float _maxStamina = 100f; // Максимальное количество стамины
        [SerializeField]
        private float _stamina = 100f; // Текущее количество стамины
        [SerializeField]
        private float _stationaryRecoveryAmount = 2f; // Скорость восстановления стамины, если игрок стоит
        [SerializeField]
        private float _movingRecoveryAmount = 1f; // Скорость восстановления стамины, если игрок движется
        [SerializeField]
        private float _recoveryInterval = 1f; // Интервал между восстановлением стамины
        [SerializeField]
        private float _sprintingConsumption = 2f; // Расход стамины во время спринта

        private float _elapsedTime; // Переменная для отслеживания времени между восстановлением стамины

        public float MaxStamina => _maxStamina; // Геттер для максимальной стамины
        public float Stamina => _stamina; // Геттер для текущей стамины

        private void Awake()
        {
            _stamina = _maxStamina; // Устанавливаем стамину на максимум при запуске

            // Подписываемся на событие спринта и уменьшаем стамину
            _movement.OnSprinting += HandleSprinting;
        }

        private void Update()
        {
            RecoverStamina(); // Постоянно пытаемся восстановить стамину
        }

        /// <summary>
        /// Логика восстановления стамины.
        /// </summary>
        private void RecoverStamina()
        {
            // Если игрок спринтует, стамина не восстанавливается
            if (_movement.IsSprinting)
            {
                return;
            }

            // Восстанавливаем стамину, если прошёл интервал восстановления
            if (_elapsedTime >= _recoveryInterval)
            {
                if (_movement.IsMoving) // Игрок движется
                {
                    IncreaseStamina(_movingRecoveryAmount);
                }
                else // Игрок стоит на месте
                {
                    IncreaseStamina(_stationaryRecoveryAmount);
                }

                _elapsedTime = 0f; // Сбрасываем таймер
            }

            _elapsedTime += Time.deltaTime; // Увеличиваем время
        }

        /// <summary>
        /// Увеличение стамины.
        /// </summary>
        /// <param name="amount">Количество для увеличения.</param>
        public void IncreaseStamina(float amount)
        {
            _stamina += amount; // Увеличиваем стамину
            _stamina = Mathf.Clamp(_stamina, 0, _maxStamina); // Ограничиваем значение между 0 и максимумом
            OnIncrease?.Invoke(); // Уведомляем слушателей
        }

        /// <summary>
        /// Уменьшение стамины.
        /// </summary>
        /// <param name="amount">Количество для уменьшения.</param>
        public void DecreaseStamina(float amount)
        {
            if (_stamina > 0) // Уменьшаем стамину только если она больше 0
            {
                _stamina -= amount; // Уменьшаем стамину
                _stamina = Mathf.Clamp(_stamina, 0, _maxStamina); // Ограничиваем значение между 0 и максимумом
                OnDecrease?.Invoke(); // Уведомляем слушателей
            }
        }

        /// <summary>
        /// Обработка расхода стамины во время спринта.
        /// </summary>
        private void HandleSprinting()
        {
            DecreaseStamina(_sprintingConsumption * Time.deltaTime);
        }
    }
}
