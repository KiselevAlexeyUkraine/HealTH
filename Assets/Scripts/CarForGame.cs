using System;
using UnityEngine;
using UnityEngine.AI;

public class CarForGame : MonoBehaviour
{
    public Action PlayerDamaged; // Событие, вызываемое при столкновении с игроком

    [SerializeField] private Transform[] patrolPoints; // Массив точек патрулирования
    [SerializeField] private float waitTimeAtPoint = 2f; // Время ожидания на каждой точке
    [SerializeField] private float damageInterval = 1f; // Интервал времени между нанесением урона

    [SerializeField] private AudioClip engineSound; // Звук двигателя
    [SerializeField] private AudioClip idleSound; // Звук в стоячем положении
    [SerializeField] private float volume = 1f; // Громкость звуков

    private NavMeshAgent navMeshAgent; // Компонент NavMeshAgent
    private int currentPatrolIndex = 0; // Индекс текущей точки патрулирования
    private float waitTimer = 0f; // Таймер ожидания
    private float lastDamageTime = 0f; // Время последнего нанесенного урона

    private AudioSource audioSource; // Компонент AudioSource
    private Animator animator; // Компонент Animator

    private bool isMoving = false; // Флаг движения
    private bool isPlayerInsideTrigger = false; // Флаг, находится ли игрок в триггере

    private Transform playerTransform; // Ссылка на игрока для оптимизации проверок

    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();

        if (patrolPoints.Length > 0)
        {
            navMeshAgent.SetDestination(patrolPoints[currentPatrolIndex].position);
        }

        PlaySound(idleSound); // Начинаем с проигрывания звука в стоячем положении
    }

    private void FixedUpdate()
    {
        HandlePatrolMovement(); // Управление патрулем
        CheckMovementState(); // Проверяем состояние движения
    }

    private void HandlePatrolMovement()
    {
        if (isPlayerInsideTrigger) return; // Если игрок в триггере, машина не патрулирует

        if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
        {
            waitTimer += Time.fixedDeltaTime;
            if (waitTimer >= waitTimeAtPoint)
            {
                waitTimer = 0f;
                currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
                navMeshAgent.SetDestination(patrolPoints[currentPatrolIndex].position);
            }
        }
    }

    private void CheckMovementState()
    {
        bool currentlyMoving = navMeshAgent.velocity.sqrMagnitude > 0.01f;

        if (currentlyMoving != isMoving)
        {
            isMoving = currentlyMoving;

            if (isMoving)
            {
                SetMovingState(true);
            }
            else
            {
                SetMovingState(false);
            }
        }
    }

    private void SetMovingState(bool moving)
    {
        animator?.SetBool("isMoving", moving);

        if (moving)
        {
            PlaySound(engineSound);
        }
        else
        {
            PlaySound(idleSound);
        }
    }

    private void PlaySound(AudioClip clip)
    {
        if (clip != null && audioSource.clip != clip)
        {
            audioSource.clip = clip;
            audioSource.volume = volume;
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInsideTrigger = true;
            playerTransform = other.transform; // Сохраняем ссылку на игрока
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (isPlayerInsideTrigger && playerTransform != null && isMoving && Time.time > lastDamageTime + damageInterval)
        {
            PlayerDamaged?.Invoke();
            lastDamageTime = Time.time;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInsideTrigger = false;
            playerTransform = null; // Убираем ссылку на игрока
        }
    }
}
