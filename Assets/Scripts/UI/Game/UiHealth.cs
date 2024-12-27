using System.Collections.Generic;
using Player;
using UnityEngine;

namespace UI.Game
{
    public class UiHealth : MonoBehaviour
    {
        [SerializeField]
        private PlayerHealth _health;
        [SerializeField]
        private GameObject _healthContainer;
        [SerializeField]
        private GameObject _healthPointPrefab;

        private List<GameObject> _healthImages;
        
        private void Awake()
        {
            _healthImages = new List<GameObject>();
            
            for (var i = 0; i < _health.MaxHealth; i++)
            {
                var go = Instantiate(_healthPointPrefab, _healthContainer.transform);
                _healthImages.Add(go);
            }

            _health.OnIncrease += () => _healthImages[_health.Health].gameObject.SetActive(true);
            _health.OnDecrease += () => _healthImages[_health.Health].gameObject.SetActive(false);
        }
    }
}