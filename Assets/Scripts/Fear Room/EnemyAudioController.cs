using UnityEngine;

public class EnemyAudioController : MonoBehaviour
{
    public static EnemyAudioController Instance { get; private set; }
    [SerializeField] float timerCoulDownSound = 10f;
   
    [SerializeField] AudioClip[] audioClips;
    AudioSource audioSource;
    float currenTimeDelaySound;

    private void Awake()
    {   
        Instance = this;
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        currenTimeDelaySound = timerCoulDownSound;
    }


    private void Update()
    {
     
        currenTimeDelaySound -= Time.deltaTime;
    }


    public void PlayFx()
    {
       
        if (audioSource.isPlaying || currenTimeDelaySound > 0) return;
        int rand = Random.Range(0, audioClips.Length);
        audioSource.PlayOneShot(audioClips[rand]);
        currenTimeDelaySound = timerCoulDownSound;
    }
}
