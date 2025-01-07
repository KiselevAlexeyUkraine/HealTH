using UnityEngine;

namespace Player
{
    public class PlayerCollector : MonoBehaviour
    {
        [SerializeField] private PlayerStats _stats;

        void OnTriggerEnter(Collider other)
        {
            if (_stats == null)
            {
                Debug.LogError("PlayerStats is not assigned!");
                return;
            }

            switch (other.tag)
            {
                case "Health":
                    _stats.AddScore(ScoreType.Health);
                    Destroy(other.gameObject);
                    break;
                case "Coin":
                    _stats.AddScore(ScoreType.Coin);
                    Destroy(other.gameObject);
                    break;
                case "Key":
                    _stats.AddScore(ScoreType.Key);
                    Destroy(other.gameObject);
                    break;
                default:
                    Debug.LogWarning($"Unhandled tag: {other.tag}");
                    break;
            }
        }
    }
}