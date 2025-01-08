using System.Collections.Generic;
using Player;
using UnityEngine;

namespace UI.Game
{
    /// <summary>
    /// Класс для управления отображением здоровья игрока в UI.
    /// Создает визуальные элементы здоровья и обновляет их состояние в зависимости от текущего здоровья игрока.
    /// </summary>
    public class UiHealth : MonoBehaviour
    {
        [SerializeField]
        private PlayerHealth _health; // Ссылка на компонент здоровья игрока.
        [SerializeField]
        private GameObject _healthContainer; // Контейнер для отображения визуальных элементов здоровья.
        [SerializeField]
        private GameObject _healthPointPrefab; // Префаб для представления одного элемента здоровья.

        private List<GameObject> _healthImages; // Список для хранения визуальных элементов здоровья.

        /// <summary>
        /// Инициализация визуальных элементов здоровья и подписка на события изменения здоровья.
        /// </summary>
        private void Awake()
        {
            // Проверка на корректное назначение компонентов.
            if (_health == null || _healthContainer == null || _healthPointPrefab == null)
            {
                Debug.LogError("Не все ссылки заданы в инспекторе!");
                return;
            }

            // Инициализируем список для хранения элементов здоровья.
            _healthImages = new List<GameObject>();

            // Создаем элементы здоровья на основе максимального количества здоровья игрока.
            for (var i = 0; i < _health.MaxHealth; i++)
            {
                var go = Instantiate(_healthPointPrefab, _healthContainer.transform);
                _healthImages.Add(go); // Добавляем созданный элемент в список.
            }

            // Подписываемся на события изменения здоровья.
            _health.OnIncrease += HandleHealthIncrease;
            _health.OnDecrease += HandleHealthDecrease;

            // Инициализируем UI на основе текущего состояния здоровья.
            UpdateHealthUI();
        }

        /// <summary>
        /// Убирает подписки с событий при уничтожении объекта, чтобы избежать утечек памяти.
        /// </summary>
        private void OnDestroy()
        {
            if (_health != null)
            {
                _health.OnIncrease -= HandleHealthIncrease;
                _health.OnDecrease -= HandleHealthDecrease;
            }
        }

        /// <summary>
        /// Обработчик события увеличения здоровья.
        /// </summary>
        private void HandleHealthIncrease()
        {
            UpdateHealthUI(); // Обновляем весь UI здоровья.
        }

        /// <summary>
        /// Обработчик события уменьшения здоровья.
        /// </summary>
        private void HandleHealthDecrease()
        {
            UpdateHealthUI(); // Обновляем весь UI здоровья.
        }

        /// <summary>
        /// Обновляет UI здоровья на основе текущего состояния.
        /// </summary>
        private void UpdateHealthUI()
        {
            for (int i = 0; i < _healthImages.Count; i++)
            {
                _healthImages[i].gameObject.SetActive(i < _health.Health); // Активируем элементы, соответствующие текущему здоровью.
            }
        }
    }
}
