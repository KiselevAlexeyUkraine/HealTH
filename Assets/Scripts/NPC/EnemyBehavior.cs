using Player;
using System;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehavior : MonoBehaviour
{
    public Action EnemyAttack;
    private PlayerHealth playerStats; // Ссылка на скрипт PlayerStats
    [SerializeField] private Transform[] patrolPoints; // Точки для патрулирования
    [SerializeField] private float patrolSpeed = 2f;
    [SerializeField] private float chaseSpeed = 5f;
    [SerializeField] private float waitTimeAtPatrolPoint = 2f; // Время ожидания на точке патрулирования
    [SerializeField] private float attackRadius = 1.5f; // Радиус атаки
    private int damage = 1;
    [SerializeField] private float detectionRadius = 8f; // Радиус обнаружения игрока

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
    private bool isReturningToPatrol; // Новое состояние для возвращения к патрулированию
    private float waitTimer;
    private bool isWaiting;

    // Новые переменные для отслеживания времени
    [SerializeField] private float chaseDuration = 5f; // Время преследования
    private float chaseTimer = 0f; // Таймер для отслеживания времени преследования

    // Переменная для отслеживания состояния невидимости игрока
    private bool isPlayerInvisible = false;

    void Start()
    {
        playerStats = GameObject.Find("Character").GetComponent<PlayerHealth>();
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
        // Проверяем расстояние до игрока
        float distanceToPlayer = player != null ? Vector3.Distance(transform.position, player.position) : Mathf.Infinity;

        if (isChasing)
        {
            chaseTimer += Time.deltaTime; // Увеличиваем таймер преследования

            if (chaseTimer >= chaseDuration)
            {
                isChasing = false; // Останавливаем преследование
                isReturningToPatrol = true; // Устанавливаем состояние возвращения к патрулированию
                agent.isStopped = true; // Останавливаем агента
                Debug.Log("Не Бежим");
                return; // Выходим из метода
            }

            if (distanceToPlayer <= attackRadius)
            {
                AttackPlayer();
            }
            else
            {
                ChasePlayer();
            }
        }
        else if (isReturningToPatrol)
        {
            // Если игрок находится в радиусе обнаружения, начинаем преследование
            if (distanceToPlayer <= detectionRadius && !isPlayerInvisible)
            {
                isChasing = true; // Возобновляем преследование
                chaseTimer = 0f; // Сбрасываем таймер преследования
                isReturningToPatrol = false; // Сбрасываем состояние возвращения
                return; // Выходим из метода
            }

            // Если враг возвращается к патрульной точке, не реагируем на игрока
            if (Vector3.Distance(transform.position, patrolPoints[currentPatrolIndex].position) < 0.5f)
            {
                isReturningToPatrol = false; // Сбрасываем состояние возвращения
                MoveToNextPatrolPoint(); // Возвращаемся к патрулированию
            }
        }
        else
        {
            // Проверяем, находится ли игрок в радиусе обнаружения
            if (distanceToPlayer <= detectionRadius && !isPlayerInvisible)
            {
                isChasing = true; // Начинаем преследование
                chaseTimer = 0f; // Сбрасываем таймер преследования
            }
            else
            {
                Patrol();
            }
        }

        // Проверка на атаку, даже если игрок невидим
        if (distanceToPlayer <= attackRadius && player != null && !isPlayerInvisible)
        {
            AttackPlayer();
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

        if (audioSource != null && patrolClip != null && !audioSource.isPlaying)
        {
            audioSource.clip = patrolClip;
            audioSource.loop = true;
            audioSource.Play(); // Включаем звук патрулирования
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
            audioSource.Play(); // Включаем звук преследования
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
            Quaternion lookRotation = Quaternion.LookRotation(directionToPlayer); // Исправлено: инициализация переменной
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

    // Этот метод будет вызываться в анимации атаки
    public void AttackPlayerAnimationEvent()
    {
        EnemyAttack?.Invoke();
        audioSource.Play(); // Включаем звук атаки
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player detected - attack!");
            player = other.transform;
            // Проверяем, видим ли игрок
            if (!isPlayerInvisible && !isChasing && !isReturningToPatrol)
            {
                isChasing = true; // Начинаем преследование
                chaseTimer = 0f; // Сбрасываем таймер преследования при обнаружении игрока
            }
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
                audioSource.Play(); // Включаем звук патрулирования
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

    // Новые методы для обработки невидимости игрока
    public void OnPlayerInvisible()
    {
        Debug.Log("Player is now invisible. Stopping chase and canceling attack.");
        isPlayerInvisible = true; // Устанавливаем состояние невидимости
        isChasing = false; // Останавливаем преследование
        player = null; // Убираем ссылку на игрока
        isAttacking = false; // Отменяем атаку
        agent.isStopped = false; // Возвращаемся к патрулированию
        MoveToNextPatrolPoint(); // Возвращаемся к патрулированию

        if (animator != null)
        {
            animator.SetBool("IsAttacking", false); // Сбрасываем анимацию атаки
            animator.SetBool("IsChasing", false); // Сбрасываем анимацию преследования
        }
    }

    public void OnPlayerVisible()
    {
        Debug.Log("Player is now visible. Resuming chase.");
        isPlayerInvisible = false; // Сбрасываем состояние невидимости
        // Здесь можно добавить логику для возобновления преследования, если это необходимо
        if (player != null && Vector3.Distance(transform.position, player.position) <= detectionRadius)
        {
            isChasing = true; // Возобновляем преследование
            chaseTimer = 0f; // Сбрасываем таймер преследования
        }
    }
}