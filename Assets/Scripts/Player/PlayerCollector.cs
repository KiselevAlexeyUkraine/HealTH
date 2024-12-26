using UnityEngine;

namespace Player
{
    public class PlayerCollector : MonoBehaviour
    {
        [SerializeField] private PlayerStats _stats;
    
        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Health"))
            {
                _stats.AddScore(ScoreType.Health);
                Destroy(other.gameObject);
            }
            
            if (other.CompareTag("Coin"))
            {
                _stats.AddScore(ScoreType.Coin);
                Destroy(other.gameObject);
            }

            if (other.CompareTag("Key"))
            {
                _stats.AddScore(ScoreType.Key);
                Destroy(other.gameObject);
            }
        }
    }
}