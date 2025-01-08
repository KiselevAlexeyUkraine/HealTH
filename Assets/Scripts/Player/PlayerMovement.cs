using System;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMovement : MonoBehaviour
    {
        // События, связанные с движением, спринтом и прыжком.
        public Action OnMoving;
        public Action OnSprinting;
        public Action OnJump;

        [SerializeField]
        private PlayerCamera _camera; // Ссылка на управление камерой.
        [SerializeField]
        private PlayerStamina _stamina; // Ссылка на стамину игрока.
        [SerializeField]
        private GroundChecker _groundChecker; // Ссылка на проверку земли.

        [SerializeField]
        private float _moveSpeed = 5f; // Скорость обычного движения.
        [SerializeField]
        private float _sprintSpeed = 8f; // Скорость при спринте.
        [SerializeField]
        private float _rotationSpeed = 10f; // Скорость вращения.
        [SerializeField]
        private float _jumpForce = 10f; // Сила прыжка.
        [SerializeField]
        private float _gravity = -9.81f; // Гравитация.

        private CharacterController _characterController; // Компонент управления персонажем.
        private Vector3 _velocity; // Скорость падения/гравитации.

        [field: SerializeField]
        public bool CanJump { get; private set; } // Возможность прыжка.
        [field: SerializeField]
        public bool HasJump { get; private set; } // Флаг, указывает на выполнение прыжка.
        [field: SerializeField]
        public bool IsGrounded { get; private set; } // Флаг, указывает, на земле ли персонаж.
        [field: SerializeField]
        public bool Idle { get; private set; } // Флаг, указывает, находится ли персонаж в состоянии покоя.
        [field: SerializeField]
        public bool IsMoving { get; private set; } // Флаг, указывает, движется ли персонаж.
        [field: SerializeField]
        public bool IsSprinting { get; private set; } // Флаг, указывает, выполняется ли спринт.

        public float VelocityY => _velocity.y; // Вертикальная составляющая скорости.

        private Vector3 _desiredMoveDirection; // Сохраняет направление движения для камеры.

        private void Awake()
        {
            _characterController = GetComponent<CharacterController>(); // Инициализация компонента CharacterController.
        }

        private void Update()
        {
            IsGrounded = _groundChecker.IsGrounded; // Проверяем, находится ли персонаж на земле.
            var movement = InputPlayer.Movement;

            if (movement != Vector3.zero)
            {
                HandleMovement(movement);
            }
            else
            {
                HandleIdle();
            }

            if (InputPlayer.Jump && IsGrounded && CanJump)
            {
                PerformJump();
            }

            ApplyGravity();
        }

        private void LateUpdate()
        {
            // Обработка камеры в LateUpdate для устранения "дёрганий".
            RotateTowardsDirection(_desiredMoveDirection);
        }

        // Метод для перемещения вперед (например, для рывков).
        public void MoveForward(Vector3 direction)
        {
            _characterController.Move(direction);
        }

        // Обработка движения персонажа.
        private void HandleMovement(Vector3 movement)
        {
            Idle = false;
            IsSprinting = InputPlayer.Spriting && _stamina.Stamina > 0f;

            var speed = IsSprinting ? _sprintSpeed : _moveSpeed;
            if (IsSprinting)
            {
                IsMoving = false;
                OnSprinting?.Invoke();
            }
            else
            {
                IsMoving = true;
                OnMoving?.Invoke();
            }

            _desiredMoveDirection = (_camera.CameraForward() * movement.z + _camera.CameraRight() * movement.x).normalized;

            _characterController.Move(_desiredMoveDirection * (speed * Time.deltaTime));
        }

        // Обработка состояния покоя.
        private void HandleIdle()
        {
            Idle = true;
            IsSprinting = false;
            IsMoving = false;
        }

        // Поворот персонажа в направлении движения.
        private void RotateTowardsDirection(Vector3 desiredDirection)
        {
            if (desiredDirection != Vector3.zero)
            {
                var toRotation = Quaternion.LookRotation(desiredDirection, Vector3.up);
                _characterController.transform.rotation = Quaternion.RotateTowards(_characterController.transform.rotation, toRotation, _rotationSpeed * Time.deltaTime);
            }
        }

        // Выполнение прыжка.
        private void PerformJump()
        {
            _velocity.y = Mathf.Sqrt(_jumpForce * -2f * _gravity);
            CanJump = false;
            OnJump?.Invoke();
        }

        // Применение гравитации.
        private void ApplyGravity()
        {
            if (IsGrounded && _velocity.y < 0f)
            {
                _velocity.y = -1f; // Устанавливаем небольшую отрицательную скорость, чтобы не зависать на земле.
            }

            _velocity.y += _gravity * Time.deltaTime;
            _characterController.Move(_velocity * Time.deltaTime);

            if (IsGrounded && !CanJump)
            {
                CanJump = true; // Восстанавливаем возможность прыжка при нахождении на земле.
            }
        }
    }
}
