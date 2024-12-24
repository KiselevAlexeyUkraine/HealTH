using UnityEngine;
using UnityEngine.UI;

namespace UI.Menu.Pages
{
    public class AuthorsPage : BasePage
    {
        [SerializeField] 
        private Button _back;

        private void Awake()
        {
            _back.onClick.AddListener(() => { PageSwitcher.Open(PageName.Menu); });
        }

        private void OnDestroy()
        {
            _back.onClick.RemoveAllListeners();
        }
    }
}