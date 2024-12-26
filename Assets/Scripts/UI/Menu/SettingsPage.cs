using UnityEngine;
using UnityEngine.UI;

namespace UI.Menu
{
    public class SettingsPage : BasePage
    {
        [SerializeField] 
        private Button _back;
        [SerializeField] 
        private Slider _volume;
        
        private void Awake()
        {
            _back.onClick.AddListener(() => { PageSwitcher.Open(PageName.Menu); });
            _volume.onValueChanged.AddListener((value) => { });
        }

        private void OnDestroy()
        {
            _back.onClick.RemoveAllListeners();
            _volume.onValueChanged.RemoveAllListeners();
        }
    }
}