using UnityEngine;

namespace Player
{
    // Этот класс управляет анимациями игрока, основываясь на данных из PlayerMovement и PlayerDash.
    public class PlayerAnimation : MonoBehaviour
    {
        [SerializeField]
        private PlayerMovement _movement; // Ссылка на компонент PlayerMovement для получения состояния движения.
        [SerializeField]
        private PlayerDash _dash; // Ссылка на компонент PlayerDash для управления рывком (не используется в текущем коде).
        [SerializeField]
        private Animator _animator; // Аниматор для управления анимациями игрока.

        // Кэшированные хэши параметров анимации для повышения производительности.
        private readonly int _idle = Animator.StringToHash("Idle");
        private readonly int _isMoving = Animator.StringToHash("IsMoving");
        private readonly int _isSprinting = Animator.StringToHash("IsSprinting");
        private readonly int _canJump = Animator.StringToHash("CanJump");
        private readonly int _landing = Animator.StringToHash("Landing");
        private readonly int _hasDash = Animator.StringToHash("HasDash");
        private readonly int _velocityY = Animator.StringToHash("VelocityY");
        private readonly int _isGrounded = Animator.StringToHash("IsGrounded");

        // Обновляет состояния анимации каждый кадр.
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