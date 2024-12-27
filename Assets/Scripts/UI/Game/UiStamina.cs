using Player;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Game
{
    public class UiStamina : MonoBehaviour
    {
        [SerializeField]
        private PlayerStamina _stamina;
        [SerializeField]
        private Image _staminaFill;
        
        private void Awake()
        {
            _stamina.OnIncrease += () => _staminaFill.fillAmount = _stamina.Stamina / _stamina.MaxStamina;
            _stamina.OnDecrease += () => _staminaFill.fillAmount = _stamina.Stamina / _stamina.MaxStamina;
        }
    }
}