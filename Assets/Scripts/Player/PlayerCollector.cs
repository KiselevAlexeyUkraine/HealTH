using UnityEngine;

namespace Player
{
    public class PlayerCollector : MonoBehaviour
    {
        [SerializeField] private PlayerStats _playerStats;
    
        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Coin"))
            {
                _playerStats.AddScore((int)ScoreValutes.scoreCoins);
                Destroy(other.gameObject);
            }

            if (other.CompareTag("Key"))
            {
                Destroy(other.gameObject);
            }
        }
    }
}