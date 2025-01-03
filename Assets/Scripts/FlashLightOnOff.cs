using UnityEngine;
using Player;

namespace FlashLight
{
    public class FlashLightOnOff : MonoBehaviour
    {
        [SerializeField]
        private PlayerHealth playerHealth;

        private Animator animator;

        private void Start()
        {
            animator = GetComponent<Animator>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                if (playerHealth.Health >= 2)
                {
                    animator.SetInteger("FlashLight", 1);
                }
                else
                {
                    animator.SetInteger("FlashLight", 2);
                }
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                animator.SetInteger("FlashLight", 0);
            }
        }
    }
}
