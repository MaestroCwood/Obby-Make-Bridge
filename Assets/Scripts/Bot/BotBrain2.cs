using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class BotBrain2 : MonoBehaviour
{

    [SerializeField] Transform[] target;
    NavMeshAgent agent;
    SimpleAIThirdPersonController controller;

    float distanceToTarget;
    int currentindextPos = 0;
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        controller = GetComponent<SimpleAIThirdPersonController>();
    }

    private void Start()
    {
        StartCoroutine(ScanToTatget());
        StartToTarget();
    }


    public void StartToTarget()
    {   
       
        agent.SetDestination(target[currentindextPos].position);
             

    }


    IEnumerator ScanToTatget()
    {
        while (true)
        {
            if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
            {
                currentindextPos++;

                if (currentindextPos >= target.Length)
                    currentindextPos = 0;

                StartToTarget();
            }

            yield return new WaitForSeconds(Random.Range(1f,3f));
        }
    }
}
