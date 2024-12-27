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

        [field: SerializeField]
        public bool CanJump { get; private set; }
        [field: SerializeField]
        public bool HasJump { get; private set; }
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
            var movement = InputPlayer.Movement;
            
            if (movement != Vector3.zero)
            {
                Idle = false;
                IsSprinting = InputPlayer.Spriting;
                Move(movement);
            }
            else
            {
                Idle = true;
                IsSprinting = false;
                IsMoving = false;
            }
            
            if (InputPlayer.Jump && IsGrounded && CanJump)
            {
                Jump();
                CanJump = false;
            }
            else if (IsGrounded && !CanJump)
            {
                CanJump = true;
            }

            Gravity();
        }

        public void MoveForward(Vector3 direction)
        {
            _characterController.Move(direction);
        }

        private void Move(Vector3 movement)
        {
            float speed;
            
            if (IsSprinting && _stamina.Stamina > 0f)
            {
                IsMoving = false;
                speed = _sprintSpeed;
                OnSprinting?.Invoke();
            }
            else
            {
                IsMoving = true;
                speed = _moveSpeed;
                OnMoving?.Invoke();
            }
            
            var targetRotation = _camera.GetRotation();
            var toward = Quaternion.Lerp(
                _characterController.transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime
            );
            
            _characterController.transform.rotation = toward;
            _characterController.Move(toward * movement * (speed * Time.deltaTime));
        }

        private void Jump()
        {
            _velocity.y = Mathf.Sqrt(_jumpForce * -2f * _gravity);
        }

        private void Gravity()
        {
            if (_groundChecker.IsGrounded && _velocity.y < 0f)
            {
                _velocity.y = -1f; 
            }

            _velocity.y += _gravity * Time.deltaTime;
            _characterController.Move(_velocity * Time.deltaTime);
        }
    }
}