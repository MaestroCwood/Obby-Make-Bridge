using StarterAssets;
using Unity.Cinemachine;
using UnityEngine;
using YG;

public class CutSceneControl : MonoBehaviour
{
    [SerializeField] GameObject[] targetActiveObj;
    [SerializeField] GameObject uiMobileCanvas;
    [SerializeField] CinemachineCamera cameraAnimate;
    [SerializeField] StarterAssetsInputs starterAssetsInputs;
    [SerializeField] ThirdPersonController thirdPersonController;
    [SerializeField] CheckDevice checkDevice;

    [SerializeField] AudioSource musicBgSource;
    private void Start()
    {
        starterAssetsInputs.enabled = false;
        thirdPersonController.enabled = false;
    }
    public void ActivateObje()
    {
        foreach (GameObject obj in targetActiveObj)
        {
            obj.SetActive(true);
        }

        starterAssetsInputs.enabled =true;
        thirdPersonController.enabled=true;
        DeactivateCam();
        PlayBgMusic();

        checkDevice.ChangeActiveSelfObject();
        if (YG2.envir.isMobile)
            uiMobileCanvas.SetActive(true);
        gameObject.SetActive(false);
        
    }


    void DeactivateCam()
    {
        cameraAnimate.Priority = 0;
    }

    public void ResetAnimate()
    {
        ActivateObje();
        
    }


    void PlayBgMusic()
    {
        musicBgSource.PlayDelayed(5f);
    }
}
