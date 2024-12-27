using System;
using UnityEngine;

namespace Player
{
    public class PlayerHealth : MonoBehaviour
    {
        public Action OnIncrease;
        public Action OnDecrease;
        
        [SerializeField]
        private PlayerDash _dash;
        [SerializeField] 
        private int _maxHealth = 5;
        [SerializeField]
        private int _health = 5;

        public int MaxHealth => _maxHealth;
        public int Health => _health;
        
        private void Awake()
        {
            _health = _maxHealth;
            _dash.OnDash += DecreaseHealth;
        }
        
        public void IncreaseHealth() // Переделать чтобы Андрей показал как правильно
        {
            _health = Mathf.Clamp(++_health, 0, _maxHealth);
            OnIncrease?.Invoke();
        }
        
        private void DecreaseHealth()
        {
            _health = Mathf.Clamp(--_health, 0, _maxHealth);
            OnDecrease?.Invoke();
        }

        private void OnDestroy()
        {
            _dash.OnDash -= DecreaseHealth;
        }
    }
}