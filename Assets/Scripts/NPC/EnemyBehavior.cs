using UnityEngine;
using UnityEngine.AI;

public class EnemyBehavior : MonoBehaviour
{
    [SerializeField] private PlayerStats playerStats; // Ссылка на скрипт PlayerStats
    [SerializeField] private Transform[] patrolPoints; // Точки для патрулирования
    [SerializeField] private float patrolSpeed = 2f;
    [SerializeField] private float chaseSpeed = 5f;
    [SerializeField] private float waitTimeAtPatrolPoint = 2f; // Время ожидания на точке патрулирования
    [SerializeField] private float attackRadius = 1.5f; // Радиус атаки
    [SerializeField] private float damage = 10f;
    [SerializeField] private float detectionRadius = 8f; // Радиус обнаружения игрока
    [SerializeField] private float playerHeightThreshold = 1f; // Пороговое значение по Y для отмены преследования
    [SerializeField] private float timeAboveThreshold = 5f; // Время, в течение которого игрок должен оставаться выше порога

    [SerializeField] private AudioClip patrolClip; // Звук для патрулирования
    [SerializeField] private AudioClip chaseClip; // Звук для преследования
    [SerializeField] private AudioClip attackClip; // Звук для атаки

    private Animator animator; // Аниматор для управления анимациями
    private AudioSource audioSource; // Источник звука
    private NavMeshAgent agent;
    private Transform player;
    private int currentPatrolIndex;
    private bool isChasing;
    private bool isAttacking;
    private float waitTimer;
    private bool isWaiting;

    // Новые переменные для отслеживания времени
    private float timeAboveThresholdCounter = 0f; // Таймер для отслеживания времени выше порога

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        if (patrolPoints.Length > 0)
        {
            currentPatrolIndex = 0;
            MoveToNextPatrolPoint();
        }
        else
        {
            Debug.LogWarning("No patrol points assigned to enemy.");
        }

        if (audioSource != null && patrolClip != null)
        {
            audioSource.clip = patrolClip;
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    void Update()
    {
        // Проверяем, находится ли игрок выше порогового значения по Y
        if (player != null)
        {
            if (player.position.y > playerHeightThreshold)
            {
                // Увеличиваем таймер, если игрок выше порога
                timeAboveThresholdCounter += Time.deltaTime;

                // Проверяем, превышает ли таймер заданное время
                if (timeAboveThresholdCounter >= timeAboveThreshold)
                {
                    isChasing = false; // Отменяем преследование
                    MoveToNextPatrolPoint(); // Возвращаемся к патрулированию
                    return; // Выходим из метода
                }
            }
            else
            {
                // Сбрасываем таймер, если игрок ниже порога
                timeAboveThresholdCounter = 0f;
            }
        }

        if (isChasing)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            if (distanceToPlayer <= attackRadius)
            {
                AttackPlayer();
            }
            else
            {
                ChasePlayer();
            }
        }
        else
        {
            Patrol();
        }
    }

    private void Patrol()
    {
        agent.speed = patrolSpeed;

        if (isWaiting)
        {
            waitTimer += Time.deltaTime;
            if (waitTimer >= waitTimeAtPatrolPoint)
            {
                isWaiting = false;
                MoveToNextPatrolPoint();
            }
        }
        else if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            isWaiting = true;
            waitTimer = 0f;
            animator.SetBool("IsIdle", true); // Устанавливаем булевое значение для ожидания
        }

        if (animator != null)
        {
            animator.SetBool("IsWalking", !isWaiting);
        }
    }

    private void MoveToNextPatrolPoint()
    {
        if (patrolPoints.Length == 0) return;

        agent.destination = patrolPoints[currentPatrolIndex].position;
        currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;

        if (animator != null)
        {
            animator.SetBool("IsWalking", true);
            animator.SetBool("IsIdle", false); // Сбрасываем булевое значение ожидания
        }
    }

    private void ChasePlayer()
    {
        isAttacking = false;
        agent.speed = chaseSpeed;
        agent.isStopped = false;

        if (player != null)
        {
            agent.SetDestination(player.position);
        }

        if (animator != null)
        {
            animator.SetBool("IsWalking", false);
            animator.SetBool("IsChasing", true);
            animator.SetBool("IsAttacking", false);
            animator.SetBool("IsIdle", false); // Сбрасываем булевое значение ожидания
        }

        if (audioSource != null && chaseClip != null && audioSource.clip != chaseClip)
        {
            audioSource.clip = chaseClip;
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Car"))
        {
            // Останавливаем преследование игрока
            isChasing = false;
            Debug.Log("Мы не бежим за игроком");

            // Возвращаемся к патрулированию
            MoveToNextPatrolPoint();
        }
    }

    private void AttackPlayer()
    {
        isAttacking = true;
        agent.isStopped = true;

        if (player != null)
        {
            // Поворачиваем врага в сторону игрока
            Vector3 directionToPlayer = (player.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(directionToPlayer);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f); // Плавный поворот
        }

        if (animator != null)
        {
            animator.SetBool("IsChasing", false);
            animator.SetBool("IsAttacking", true);
            animator.SetBool("IsIdle", false); // Сбрасываем булевое значение ожидания
        }

        if (audioSource != null && attackClip != null && audioSource.clip != attackClip)
        {
            audioSource.clip = attackClip;
            audioSource.loop = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player detected - attack!");
            player = other.transform;
            isChasing = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player out of range, resume patrol.");
            player = null;
            isChasing = false;
            isAttacking = false;
            agent.isStopped = false;
            MoveToNextPatrolPoint();

            if (animator != null)
            {
                animator.SetBool("IsChasing", false);
                animator.SetBool("IsAttacking", false);
                animator.SetBool("IsIdle", false); // Сбрасываем булевое значение ожидания
            }

            if (audioSource != null && patrolClip != null && audioSource.clip != patrolClip)
            {
                audioSource.clip = patrolClip;
                audioSource.loop = true;
                audioSource.Play();
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }

    public void AttackPlayerAnimationEvent()
    {
        playerStats.DecreaseMotivationForEnemy(damage);
        audioSource.Play();
    }
}