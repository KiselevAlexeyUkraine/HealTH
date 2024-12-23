using UnityEngine;
using UnityEngine.UI;

namespace UI.Menus
{
    public class AuthorsMenu : Menu
    {
        [SerializeField] 
        private Button _back;

        private void Awake()
        {
            _back.onClick.AddListener(() => { MenuSwitcher.instance.OpenMenu(MenuNames.GameMenu); });
        }

        private void OnDestroy()
        {
            _back.onClick.RemoveAllListeners();
        }
    }
}