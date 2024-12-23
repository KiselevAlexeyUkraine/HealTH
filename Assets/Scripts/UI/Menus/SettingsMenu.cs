using UnityEngine;
using UnityEngine.UI;

namespace UI.Menus
{
    public class SettingsMenu : Menu
    {
        [SerializeField] 
        private Button _back;
        [SerializeField] 
        private Slider _volume;

        private void Awake()
        {
            _back.onClick.AddListener(() => { MenuSwitcher.instance.OpenMenu(MenuNames.ContinuationGame); });
            _volume.onValueChanged.AddListener((value) => { });
        }

        private void OnDestroy()
        {
            _back.onClick.RemoveAllListeners();
            _volume.onValueChanged.RemoveAllListeners();
        }
    }
}