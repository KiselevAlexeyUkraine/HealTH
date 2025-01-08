using UnityEngine;

namespace Player
{
    // Класс отвечает за управление визуальными эффектами игрока, такими как отображение частиц при спринте.
    public class PlayerEffects : MonoBehaviour
    {
        [SerializeField]
        private PlayerMovement _movement; // Ссылка на скрипт управления движением игрока.
        [SerializeField]
        private ParticleSystem _sprintParticles; // Частицы, которые активируются при спринте.

        private void Update()
        {
            // Проверяем, нужно ли запустить или остановить частицы.
            if (ShouldPlayParticles())
            {
                _sprintParticles.Play();
            }
            else if (ShouldStopParticles())
            {
                _sprintParticles.Stop();
            }
        }

        // Метод определяет, нужно ли включить частицы спринта.
        private bool ShouldPlayParticles()
        {
            return _movement.IsSprinting && !_movement.IsMoving && !_sprintParticles.isPlaying;
        }

        // Метод определяет, нужно ли выключить частицы спринта.
        private bool ShouldStopParticles()
        {
            return (_movement.IsMoving || _movement.Idle) && _sprintParticles.isPlaying;
        }
    }
}
