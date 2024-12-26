using UnityEngine;

namespace Player
{
    public class PlayerStamina : MonoBehaviour
    {
        [SerializeField] 
        private float _stamina = 100f; 
        [SerializeField] 
        private float _stationaryRecoveryAmount = 2f; 
        [SerializeField] 
        private float _movingRecoveryAmount = 1f; 
        [SerializeField] 
        private float _recoveryInterval = 1f;

        private PlayerMovement _movement;
        private float _elapsedTime;
        
        public float Stamina => _stamina;

        private void Update()
        {
            RecoverStamina();
        }

        private void RecoverStamina()
        {
            if (_elapsedTime >= _recoveryInterval)
            {
                if (InputPlayer.Movement.magnitude <= 0.2f)
                {
                    IncreaseStamina(_stationaryRecoveryAmount);
                }
                else
                {
                    IncreaseStamina(_movingRecoveryAmount);
                }

                _elapsedTime = 0f;
            }

            _elapsedTime += Time.deltaTime;
        }
        
        public void DecreaseStamina(float amount)
        {
            _stamina -= amount;
            _stamina = Mathf.Clamp(_stamina, 0, 100); 
        }

        public void IncreaseStamina(float amount)
        {
            _stamina += amount;
            _stamina = Mathf.Clamp(_stamina, 0, 100);
        }
    }
}