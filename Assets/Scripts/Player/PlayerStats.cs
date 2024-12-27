using System;
using TMPro;
using UnityEngine;

namespace Player
{
    public class PlayerStats : MonoBehaviour
    {
        private PlayerHealth _health;

        [SerializeField] private TextMeshProUGUI _textMeshProCoin;
        [SerializeField] private TextMeshProUGUI _textMeshProKeys;
        [SerializeField]
        private int coin = 0;
        [SerializeField]
        private int key = 0;

        private void Awake()
        {
            _health = GetComponent<PlayerHealth>();
        }

        public void AddScore(ScoreType type)
        {
            switch (type)
            {
                case ScoreType.Health:
                    _health.IncreaseHealth();
                    break;
                case ScoreType.Coin:
                    coin++;
                    break;
                case ScoreType.Key:
                    key++;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
            VieScore();
        }

        public void DecreaseKeys()
        {
            key--;
            VieScore();
        }

        private void VieScore()
        {
            _textMeshProCoin.text = coin.ToString();
            _textMeshProKeys.text = key.ToString();
        }
    }
}