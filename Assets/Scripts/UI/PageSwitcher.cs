using System;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class PageSwitcher : MonoBehaviour
    {
        [SerializeField]
        private PageName _startPage;
        [SerializeField] 
        private List<BasePage> _pages = new();

        private BasePage _currentPage;

        private void Awake()
        {
            foreach (var page in _pages)
            {
                page.PageSwitcher = this;
            }
            
            foreach (var page in _pages)
            {
                if (page.pageName != _startPage)
                {
                    page.Close().Forget();
                    continue;
                }
                
                _currentPage = page;
                _currentPage.Open().Forget();
            }
        }

        public void Open(PageName pageName)
        {
            for (var i = 0; i < _pages.Count; i++)
            {
                if (_pages[i].pageName == pageName)
                {
                    _currentPage.Close().Forget();
                    _currentPage = _pages[i];
                    _currentPage.Open().Forget();
                    return;
                }
            }
        }
    }
}
