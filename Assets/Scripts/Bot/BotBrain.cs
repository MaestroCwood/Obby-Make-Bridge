using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Splines;

public class BotBrain : MonoBehaviour
{
    public SimpleAIThirdPersonController controller;
    public SplineAnimate splineAnimateTelejka;
    [SerializeField] private Transform[] targets;
    [SerializeField] private GameObject telejkaPrefab;
    [SerializeField] private SplineContainer splineContainer;
    [SerializeField] private GameObject[] bridgeElements;
    [SerializeField] private Transform startPosBridgeCreated;
    [SerializeField] SplineAnimate splineBot;
    [SerializeField] bool isMakingBridge;
    private NavMeshAgent agent;

    private int currentBridgeIndex = 0;
    private int countCurrentMakeBridgeElement = 0;
    Vector3 sizeBridge;

    bool isStartMake = false;
    private bool isWaitingAtTarget = false;
    private enum State
    {
        GotoTarget0,
        RideTelejka,
        GotoTarget1,
        RideGorka,
        GotoTarget2,
        Idle
    }

    private State currentState;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        TransitionTo(State.GotoTarget0);
        sizeBridge = bridgeElements[0].GetComponent<Renderer>().bounds.size;


        StartCoroutine(BuildBridgeRoutine());
    }

    private void TransitionTo(State newState)
    {
        if (splineAnimateTelejka != null)
            splineAnimateTelejka.Completed -= OnTelejkaCompleted;
        if (splineBot != null)
            splineBot.Completed -= OnGorkaCompleted;

        currentState = newState;

        switch (currentState)
        {
            case State.GotoTarget0:
                EnableAgent();
                agent.SetDestination(targets[0].position);
                break;

            case State.RideTelejka:
                StartCoroutine(SpawnAndRideTelejka());
                break;

            case State.GotoTarget1:
                EnableAgent();
                agent.SetDestination(targets[1].position);
                break;

            case State.RideGorka:
                StartCoroutine(RideGorkaSpline());
                break;

            case State.GotoTarget2:
                EnableAgent();
                agent.SetDestination(targets[2].position);
                break;
        }
    }

    void EnableAgent()
    {
        agent.enabled = true;
        agent.isStopped = false;
    }

    IEnumerator BuildBridgeRoutine()
    {
        while (countCurrentMakeBridgeElement < 10 && isMakingBridge)
        {
            if (isStartMake)
            {
                yield return new WaitForSeconds(Random.Range(25f, 95f));

                int randIndex = Random.Range(0, bridgeElements.Length);
                GameObject prefab = bridgeElements[randIndex];

                Vector3 pos = startPosBridgeCreated.position + Vector3.forward * (currentBridgeIndex * sizeBridge.z);
                GameObject go = Instantiate(prefab, pos, Quaternion.identity);

                var coinGen = go.GetComponent<GenerateCoin>();
                if (coinGen != null) Destroy(coinGen);

                currentBridgeIndex++;
                countCurrentMakeBridgeElement++;

                Debug.Log("Построен блок #" + countCurrentMakeBridgeElement);
            }
            else
            {
                yield return null;
            }


        }

        Debug.Log("===== МОСТ ГОТОВ =====");
    }
    private IEnumerator SpawnAndRideTelejka()
    {
        yield return new WaitForSeconds(1f);
        var telejka = Instantiate(telejkaPrefab, transform.position, Quaternion.identity);
        transform.SetParent(telejka.transform);
        splineAnimateTelejka = telejka.GetComponent<SplineAnimate>();
        splineAnimateTelejka.Container = splineContainer;
        splineAnimateTelejka.Completed += OnTelejkaCompleted;
        agent.enabled = false;
        transform.localPosition = new Vector3(0, 3, 0);
        transform.rotation = Quaternion.Euler(0f, 90f, 0f);
        splineAnimateTelejka.Play();
    }

    private void OnTelejkaCompleted()
    {
        splineAnimateTelejka.Completed -= OnTelejkaCompleted;
        transform.SetParent(null);
        TransitionTo(State.GotoTarget1);

        isStartMake = true;
    }

    private IEnumerator RideGorkaSpline()
    {
        yield return new WaitForSeconds(2f);
        agent.enabled = false;
        splineBot.enabled = true;
        splineBot.Completed += OnGorkaCompleted;
        transform.SetParent(splineBot.transform, false);
        transform.localPosition = Vector3.zero;
        splineBot.Play();
    }

    private void OnGorkaCompleted()
    {
        splineBot.Completed -= OnGorkaCompleted;
        splineBot.enabled = false;
        transform.SetParent(null, false);

        // Если еще не построено 10 элементов, бот идет к точке 2 и потом снова на горку
        if (countCurrentMakeBridgeElement < 10)
        {
            TransitionTo(State.GotoTarget2);
        }
        else
        {
            // Когда мост готов, бот возвращается к точке 0
            TransitionTo(State.GotoTarget0);
        }

        splineBot.Restart(false);
    }

    private void Update()
    {
        if (agent.enabled && !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            switch (currentState)
            {
                case State.GotoTarget0:
                    TransitionTo(State.RideTelejka);
                    break;

                case State.GotoTarget1:
                    TransitionTo(State.RideGorka);
                    break;

                case State.GotoTarget2:
                    if (countCurrentMakeBridgeElement < 10)
                        TransitionTo(State.RideGorka); // Снова на горку
                    else
                        TransitionTo(State.GotoTarget0); // Мост готов
                    break;

            }
        }
    }
}
