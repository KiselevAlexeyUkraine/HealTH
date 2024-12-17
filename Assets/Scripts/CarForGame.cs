using UnityEngine;
using UnityEngine.AI;

public class CarForGame : MonoBehaviour
{
    [SerializeField] private float motivationLoss = 10f; // Сколько мотивации теряет игрок
    [SerializeField] private Transform[] patrolPoints; // Массив точек патрулирования
    [SerializeField] private float waitTimeAtPoint = 2f; // Время ожидания на каждой точке
    [SerializeField] private float damageInterval = 1f; // Интервал времени между ударами

    [SerializeField] private AudioClip engineSound; // Звук двигателя
    [SerializeField] private AudioClip idleSound; // Звук в стоячем положении
    [SerializeField] private float volume = 1f; // Громкость звуков

    private NavMeshAgent navMeshAgent; // Компонент NavMeshAgent
    private int currentPatrolIndex = 0; // Индекс текущей точки патрулирования
    private float waitTimer = 0f; // Таймер ожидания
    private bool isWaiting = false; // Флаг ожидания
    private float lastDamageTime = 0f; // Время последнего нанесенного урона

    private AudioSource audioSource; // Компонент AudioSource
    private Animator animator; // Компонент Animator

    private bool isMoving = false; // Флаг движения

    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();

        if (patrolPoints.Length > 0)
        {
            navMeshAgent.SetDestination(patrolPoints[currentPatrolIndex].position);
        }

    }

    private void Update()
    {
        if (isWaiting)
        {
            HandleWaiting();
        }
        else
        {
            HandlePatrolling();
        }

        UpdateAnimationAndSound();
    }

    private void HandleWaiting()
    {
        // Увеличиваем таймер ожидания
        waitTimer += Time.deltaTime;
        if (waitTimer >= waitTimeAtPoint)
        {
            // Если время ожидания истекло, переходим к следующей точке
            isWaiting = false;
            waitTimer = 0f;
            currentPatrolIndex++;
            if (currentPatrolIndex >= patrolPoints.Length)
            {
                currentPatrolIndex = 0; // Зацикливаем патрулирование
            }
            navMeshAgent.SetDestination(patrolPoints[currentPatrolIndex].position);
        }
    }

    private void HandlePatrolling()
    {
        // Проверяем, достигли ли мы текущей точки патрулирования
        if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
        {
            // Начинаем ожидание
            isWaiting = true;
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        // Проверяем, столкнулась ли машина с игроком
        if (collision.gameObject.CompareTag("Player") && navMeshAgent.velocity.magnitude > 0)
        {
            // Проверяем, прошло ли достаточно времени с последнего удара
            if (Time.time - lastDamageTime >= damageInterval)
            {
                DealDamageToPlayer(collision.collider);
                lastDamageTime = Time.time; // Обновляем время последнего удара
            }
        }
    }


    private void DealDamageToPlayer(Collider playerCollider)
    {
        PlayerStats playerStats = playerCollider.GetComponent<PlayerStats>();

        if (playerStats != null)
        {
            // Уменьшаем мотивацию игрока
            playerStats.DecreaseMotivationForEnemy(motivationLoss);
        }
    }

    private void UpdateAnimationAndSound()
    {
        // Проверяем, движется ли машина
        bool currentlyMoving = navMeshAgent.velocity.magnitude > 0;

        if (currentlyMoving != isMoving)
        {
            isMoving = currentlyMoving;

            if (isMoving)
            {
                // Включаем анимацию движения и звук двигателя
                animator.SetBool("isMoving", true);
                PlaySound(engineSound);
            }
            else
            {
                // Включаем анимацию стоячего положения и звук в стоячем положении
                animator.SetBool("isMoving", false);
                PlaySound(idleSound);
            }
        }
    }

    private void PlaySound(AudioClip clip)
    {
        if (clip != null)
        {
            audioSource.clip = clip;
            audioSource.volume = volume;
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
    }
}