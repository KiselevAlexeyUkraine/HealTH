using UnityEngine;

namespace Player
{
    public class PlayerCamera : MonoBehaviour
    {
        [SerializeField]
        private Camera _camera;

        public Quaternion GetRotation()
        {
            var cameraForward = _camera.transform.forward;
            cameraForward.y = 0; 
            cameraForward.Normalize(); 

            return Quaternion.LookRotation(cameraForward, Vector3.up);
        }
    }
}