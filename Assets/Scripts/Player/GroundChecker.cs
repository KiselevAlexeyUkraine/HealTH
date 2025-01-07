using UnityEngine;

namespace Player
{
    // Этот класс проверяет, находится ли объект на земле, используя заданную точку проверки.
    public class GroundChecker : MonoBehaviour
    {
        [SerializeField] private Transform _groundCheck; // Трансформ, указывающий точку проверки на земле.
        [SerializeField] private LayerMask _groundMask; // Слой, определяющий, что считается землей.
        [SerializeField] private float _groundCheckRadius = 0.5f; // Радиус проверки соприкосновения с землей.

        private void Awake()
        {
            if (_groundCheck == null)
            {
                Debug.LogError("GroundCheck Transform is not assigned!");
            }
        }

        // Свойство возвращает true, если объект касается слоя "земля".
        public bool IsGrounded => Physics.CheckSphere(_groundCheck.position, _groundCheckRadius, _groundMask);

        // Визуализирует область проверки в редакторе Unity.
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue; // Устанавливает синий цвет для визуализации.
            Gizmos.DrawSphere(_groundCheck.position, _groundCheckRadius); // Рисует сферу проверки.
        }
    }
}