using StarterAssets;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.AI;

public class SahurController : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] ThirdPersonController playerController;

    [Header("Patrol Points")]
    [SerializeField] Transform pointA;
    [SerializeField] Transform pointB;

    [Header("Distances")]
    [SerializeField] float chaseDistance = 20f;

    [Header("Scale Settings")]
    [SerializeField] Vector3 normalScale = Vector3.one;
    [SerializeField] Vector3 chaseScale = new Vector3(1.3f, 1.3f, 1.3f);
    [SerializeField] float scaleSpeed = 2f;

    [Header("Speed")]
    [SerializeField] float patrolSpeed = 2f;
    [SerializeField] float chaseSpeed = 5f;

    [Header("Cinemachine Shake")]
    [SerializeField] CinemachineImpulseSource impulseSource;

    private NavMeshAgent agent;
    private Animator animator;

    private Transform currentPatrolTarget;

    private enum AIState { Patrol, Chase }
    private AIState state;

    Vector3 startPosPlayer;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        agent.stoppingDistance = 0.3f;
        agent.autoBraking = true;   // Рекомендуется true для точной остановки у точек

        currentPatrolTarget = pointA;
        agent.speed = patrolSpeed;

        state = AIState.Patrol;
        agent.SetDestination(currentPatrolTarget.position);

        startPosPlayer = playerController.transform.position;
        animator.SetBool("Run", true);
    }

    private void Update()
    {
        float distance = Vector3.Distance(transform.position, playerController.transform.position);

        switch (state)
        {
            case AIState.Patrol:
                PatrolState(distance);
                break;

            case AIState.Chase:
                ChaseState(distance);
                break;
        }
    }

    void PatrolState(float distance)
    {
        // Переход в погоню
        if (distance <= chaseDistance)
        {
            state = AIState.Chase;
            agent.speed = chaseSpeed;
             // Включаем анимацию бега
            impulseSource?.GenerateImpulse();
            return;
        }

        // Смена точки патрулирования при достижении текущей
        // Используем прямое расстояние до цели, а не agent.remainingDistance (надёжнее при autoBraking = false)
        if (Vector3.Distance(transform.position, currentPatrolTarget.position) <= agent.stoppingDistance + 0.1f)
        {
            currentPatrolTarget = currentPatrolTarget == pointA ? pointB : pointA;
            agent.SetDestination(currentPatrolTarget.position);
            animator.SetBool("Run", true);
        }

        // Плавное возвращение к нормальному масштабу
        transform.localScale = Vector3.Lerp(
            transform.localScale,
            normalScale,
            Time.deltaTime * scaleSpeed
        );
    }

    void ChaseState(float distance)
    {
        // Возврат в патруль, если игрок далеко
        if (distance > chaseDistance)
        {
            state = AIState.Patrol;
            agent.speed = patrolSpeed;
            animator.SetBool("Run", true);   // <-- ИСПРАВЛЕНО: выключаем бег
            agent.SetDestination(currentPatrolTarget.position);
            return;
        }

        // Преследование игрока
        agent.SetDestination(playerController.transform.position);

        // Увеличение масштаба
        transform.localScale = Vector3.Lerp(
            transform.localScale,
            chaseScale,
            Time.deltaTime * scaleSpeed
        );
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerController.Teleport(startPosPlayer);
        }
    }
}