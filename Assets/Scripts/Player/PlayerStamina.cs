using System;
using UnityEngine;

namespace Player
{
    public class PlayerStamina : MonoBehaviour
    {
        public Action OnIncrease;
        public Action OnDecrease;
        
        [SerializeField]
        private PlayerMovement _movement;

        [SerializeField]
        private float _maxStamina = 100f;
        [SerializeField] 
        private float _stamina = 100f; 
        [SerializeField] 
        private float _stationaryRecoveryAmount = 2f; 
        [SerializeField] 
        private float _movingRecoveryAmount = 1f; 
        [SerializeField] 
        private float _recoveryInterval = 1f;
        [SerializeField]
        private float _sprintingConsumption = 2f;

        private float _elapsedTime;

        public float MaxStamina => _maxStamina;
        public float Stamina => _stamina;

        private void Awake()
        {
            _stamina = _maxStamina;
            
            _movement.OnSprinting += () => DecreaseStamina(_sprintingConsumption * Time.deltaTime);
        }

        private void Update()
        {
            RecoverStamina();
        }

        private void RecoverStamina()
        {
            if (_movement.IsSprinting)
            {
                return;
            }
            
            if (_elapsedTime >= _recoveryInterval)
            {
                if (_movement.IsMoving)
                {
                    IncreaseStamina(_movingRecoveryAmount);
                }
                else
                {
                    IncreaseStamina(_stationaryRecoveryAmount);
                }

                _elapsedTime = 0f;
            }

            _elapsedTime += Time.deltaTime;
        }
        
        
        public void IncreaseStamina(float amount)
        {
            _stamina += amount;
            _stamina = Mathf.Clamp(_stamina, 0, 100);
            OnIncrease?.Invoke();
        }
        
        public void DecreaseStamina(float amount)
        {
            _stamina -= amount;
            _stamina = Mathf.Clamp(_stamina, 0, 100); 
            OnDecrease?.Invoke();
        }
    }
}