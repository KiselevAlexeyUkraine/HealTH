using UnityEngine;

namespace Player
{
    // Статический класс для обработки ввода игрока.
    public static class InputPlayer
    {
        // Возвращает нормализованный вектор движения на основе ввода по осям.
        public static Vector3 Movement => new Vector3(Horizontal, 0.0f, Vertical).normalized;

        // Возвращает true, если удерживается клавиша бега (Shift).
        public static bool Spriting => Input.GetKey(KeyCode.LeftShift);

        // Возвращает true, если была нажата клавиша прыжка (Space).
        public static bool Jump => Input.GetKeyDown(KeyCode.Space);

        // Возвращает true, если была нажата клавиша рывка (Alt).
        public static bool Dash => Input.GetKeyDown(KeyCode.LeftAlt);

        // Возвращает true, если была нажата клавиша паузы (Escape).
        public static bool Pause => Input.GetKeyDown(KeyCode.Escape);

        // Возвращает значение ввода по горизонтальной оси.
        private static float Horizontal => Input.GetAxisRaw("Horizontal");

        // Возвращает значение ввода по вертикальной оси.
        private static float Vertical => Input.GetAxisRaw("Vertical");
    }
}