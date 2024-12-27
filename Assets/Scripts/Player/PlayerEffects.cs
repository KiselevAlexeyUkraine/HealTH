using UnityEngine;

namespace Player
{
    public class PlayerEffects : MonoBehaviour
    {
        [SerializeField]
        private PlayerMovement _movement;
        [SerializeField]
        private ParticleSystem _sprintParticles;

        private void Update()
        {
            if (_movement.IsSprinting && !_movement.IsMoving && !_sprintParticles.isPlaying)
            {
                _sprintParticles.Play();
            }
            else if ((_movement.IsMoving || _movement.Idle) && _sprintParticles.isPlaying)
            {
                _sprintParticles.Stop();
            }
        }
    }
}