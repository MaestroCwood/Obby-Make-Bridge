using System;
using System.Collections;
using UnityEngine;

public class FairLight : MonoBehaviour
{
    [SerializeField] GameObject fairWhite;
    [SerializeField] GameObject fairRed;
    [SerializeField] GameObject tutorFindFireTxt;
    [SerializeField] GameObject tutorFindExitTxt;
    [SerializeField] GameObject tutorGameObj;

    [SerializeField] Transform playerHoldPos;

    bool isPickUpFire = false;

 
    Vector3 startPos;
    private void Awake()
    {
        GameEvents.OnPlayerTriggerHeadFear += OnTriggerHeadPlayer;
    }

    private void Start()
    {
        startPos = transform.position;
    }
    private void OnDisable()
    {
        GameEvents.OnPlayerTriggerHeadFear -= OnTriggerHeadPlayer;
    }

    private void OnTriggerHeadPlayer()
    {
        isPickUpFire = false;
        transform.SetParent(null);
        transform.position = startPos;
        SwitchFire(true, false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isPickUpFire && other.CompareTag("Player"))
        {
            isPickUpFire=true;
            tutorFindFireTxt.SetActive(false);
            tutorFindExitTxt.SetActive(true);
            StartCoroutine(OutShowTutorObj());
            PickUpFire();
        }
    }


    void PickUpFire()
    {
        SwitchFire(false, true);
        gameObject.transform.SetParent(playerHoldPos, false );
        transform.localPosition = Vector3.zero;
        transform.localPosition = Vector3.up * 3.7f;
    }

    void SwitchFire(bool red, bool white)
    {
        fairRed.SetActive(red);
        fairWhite.SetActive(white);
    }

    IEnumerator OutShowTutorObj()
    {
        yield return new WaitForSeconds(10f);
        tutorGameObj.gameObject.SetActive(false);
    }
}
