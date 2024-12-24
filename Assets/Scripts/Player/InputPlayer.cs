using UnityEngine;

namespace Player
{
    public class InputPlayer
    {
        public Vector3 Movement => new Vector3(Horizontal, 0.0f, Vertical).normalized;
        public bool Spriting => Input.GetKey(KeyCode.LeftShift);
        public bool Jump => Input.GetKey(KeyCode.Space);
        public bool Pause => Input.GetKeyDown(KeyCode.Escape);
        public float Horizontal => Input.GetAxis("Horizontal");
        public float Vertical => Input.GetAxis("Vertical");
    }
}