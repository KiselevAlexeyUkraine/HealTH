using UnityEngine;

namespace Player
{
    public class PlayerAnimation : MonoBehaviour
    {
        [SerializeField]
        private PlayerMovement _movement;
        [SerializeField]
        private PlayerDash _dash;
        [SerializeField]
        private Animator _animator;

        private readonly int _idle = Animator.StringToHash("Idle");
        private readonly int _isMoving = Animator.StringToHash("IsMoving");
        private readonly int _isSprinting = Animator.StringToHash("IsSprinting");
        private readonly int _canJump = Animator.StringToHash("CanJump");
        private readonly int _landing = Animator.StringToHash("Landing");
        private readonly int _hasDash = Animator.StringToHash("HasDash");
        private readonly int _velocityY = Animator.StringToHash("VelocityY");
        private readonly int _isGrounded = Animator.StringToHash("IsGrounded");
        
        private void Update()
        {
            _animator.SetBool(_isMoving, _movement.IsMoving);
            _animator.SetBool(_isSprinting, _movement.IsSprinting);
            _animator.SetBool(_idle, _movement.Idle);
            _animator.SetBool(_canJump, _movement.CanJump);
            _animator.SetFloat(_velocityY, _movement.VelocityY);
            _animator.SetBool(_isGrounded, _movement.IsGrounded);
        }
    }
}