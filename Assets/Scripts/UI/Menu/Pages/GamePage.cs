using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Menu.Pages
{
    public class GamePage : BasePage
    {
        [SerializeField]
        private ScrollRect _scrollRect;
        [SerializeField]
        private Button _back;

        private List<Button> _levels;
        
        private void Awake()
        {
            _levels.AddRange(_scrollRect.content.GetComponentsInChildren<Button>(true));

            for (var i = 0; i < _levels.Count; i++)
            {
                var index = i + 1;
                _levels[i].onClick.AddListener(() => SceneSwitcher.instance.LoadScene(index));
            }
            
            _back.onClick.AddListener(() => { PageSwitcher.Open(PageName.None); });
        }

        private void OnDestroy()
        {
            foreach (var button in _levels)
            {
                button.onClick.RemoveAllListeners();
            }
            
            _back.onClick.RemoveAllListeners();
        }
    }
}