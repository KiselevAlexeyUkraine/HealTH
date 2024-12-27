using UnityEngine;
using UnityEngine.UI;

namespace UI.Game
{
    public class PausePage : BasePage
    {
        [SerializeField] 
        private Button _continue;
        [SerializeField] 
        private Button _restart;
        [SerializeField] 
        private Button _settings;
        [SerializeField] 
        private Button _exit;

        [SerializeField] 
        private CursorToggle _cursorToggle;

        private void Awake()
        {
            _continue.onClick.AddListener(() => { _cursorToggle.ToggleCursorVisibility(); PageSwitcher.Open(PageName.Stats); });
            _restart.onClick.AddListener(() => { _cursorToggle.ToggleCursorVisibility(); });
            _settings.onClick.AddListener(() => { PageSwitcher.Open(PageName.Settings); });
            _exit.onClick.AddListener(() => { SceneSwitcher.instance.LoadScene(0); });
        }

        private void OnDestroy()
        {
            _continue.onClick.RemoveAllListeners();
            _restart.onClick.RemoveAllListeners();
            _settings.onClick.RemoveAllListeners();
            _exit.onClick.RemoveAllListeners();
        }
    }
}