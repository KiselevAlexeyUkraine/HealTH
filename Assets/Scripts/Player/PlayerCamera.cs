using UnityEngine;

namespace Player
{
    // Этот класс отвечает за получение направлений "вперед" и "вправо" относительно камеры.
    public class PlayerCamera : MonoBehaviour
    {
        [SerializeField]
        private Camera _camera; // Камера, относительно которой вычисляются направления.

        // Возвращает направление "вперед" относительно камеры, с обнуленной вертикальной составляющей.
        public Vector3 CameraForward()
        {
            Vector3 cameraForward = _camera.transform.forward; // Получаем вектор "вперед" камеры.
            cameraForward.y = 0; // Убираем вертикальную составляющую.
            cameraForward.Normalize(); // Нормализуем вектор, чтобы он имел длину 1.

            return cameraForward;
        }

        // Возвращает направление "вправо" относительно камеры, с обнуленной вертикальной составляющей.
        public Vector3 CameraRight()
        {
            Vector3 right = _camera.transform.right; // Получаем вектор "вправо" камеры.
            right.y = 0; // Убираем вертикальную составляющую.
            right.Normalize(); // Нормализуем вектор, чтобы он имел длину 1.

            return right;
        }
    }
}