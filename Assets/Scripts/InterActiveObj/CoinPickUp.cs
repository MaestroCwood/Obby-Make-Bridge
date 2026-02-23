using System.Collections;
using UnityEngine;

public class CoinPickUp : MonoBehaviour
{
    public int minAddCoin = 1;
    public int maxAddCoin = 100;

    public float delayToDeactivate = 10f;

    MeshRenderer meshCoin;
    SphereCollider colliderCoin;


    private void Awake()
    {
        meshCoin = GetComponent<MeshRenderer>();
        colliderCoin = GetComponent<SphereCollider>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            AddCpoinPlayer();
            StartCoroutine(DelayToDeactivateCoin());
            Debug.Log("TRIGGER COIN!");
        }
    }


    void AddCpoinPlayer()
    {
        int rand = Random.Range(minAddCoin, maxAddCoin);
        GameManager.instance.AddCoin(rand);
        CoinEffect.Play(rand);
    }

    void DeactivateCoin()
    {
        meshCoin.gameObject.SetActive(false);
        colliderCoin.gameObject.SetActive(false);
    }

    void ActivateCoin()
    {
        meshCoin.gameObject.SetActive(true);
        colliderCoin.gameObject.SetActive(true);
    }


    IEnumerator DelayToDeactivateCoin()
    {
        DeactivateCoin();
        yield return new WaitForSeconds(delayToDeactivate);
        ActivateCoin();
    }
}
