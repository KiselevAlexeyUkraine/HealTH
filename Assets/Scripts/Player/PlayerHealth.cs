using System;
using UnityEngine;

namespace Player
{
    // Класс отвечает за управление здоровьем игрока, его увеличением, уменьшением и связными событиями.
    public class PlayerHealth : MonoBehaviour
    {
        public Action OnIncrease; // Событие, вызываемое при увеличении здоровья.
        public Action OnDecrease; // Событие, вызываемое при уменьшении здоровья.

        [SerializeField]
        private PlayerDash _dash; // Ссылка на компонент рывка игрока.
        [SerializeField]
        private int _maxHealth = 5; // Максимальное значение здоровья.
        [SerializeField]
        private int _health = 5; // Текущее значение здоровья.
        [SerializeField]
        private EnemyBehavior _enemyBehavior; // Ссылка на поведение врага.
        [SerializeField]
        private CarForGame _carForGame; // Ссылка на поведение врага.

        public int MaxHealth => _maxHealth; // Свойство для получения максимального здоровья.
        public int Health => _health; // Свойство для получения текущего здоровья.

        private void Awake()
        {
            _health = _maxHealth; // Устанавливаем здоровье на максимум при старте.
            _dash.OnDash += DecreaseHealth; // Подписываемся на событие рывка для уменьшения здоровья.
            _enemyBehavior.EnemyAttack += DecreaseHealth; // Подписываемся на атаку врага для уменьшения здоровья.
            _carForGame.PlayerDamaged += DecreaseHealth; // Подписываемся на атаку машины для уменьшения здоровья.
        }

        // Увеличивает здоровье игрока, не превышая максимальное значение.
        public void IncreaseHealth(int amount = 1)
        {
            if (amount <= 0)
            {
                Debug.LogWarning("Attempted to increase health with a non-positive amount.");
                return;
            }

            _health = Mathf.Clamp(_health + amount, 0, _maxHealth); // Увеличиваем здоровье с учетом максимума.
            OnIncrease?.Invoke(); // Вызываем событие увеличения здоровья.
        }

        // Уменьшает здоровье игрока, не опускаясь ниже нуля.
        private void DecreaseHealth()
        {
            _health = Mathf.Clamp(_health - 1, 0, _maxHealth); // Уменьшаем здоровье с учетом минимума.
            OnDecrease?.Invoke(); // Вызываем событие уменьшения здоровья.
        }

        private void OnDestroy()
        {
            _dash.OnDash -= DecreaseHealth; // Отписываемся от события рывка.
            _enemyBehavior.EnemyAttack -= DecreaseHealth; // Отписываемся от события атаки врага.
        }
    }
}
