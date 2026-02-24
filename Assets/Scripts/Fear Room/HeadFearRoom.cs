using StarterAssets;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class HeadFearRoom : MonoBehaviour
{



    [Header("Setings")]
    [SerializeField] float stopDistance = 2f;
    [SerializeField] float speed;
    [SerializeField] float minTimeLifeEnemy = 10;
    [SerializeField] float maxTimeLifeEnemy = 10;
    [SerializeField] float distanceToPlaySound = 15f;

    [Header("Reference")]
    [SerializeField] GameObject fxDedtroy;



    ThirdPersonController playerControllerl;

    NavMeshAgent agent;



    bool isHasTriggerPlayer = false;
    bool isLifeEnemy = true;

    Transform startPos;

    float currentLifeTime;
    float lifeTime;
    float distanceToPlayer;
    float randSpeed;
    float delayCallSound;
    private void Awake()
    {

        playerControllerl = FindAnyObjectByType<ThirdPersonController>();
        agent = GetComponent<NavMeshAgent>();

       
        startPos = playerControllerl.transform;
    }
    private void Start()
    {
        randSpeed = Random.Range(playerControllerl.SprintSpeed - 5, playerControllerl.SprintSpeed);
        
       
        SetingsHead();
        StartCoroutine(HeadTarget());
        lifeTime = Random.Range(minTimeLifeEnemy, maxTimeLifeEnemy);
        currentLifeTime = lifeTime;
        agent.speed = randSpeed;
    }

    private void Update()
    {
        if (isLifeEnemy && currentLifeTime > 0)
        {
            currentLifeTime -= Time.deltaTime;
        }
        else
        {
            DestroyHead();
        }

        distanceToPlayer = Vector3.Distance(playerControllerl.transform.position, transform.position);
        if (distanceToPlayer < distanceToPlaySound && delayCallSound > 1)
        {
            EnemyAudioController.Instance.PlayFx();
            delayCallSound = 0;
            Debug.Log("PLAY SOUND!!!");
        }

        delayCallSound += Time.deltaTime;
    }

    void SetingsHead()
    {
        agent.speed = speed;
    }

    IEnumerator HeadTarget()
    {

        while (true && !isHasTriggerPlayer && agent.isOnNavMesh)
        {
            if (agent != null)
            {
               
                agent.SetDestination(playerControllerl.transform.position);
                yield return new WaitForSeconds(0.3f);
            }

        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            GameEvents.OnPlayerTriggerHeadFear?.Invoke();
          
            DestroyHead();

        }
    }

    void DestroyHead()
    {
        isLifeEnemy = false;
        GameEvents.OnDiedEnemyHead?.Invoke();
        AudioManager.instance.PlayFx(3);
        Instantiate(fxDedtroy, new Vector3(agent.transform.position.x, 2, agent.transform.position.z), Quaternion.identity);
        Destroy(gameObject);
    }


}
