using UnityEngine;
using UnityEngine.UI;

namespace UI.Menus
{
    public class ResultGameMenu : Menu
    {
        [SerializeField] 
        private Button _continuationGame;
        [SerializeField] 
        private Button _nextLevel;
        [SerializeField] 
        private Button _restart;
        [SerializeField] 
        private Button _settings;
        [SerializeField] 
        private Button _backMainMenu;
        [SerializeField] 
        private Button _exit;

        private void Awake()
        {
            _continuationGame.onClick.AddListener(() => { MenuSwitcher.instance.OpenMenu(MenuNames.None); });
            _nextLevel.onClick.AddListener(() => { SceneSwitcher.instance.LoadNextScene(); });
            _restart.onClick.AddListener(() => { SceneSwitcher.instance.LoadScene(SceneSwitcher.instance.CurrentScene); });
            _settings.onClick.AddListener(() => { MenuSwitcher.instance.OpenMenu(MenuNames.Settings); });
            _backMainMenu.onClick.AddListener(() => { MenuSwitcher.instance.OpenMenu(MenuNames.MainMenu); });
            _exit.onClick.AddListener(() => { SceneSwitcher.instance.LoadScene(0); });
        }

        private void OnDestroy()
        {
            _continuationGame.onClick.RemoveAllListeners();
            _nextLevel.onClick.RemoveAllListeners();
            _restart.onClick.RemoveAllListeners();
            _settings.onClick.RemoveAllListeners();
            _backMainMenu.onClick.RemoveAllListeners();
            _exit.onClick.RemoveAllListeners();
        }
    }
}