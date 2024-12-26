using UnityEngine;

namespace Player
{
    public static class InputPlayer
    {
        public static Vector3 Movement => new Vector3(Horizontal, 0.0f, Vertical).normalized;
        public static bool Spriting => Input.GetKey(KeyCode.LeftShift);
        public static bool Jump => Input.GetKeyDown(KeyCode.Space);
        public static bool Dash => Input.GetKeyDown(KeyCode.LeftAlt);
        public static bool Pause => Input.GetKeyDown(KeyCode.Escape);

        private static float Horizontal => Input.GetAxisRaw("Horizontal");
        private static float Vertical => Input.GetAxisRaw("Vertical");
    }
}