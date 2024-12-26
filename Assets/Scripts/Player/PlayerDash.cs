using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Player
{
    public class PlayerDash : MonoBehaviour
    {
        [SerializeField]
        private PlayerMovement _movement;
        [SerializeField] 
        private float _dashDistance = 4f; 
        [SerializeField] 
        private float _dashDuration = 0.5f; 
        [SerializeField] 
        private float _dashCooldown = 1f; 

        private bool _canDash = true;
        private float _elapsedTime;

        private void Update()
        {
            if (InputPlayer.Dash && _canDash)
            {
                Dash().Forget();
            }
        }

        private async UniTaskVoid Dash()
        {
            _canDash = false;
            _elapsedTime = 0f;
            
            var forward = _movement.transform.forward;
            var startPosition = _movement.transform.position;
            var endPosition = startPosition + forward * _dashDistance;
            
            while (_elapsedTime < _dashDuration)
            {
                _elapsedTime += Time.deltaTime;
                
                var position = Vector3.Lerp(startPosition, endPosition, _elapsedTime / _dashDuration);
                var direction = position - _movement.transform.position;
                
                _movement.MoveForward(direction);

                await UniTask.NextFrame();
            }

            await UniTask.WaitForSeconds(_dashCooldown);
            
            _canDash = true;
        }
    }
}