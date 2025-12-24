using System;
using UnityEngine;

public class ShopManager : MonoBehaviour
{




    private void Start()
    {
        GameEvents.OnBuyedBridge += OnBuyedBridge;
    }


    private void OnDisable()
    {
        GameEvents.OnBuyedBridge -= OnBuyedBridge;
    }

    private void OnBuyedBridge(int obj, int ID)
    {
        Debug.Log("BUY!");
    }
}
