using UnityEngine;
using UnityEngine.UI;

namespace UI.Game
{
    public class FailedPage : BasePage
    {
        [SerializeField] 
        private Button _restart;
        [SerializeField] 
        private Button _menu;

        private void Awake()
        {
            _restart.onClick.AddListener(() => { SceneSwitcher.instance.LoadScene(SceneSwitcher.instance.CurrentScene); });
            _menu.onClick.AddListener(() => { PageSwitcher.Open(PageName.Menu);});
        }

        private void OnDestroy()
        {
            _restart.onClick.RemoveAllListeners();
            _menu.onClick.RemoveAllListeners();
        }
    }
}