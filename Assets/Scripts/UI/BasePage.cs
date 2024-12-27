using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace UI
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
        
        public async UniTaskVoid Open()
        {
            OnOpen?.Invoke();
            gameObject.SetActive(true);
            await Fade(0f, 1f, 0.2f);
            Opened?.Invoke();
        }

        public async UniTaskVoid Close()
        {
            OnClose?.Invoke();
            await Fade(1f, 0f, 0.2f);
            gameObject.SetActive(false);
            Closed?.Invoke();
        }

        private async UniTask Fade(float start, float end, float duration)
        {
            var elapsed = 0f;
        
            while (elapsed <= duration)
            {
                elapsed += Time.unscaledDeltaTime;
                _group.alpha = Mathf.Lerp(start, end, elapsed / duration);
                await UniTask.NextFrame();
            }
        }
    }
}
