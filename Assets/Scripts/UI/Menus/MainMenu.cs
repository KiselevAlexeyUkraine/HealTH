using UnityEngine;
using UnityEngine.UI;

namespace UI.Menus
{
    public class MainMenu : Menu
    {
        [SerializeField] 
        private Button _startGame;
        [SerializeField] 
        private Button _settings;
        [SerializeField] 
        private Button _authors;
        [SerializeField] 
        private Button _exit;

        private void Awake()
        {
            _startGame.onClick.AddListener(() => { });
            _settings.onClick.AddListener(() => { MenuSwitcher.instance.OpenMenu((MenuNames.Settings));});
            _authors.onClick.AddListener(() => { MenuSwitcher.instance.OpenMenu(MenuNames.Authors);});
            _exit.onClick.AddListener(() => { SceneSwitcher.instance.LoadScene(0); });
        }

        private void OnDestroy()
        {
            _startGame.onClick.RemoveAllListeners();
            _settings.onClick.RemoveAllListeners();
            _authors.onClick.RemoveAllListeners();
            _exit.onClick.RemoveAllListeners();
        }
    }
}