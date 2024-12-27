using System;
using Player;
using UnityEngine;

namespace Services
{
    public class PauseManager : MonoBehaviour
    {
        public static PauseManager Instance;
        
        private CursorToggle _cursorToggle;
        
        public bool IsPaused { get; private set; }
        
        private void Awake()
        {
            if (Instance != null)
            {
                return;
            }
            
            Instance = this;
            _cursorToggle = new CursorToggle();
        }

        private void Start()
        {
            Play();
        }

        private void Update()
        {
            if (InputPlayer.Pause)
            {
                SwitchState();
            }
        }

        public void Pause()
        {
            Time.timeScale = 0f;
            _cursorToggle.Enable();
        }

        public void Play()
        {
            Time.timeScale = 1;
            _cursorToggle.Disable();
        }

        public void SwitchState()
        {
            if (IsPaused)
            {
                Pause();
            }
            else
            {
                Play();  
            }
        
            IsPaused = !IsPaused;
        }
    }
}
