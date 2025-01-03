using UnityEngine;

namespace Player
{
    public class GroundChecker : MonoBehaviour
    {
        public Transform _groundCheck; 
        public LayerMask _groundMask; 
        public float _groundCheckRadius = 0.5f; 
        
        public bool IsGrounded => Physics.CheckSphere(_groundCheck.position, _groundCheckRadius, _groundMask);

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(_groundCheck.position, _groundCheckRadius);
        }
    }
}
