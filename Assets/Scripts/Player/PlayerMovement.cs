using System;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMovement : MonoBehaviour
    {
        public Action OnMoving;
        public Action OnSprinting;
        public Action OnJump;

        [SerializeField]
        private PlayerCamera _camera;
        [SerializeField]
        private PlayerStamina _stamina;
        [SerializeField]
        private GroundChecker _groundChecker;

        [SerializeField]
        private float _moveSpeed = 5f;
        [SerializeField]
        private float _sprintSpeed = 8f;
        [SerializeField]
        private float _rotationSpeed = 10f;
        [SerializeField]
        private float _jumpForce = 10f;
        [SerializeField]
        private float _gravity = -9.81f;

        private CharacterController _characterController;
        private Vector3 _velocity;
        private Vector3 _desiredMoveDirection;
        private Vector3 _inputMovement;
        private float _currentSpeed;

        [field: SerializeField]
        public bool CanJump { get; private set; }
        [field: SerializeField]
        public bool IsGrounded { get; private set; }
        [field: SerializeField]
        public bool Idle { get; private set; }
        [field: SerializeField]
        public bool IsMoving { get; private set; }
        [field: SerializeField]
        public bool IsSprinting { get; private set; }

        public float VelocityY => _velocity.y;

        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();
        }

        private void Update()
        {
            IsGrounded = _groundChecker.IsGrounded;

            _inputMovement = InputPlayer.Movement;

            if (_inputMovement != Vector3.zero)
            {
                HandleStates();
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
            _desiredMoveDirection = (_camera.CameraForward() * _inputMovement.z + _camera.CameraRight() * _inputMovement.x).normalized;

            _characterController.Move(_desiredMoveDirection * (_currentSpeed * Time.deltaTime));

            RotateTowardsDirection(_desiredMoveDirection);
        }

        private void HandleStates()
        {
            Idle = false;
            IsSprinting = InputPlayer.Spriting && _stamina.Stamina > 0f;

            _currentSpeed = IsSprinting ? _sprintSpeed : _moveSpeed;

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
        }

        private void HandleIdle()
        {
            Idle = true;
            IsSprinting = false;
            IsMoving = false;
        }

        private void RotateTowardsDirection(Vector3 desiredDirection)
        {
            if (desiredDirection != Vector3.zero)
            {
                var toRotation = Quaternion.LookRotation(desiredDirection, Vector3.up);
                _characterController.transform.rotation = Quaternion.RotateTowards(_characterController.transform.rotation, toRotation, _rotationSpeed * Time.deltaTime);
            }
        }

        private void PerformJump()
        {
            _velocity.y = Mathf.Sqrt(_jumpForce * -2f * _gravity);
            CanJump = false;
            OnJump?.Invoke();
        }

        private void ApplyGravity()
        {
            if (IsGrounded && _velocity.y < 0f)
            {
                _velocity.y = -1f;
            }

            _velocity.y += _gravity * Time.deltaTime;
            _characterController.Move(_velocity * Time.deltaTime);

            if (IsGrounded && !CanJump)
            {
                CanJump = true;
            }
        }

        /// <summary>
        /// Телепортирует игрока или перемещает его мгновенно в указанное направление, даже если он находится в прыжке.
        /// </summary>
        /// <param name="direction">Направление и расстояние для перемещения.</param>
        public void MoveForward(Vector3 direction)
        {
            // Игнорируем гравитацию и скорость, просто перемещаем игрока в указанное место
            _characterController.enabled = false; // Выключаем контроллер для корректного телепорта
            transform.position += direction; // Мгновенно перемещаем игрока
            _characterController.enabled = true; // Включаем контроллер обратно
        }
    }
}
