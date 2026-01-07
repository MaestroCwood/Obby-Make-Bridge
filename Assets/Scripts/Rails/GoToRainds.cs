using StarterAssets;
using System;
using UnityEngine;
using UnityEngine.Splines;

public class GoToRainds : MonoBehaviour
{
    [SerializeField] SplineContainer splineContainer;
    [SerializeField] MakeRaidsManager makeRaidsManager;
    [SerializeField] ThirdPersonController playerController;
    [SerializeField] StarterAssetsInputs assetsInputs;
    [SerializeField] SplineAnimate splineAnimateObj;
    [SerializeField] GameObject[] raids;
    public float speed = 0.3f;
    public static event EventHandler OnStartRaindGoMove;
    public static event EventHandler OnStopRaindGoMove;
    float distance;

    public bool isRiding { get; private set; } 

    private void Start()
    {
        assetsInputs.OnStartJumpPlayer += PlayerController_OnJump;
    }

    private void OnDisable()
    {
        assetsInputs.OnStartJumpPlayer -= PlayerController_OnJump;
    }

    private void PlayerController_OnJump()
    {   
        if (isRiding) 
            StopGoAndResetSatate();
    }

    public void StopGoAndResetSatate()
    {
        if (makeRaidsManager.RaidsElementCount < 2) return;
        isRiding = false;
        playerController.enabled = true;
        playerController.GetComponent<Animator>().enabled = enabled;
        playerController.gameObject.transform.SetParent(null);
        playerController.gameObject.transform.localScale = Vector3.one;

        for (int i = 0; i < raids.Length; i++)
        {
            raids[i].SetActive(false);
        }
        splineAnimateObj.Restart(false);
        OnStopRaindGoMove?.Invoke(this, EventArgs.Empty);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            GoToTelejkaRain();
        }
        if (isRiding)
        {
            float input = 0f;
            if (Input.GetKey(KeyCode.W) || assetsInputs.move.y > 0.01f)
                input = 1f;

            if (Input.GetKey(KeyCode.S) || assetsInputs.move.y < -0.1f)
                input = -1f;

            splineAnimateObj.NormalizedTime +=
                input * Time.deltaTime * splineAnimateObj.MaxSpeed;

            splineAnimateObj.NormalizedTime = Mathf.Clamp01(
                splineAnimateObj.NormalizedTime);
        }
    }

    private void MoveForward()
    {
        splineAnimateObj.Play();     // Запускаем движение
        
    }


    public void GoToTelejkaRain()
    {   
        if(makeRaidsManager.RaidsElementCount < 2) return;
        float splineLength = splineContainer.CalculateLength();
        splineAnimateObj.MaxSpeed = speed / splineLength;
        splineAnimateObj.enabled = true;
        Spline spline = splineContainer.Spline;
        Vector3 playerWorldPos = playerController.transform.position;
        int closestKnotIndex = -1;
        float minDistance = float.MaxValue;
        Transform hold = null;
        for (int i = 0; i < spline.Count; i++)
        {
            // Получаем локальную позицию узла
            Vector3 localKnotPos = spline[i].Position;

            // Переводим её в мировые координаты
            Vector3 worldKnotPos = splineContainer.transform.TransformPoint(localKnotPos);

            // Считаем расстояние
            float distance = Vector3.Distance(playerWorldPos, worldKnotPos);
           
            // Ищем минимум
            if (distance < minDistance)
            {
                minDistance = distance;
                closestKnotIndex = i;

                int randrRaid = UnityEngine.Random.Range(0, raids.Length);
                for(int t =0; t < raids.Length; t++)
                {
                    raids[t].SetActive(false);
                }
                raids[randrRaid].SetActive(true);
                hold = raids[randrRaid].GetComponentInChildren<Transform>();
            }
        }
       
       
        if (spline.Count > 2 && minDistance < 13f)
        {
            OnStartRaindGoMove?.Invoke(this, EventArgs.Empty);
            playerController.enabled = false;
           
            playerController.GetComponent<Animator>().enabled = false;
            playerController.gameObject.transform.SetParent(splineAnimateObj.transform, false);
            //playerController.gameObject.transform.SetParent(hold.transform, false);
            playerController.transform.localPosition = new Vector3(0, 2, 0);
            playerController.transform.localRotation = Quaternion.identity;
            isRiding = true;
            splineAnimateObj.Play();

            //splineAnimateObj.Completed += SplineAnimateObj_Completed;
        } else
        {
            Debug.Log("HUI NO COIN <2");
        }
       
    }

    private void SplineAnimateObj_Completed()
    {
        splineAnimateObj.Completed -= SplineAnimateObj_Completed;
        playerController.enabled = true;
        playerController.GetComponent<Animator>().enabled = enabled;
        playerController.gameObject.transform.SetParent(null);
        playerController.gameObject.transform.localScale = Vector3.one;
      //  splineAnimateObj.Restart(true);
    }
}
