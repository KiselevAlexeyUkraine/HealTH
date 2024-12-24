using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace UI.Menu
{
    public class BasePage : MonoBehaviour
    {
        public Action OnOpen;
        public Action OnClose;
        public Action Opened;
        public Action Closed;
        
        public PageName pageName;

        [SerializeField]
        private CanvasGroup _group;
        
        public PageSwitcher PageSwitcher { protected get; set; }

        private void Awake()
        {
            _group.alpha = 0f;
        }
        
        public void Open()
        {
            OnOpen?.Invoke();
            gameObject.SetActive(true);
            Fade(0f, 1f, 0.2f, Opened).Forget();
        }

        public void Close()
        {
            OnClose?.Invoke();
            Fade(1f, 0f, 0.2f, Closed).Forget();
            gameObject.SetActive(false);
        }

        private async UniTask Fade(float start, float end, float duration, Action callback)
        {
            var elapsed = 0f;
        
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                _group.alpha = Mathf.Lerp(start, end, elapsed / duration);
                await UniTask.Yield();
            }
            
            callback?.Invoke();
        }
    }
}
