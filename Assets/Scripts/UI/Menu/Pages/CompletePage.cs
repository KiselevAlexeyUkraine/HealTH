using UnityEngine;
using UnityEngine.UI;

namespace UI.Menu.Pages
{
    public class CompletePage : BasePage
    {
        [SerializeField] 
        private Button _continue;
        [SerializeField] 
        private Button _nextLevel;
        [SerializeField] 
        private Button _restart;
        [SerializeField] 
        private Button _menu;

        private void Awake()
        {
            _continue.onClick.AddListener(() => { PageSwitcher.Open(PageName.None); });
            _nextLevel.onClick.AddListener(() => { SceneSwitcher.instance.LoadNextScene(); });
            _restart.onClick.AddListener(() => { SceneSwitcher.instance.LoadScene(SceneSwitcher.instance.CurrentScene); });
            _menu.onClick.AddListener(() => { PageSwitcher.Open(PageName.Menu); });
        }

        private void OnDestroy()
        {
            _continue.onClick.RemoveAllListeners();
            _nextLevel.onClick.RemoveAllListeners();
            _restart.onClick.RemoveAllListeners();
            _menu.onClick.RemoveAllListeners();
        }
    }
}