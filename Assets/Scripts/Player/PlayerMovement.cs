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

        public bool HasJumped { get; private set; }
        public bool IsMoving { get; private set; }
        public bool IsSprinting { get; private set; }

        private void Start()
        {
            _characterController = GetComponent<CharacterController>();
        }

        private void Update()
        {
            var movement = InputPlayer.Movement;
            
            if (movement != Vector3.zero)
            {
                var isSprinting = InputPlayer.Spriting;
                Move(movement, isSprinting);
            }
            
            if (InputPlayer.Jump && _groundChecker.IsGrounded)
            {
                Jump();
            }

            Gravity();
        }

        public void MoveForward(Vector3 direction)
        {
            _characterController.Move(direction);
        }

        private void Move(Vector3 movement, bool isSprinting)
        {
            var speed = isSprinting ? _sprintSpeed : _moveSpeed;
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
            HasJumped = true;
        }

        private void Gravity()
        {
            if (_groundChecker.IsGrounded && _velocity.y < 0f)
            {
                _velocity.y = -2f; 
            }

            _velocity.y += _gravity * Time.deltaTime;
            _characterController.Move(_velocity * Time.deltaTime);
        }
    }
}