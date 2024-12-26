using UnityEngine;
using UnityEngine.UI;

namespace UI.Game
{
    public class PausePage : BasePage
    {
        [SerializeField] 
        private Button _continuationGame;
        [SerializeField] 
        private Button _settings;
        [SerializeField] 
        private Button _backMainMenu;
        [SerializeField] 
        private Button _exit;

        [SerializeField] 
        private CursorToggle _cursorToggle;

        private void Awake()
        {
            _continuationGame.onClick.AddListener(() => { _cursorToggle.ToggleCursorVisibility(); });
            _settings.onClick.AddListener(() => { PageSwitcher.Open((PageName.Settings));});
            _backMainMenu.onClick.AddListener(() => { PageSwitcher.Open(PageName.Menu);});
            _exit.onClick.AddListener(() => { SceneSwitcher.instance.LoadScene(0); });
        }

        private void OnDestroy()
        {
            _continuationGame.onClick.RemoveAllListeners();
            _settings.onClick.RemoveAllListeners();
            _backMainMenu.onClick.RemoveAllListeners();
            _exit.onClick.RemoveAllListeners();
        }
    }
}