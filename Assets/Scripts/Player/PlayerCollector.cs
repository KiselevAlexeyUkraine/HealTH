using UnityEngine;

namespace Player
{
    // Этот класс отвечает за сбор объектов с различными тегами и обработку их взаимодействия с игроком.
    public class PlayerCollector : MonoBehaviour
    {
        [SerializeField] private PlayerStats _stats; // Ссылка на компонент, отвечающий за статистику игрока.

        // Метод вызывается при входе в триггер-коллайдер.
        void OnTriggerEnter(Collider other)
        {
            // Проверка, инициализирован ли компонент PlayerStats.
            if (_stats == null)
            {
                Debug.LogError("PlayerStats is not assigned!"); // Вывод ошибки в консоль, если компонент не настроен.
                return; // Прерываем выполнение метода, чтобы избежать ошибок.
            }

            // Обработка различных тегов объектов, с которыми сталкивается игрок.
            if (other.CompareTag("Health"))
            {
                _stats.ChangeScore(ScoreType.Health, 1); // Увеличиваем здоровье.
                Destroy(other.gameObject); // Удаляем объект после взаимодействия.
            }
            else if (other.CompareTag("Coin"))
            {
                _stats.ChangeScore(ScoreType.Coin, 1); // Увеличиваем счет за монеты.
                Destroy(other.gameObject); // Удаляем объект после взаимодействия.
            }
            else if (other.CompareTag("Key"))
            {
                _stats.ChangeScore(ScoreType.Key, 1); // Увеличиваем счет за ключи.
                Destroy(other.gameObject); // Удаляем объект после взаимодействия.
            }
            else
            {
                Debug.LogWarning($"Unhandled tag: {other.tag}"); // Выводим предупреждение для неизвестного тега.
            }
        }
    }
}
