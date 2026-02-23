using System;
using UnityEngine;
using YG;

public class YGRewardedSkin : MonoBehaviour
{
    [SerializeField] Animator animatorPlayer;
    [SerializeField] Avatar avatar;
    [SerializeField] GameObject skinHuggy;

    [SerializeField] GameObject[] meshOtherPLayers;
    [SerializeField] GameObject ico;

    public bool isRewardedComlited = false;



    private void Start()
    {
       
        isRewardedComlited = YG2.saves.skinHuggi == 0 ? false : true;

        if(ico != null) ico.SetActive(!isRewardedComlited);


        string currentSaveSkin = PlayerPrefs.GetString("CurrentSkin");
        if (currentSaveSkin == "Huggi") ActivateSkin();
        

    }

    private void OnGetSK()
    {
       
    }

    private void OnEnable()
    {
        YG2.onRewardAdv += OnRewarded;
        YG2.onGetSDKData += OnGetSK;

    }

    private void OnDisable()
    {
        YG2.onRewardAdv -= OnRewarded;
    }

    private void OnRewarded(string obj)
    {
        if(obj == "Huggi")
        {
            ActivateSkin();
            isRewardedComlited=true;
            YG2.saves.skinHuggi = 1;
            YG2.SaveProgress();
            if (ico != null) ico.SetActive(!isRewardedComlited);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isRewardedComlited)
        {
            YG2.RewardedAdvShow("Huggi");
        } else if (isRewardedComlited)
        {
            ActivateSkin();
        }
    }


    void ActivateSkin()
    {
        for(int i = 0; i < meshOtherPLayers.Length; i++)
        {
            meshOtherPLayers[i].SetActive(false);

        }

        animatorPlayer.avatar = avatar;
        skinHuggy.SetActive(true);
        PlayerPrefs.SetString("CurrentSkin", "Huggi");
    }
}
