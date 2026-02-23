using StarterAssets;
using System;
using UnityEngine;

public class PlayerDead : MonoBehaviour
{

    ThirdPersonController playerController;

    Vector3 statPos;


    private void Awake()
    {
        playerController = GetComponent<ThirdPersonController>();
        statPos = playerController.transform.position;
    }

    private void OnEnable()
    {
        GameEvents.OnPlayerTriggerHeadFear += OnTriggerHead;
    }

    private void OnDisable()
    {
        GameEvents.OnPlayerTriggerHeadFear -= OnTriggerHead;
    }

    private void OnTriggerHead()
    {
        playerController.Teleport(statPos);

        Debug.Log("TRIGGER!!!");
    }
}
