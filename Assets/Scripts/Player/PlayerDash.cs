using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Player
{
    // Класс отвечает за механику рывка (Dash) персонажа.
    public class PlayerDash : MonoBehaviour
    {
        public Action OnDash; // Событие, вызываемое при выполнении рывка.

        [SerializeField]
        private PlayerMovement _movement; // Ссылка на компонент, отвечающий за перемещение игрока.
        [SerializeField]
        private PlayerHealth _health; // Ссылка на компонент, отвечающий за здоровье игрока.
        [SerializeField]
        private float _dashDistance = 4f; // Дистанция рывка.
        [SerializeField]
        private float _dashDuration = 0.5f; // Длительность выполнения рывка.
        [SerializeField]
        private float _dashCooldown = 1f; // Перезарядка между рывками.

        private bool _canDash = true; // Флаг, указывающий, можно ли использовать рывок.
        private float _elapsedTime; // Время, прошедшее с начала рывка.

        private void Update()
        {
            // Проверяем ввод игрока, доступность рывка и достаточный уровень здоровья.
            if (InputPlayer.Dash && _canDash && CanPerformDash())
            {
                OnDash?.Invoke(); // Вызываем событие рывка.
                Dash().Forget(); // Асинхронно выполняем рывок.
            }
        }

        // Вычисляет конечную позицию рывка.
        private Vector3 CalculateEndPosition(Vector3 startPosition, Vector3 forward)
        {
            return startPosition + Vector3.ClampMagnitude(forward * _dashDistance, _dashDistance);
        }

        // Проверяем хватает ли жизней для рывка
        private bool CanPerformDash()
        {
            return _health.Health > 1;
        }

        // Асинхронный метод выполнения рывка.
        private async UniTaskVoid Dash()
        {
            _canDash = false; // Блокируем рывок.
            _elapsedTime = 0f; // Сбрасываем время выполнения рывка.

            var forward = _movement.transform.forward; // Направление вперед.
            var startPosition = _movement.transform.position; // Начальная позиция.
            var endPosition = CalculateEndPosition(startPosition, forward); // Конечная позиция рывка.

            // Интерполяция позиции персонажа во время рывка.
            while (_elapsedTime < _dashDuration)
            {
                _elapsedTime += Time.deltaTime;

                var position = Vector3.Lerp(startPosition, endPosition, _elapsedTime / _dashDuration); // Линейная интерполяция позиции.
                var direction = position - _movement.transform.position; // Вычисляем направление движения.

                _movement.MoveForward(direction); // Перемещаем персонажа в заданном направлении.

                await UniTask.NextFrame(); // Ждем следующий кадр.
            }

            // Ожидаем окончания перезарядки.
            await UniTask.WaitForSeconds(_dashCooldown);

            _canDash = true; // Разрешаем использовать рывок снова.
        }
    }
}
