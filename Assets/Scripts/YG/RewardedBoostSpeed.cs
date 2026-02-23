using StarterAssets;
using System;
using System.Collections;
using UnityEngine;
using YG;

public class RewardedBoostSpeed : MonoBehaviour
{
    [SerializeField] ThirdPersonController playeController;
    [SerializeField] float boostTime = 60f;
    [SerializeField] float addBoost = 5f;
    [SerializeField] GameObject[] trigerObj;

    CapsuleCollider colliderCapsul;
    MeshRenderer meshRener;


    private void Awake()
    {
        colliderCapsul = GetComponent<CapsuleCollider>();
        meshRener = GetComponent<MeshRenderer>();
    }
    private void OnEnable()
    {
        YG2.onRewardAdv += OnRewardedShow;
    }

    private void OnDisable()
    {
        YG2.onRewardAdv -= OnRewardedShow;
    }

    private void OnRewardedShow(string obj)
    {   
        if(obj == "Speed")
        {
            StartCoroutine(BoostSpeedTime());
        }
       
    }

    private void OnTriggerEnter(Collider other)
    {
        YG2.RewardedAdvShow("Speed");
    }



    void DiactivateObj(bool activeta)
    {
        for (int i = 0; i < trigerObj.Length; i++)
        {
            trigerObj[i].gameObject.SetActive(activeta); 
           
        }
        colliderCapsul.enabled = activeta;
        meshRener.enabled = activeta;
    }


    IEnumerator BoostSpeedTime()
    {
        DiactivateObj(false);
        
        playeController.SprintSpeed += addBoost;
        yield return new WaitForSeconds(boostTime);
        playeController.SprintSpeed -= addBoost;
        DiactivateObj(true);
    }
}
