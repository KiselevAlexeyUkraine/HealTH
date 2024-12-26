using System;
using System.Collections.Generic;
using UI;
using UI.Menu;
using UnityEngine;

namespace Cameras
{
    public class MenuCamera : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField]
        private Animator _animator;
        [SerializeField]
        private float _transitionDuration = 0.5f;

        [Header("Page Transitions")]
        [SerializeField]
        private List<Transition> _transitions;

        private void Awake()
        {
            foreach (var transition in _transitions)
            {
                transition.page.OnOpen += () => SetState(transition.animation);
            }
        }

        public void SetState(string name)
        {
            _animator.CrossFade(name, _transitionDuration);
        }
    }

    [Serializable]
    public struct Transition
    {
        public BasePage page;
        public string animation;
    }
}
