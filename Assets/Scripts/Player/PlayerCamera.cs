using UnityEngine;

namespace Player
{
    public class PlayerCamera : MonoBehaviour
    {
        [SerializeField]
        private Camera _camera;

        public Vector3 CameraForward()
        {
            var cameraForward = _camera.transform.forward;
            cameraForward.y = 0; 
            cameraForward.Normalize();

            return cameraForward;
        }

        public Vector3 CameraRight()
        {
            Vector3 right = _camera.transform.right;
            right.y = 0; // Убираем вертикальную составляющую
            right.Normalize(); // Нормализуем вектор

            return right;
        }
    }
}