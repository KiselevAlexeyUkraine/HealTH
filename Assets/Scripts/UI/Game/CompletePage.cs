using UnityEngine;
using UnityEngine.UI;

namespace UI.Game
{
    public class CompletePage : BasePage
    {
        [SerializeField] 
        private Button _nextLevel;
        [SerializeField] 
        private Button _restart;
        [SerializeField] 
        private Button _exit;

        private void Awake()
        {
            _nextLevel.onClick.AddListener(() => { SceneSwitcher.Instance.LoadNextScene(); });
            _restart.onClick.AddListener(() => { SceneSwitcher.Instance.LoadScene(SceneSwitcher.Instance.CurrentScene); });
            _exit.onClick.AddListener(() => { PageSwitcher.Open(PageName.Menu); });
        }

        private void OnDestroy()
        {
            _nextLevel.onClick.RemoveAllListeners();
            _restart.onClick.RemoveAllListeners();
            _exit.onClick.RemoveAllListeners();
        }
    }
}