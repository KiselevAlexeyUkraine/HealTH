using UnityEngine;
using UnityEngine.AI;

public class PeacefulNPC : MonoBehaviour
{
    [SerializeField] Transform[] patrolPoints; // Точки для патрулирования
    [SerializeField] float patrolSpeed = 2f; // Скорость патрулирования
    [SerializeField] float waitTimeAtPatrolPoint = 2f; // Время ожидания на точке патрулирования
    [SerializeField]  AudioClip patrolClip; // Звук для патрулирования

    NavMeshAgent agent;
    Animator animator; // Аниматор для управления анимациями
    AudioSource audioSource; // Источник звука
    int currentPatrolIndex;
    bool isWaiting;
    float waitTimer;

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
            Debug.LogWarning("No patrol points assigned to peaceful NPC.");
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
        Patrol();
    }

    void Patrol()
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
            animator.SetBool("IsWalking", false); // Устанавливаем булевое значение для ожидания
        }
        else
        {
            animator.SetBool("IsWalking", true); // Устанавливаем булевое значение для ходьбы
        }
    }

    void MoveToNextPatrolPoint()
    {
        if (patrolPoints.Length == 0) return;

        agent.destination = patrolPoints[currentPatrolIndex].position;
        currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;

    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        foreach (var point in patrolPoints)
        {
            Gizmos.DrawSphere(point.position, 0.2f);
        }
    }
}