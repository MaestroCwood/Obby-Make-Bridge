using System;
using TMPro;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI countCoin;


    private void OnEnable()
    {
        GameManager.instance.OnUpdateCointCoin += Instance_OnUpdateCointCoin;
    }

    private void OnDisable()
    {
        GameManager.instance.OnUpdateCointCoin -= Instance_OnUpdateCointCoin;
    }

    private void Instance_OnUpdateCointCoin(object sender, int e)
    {
        countCoin.text = e.ToString();
    }
}
