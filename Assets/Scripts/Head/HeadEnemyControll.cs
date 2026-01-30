using StarterAssets;
using UnityEngine;

public class HeadEnemyControll : MonoBehaviour
{
    [SerializeField] ThirdPersonController playerController;
    [SerializeField] Transform respawnPos;
    [SerializeField] float offsetZ = -10f;
    [SerializeField] float timerCoulDownSound = 10f;
    [SerializeField] float distanceToPlaySound = 15f;
    [SerializeField] float speedMove = 10f;

    [SerializeField] AudioClip[] audioHead;
    AudioSource audioSourceHead;

    Vector3 startPos;
    float currentTimer;
    float distanceToPlayer;
    private void Awake()
    {
        audioSourceHead = GetComponent<AudioSource>();
    }

    private void Start()
    {
        currentTimer = timerCoulDownSound;
        startPos = transform.position;
        Move();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            LoseResetPosition();
            Debug.Log("Trigger lose!!!");
        }
    }

    private void OnDestroy()
    {
        LeanTween.cancel(gameObject);
    }

    void LoseResetPosition()
    {
        AudioManager.instance.PlayFx(0);
        Vector3 newpos = new Vector3(respawnPos.position.x, respawnPos.position.y, respawnPos.position.z - offsetZ);
        playerController.Teleport(newpos);
    }


    void Move()
    {
        transform.LeanMoveZ(respawnPos.position.z, speedMove).setOnComplete(() =>
        {
            transform.position = startPos;
            Move();
        });
    }


    private void Update()
    {
        distanceToPlayer = Vector3.Distance(playerController.transform.position, transform.position);
        if (distanceToPlayer < distanceToPlaySound && !audioSourceHead.isPlaying && currentTimer <= 0f)
        {
            
            PlayFx();

            currentTimer = timerCoulDownSound;

            Debug.Log("PLAY SOUND!!!");
        }

        currentTimer -= Time.deltaTime;
    }

    void PlayFx()
    {
        int rand = Random.Range(0, audioHead.Length);
        audioSourceHead.PlayOneShot(audioHead[rand]);
    }

}
