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
        }

        private void Start()
        {
            foreach (var page in _pages)
            {
                if (page.pageName != _startPage)
                {
                    continue;
                }
                
                _currentPage = page;
                _currentPage.Open();
            }
        }

        public void Open(PageName pageName)
        {
            foreach (var page in _pages)
            {
                if (page.pageName != pageName)
                {
                    continue;
                }
                
                _currentPage.Close();
                _currentPage = page;
                _currentPage.Open();
            }
        }
    }
}
