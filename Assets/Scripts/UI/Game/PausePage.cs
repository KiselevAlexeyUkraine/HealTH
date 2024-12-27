using Services;
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
        private PauseManager _pauseManager;

        private void Awake()
        {
            _continue.onClick.AddListener(() => { _pauseManager.SwitchState(); });
            _restart.onClick.AddListener(() => { SceneSwitcher.Instance.RestartCurrentScene(); });
            _settings.onClick.AddListener(() => { PageSwitcher.Open(PageName.GameSettings); });
            _exit.onClick.AddListener(() => { SceneSwitcher.Instance.LoadScene(0); });
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