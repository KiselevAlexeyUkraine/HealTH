using System;
using UnityEngine;

namespace Player
{
    public class PlayerStats : MonoBehaviour
    {
        public void AddScore(ScoreType type)
        {
            switch (type)
            {
                case ScoreType.Health:
                    break;
                case ScoreType.Coin:
                    break;
                case ScoreType.Key:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }
}