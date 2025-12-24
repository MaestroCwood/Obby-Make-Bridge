using System;
using UnityEngine;
using UnityEngine.UI;

public class GameEvents : MonoBehaviour
{
    public static Action<int> OnGenerateCoin;

    public static Action<int, int> OnBuyedBridge;

    public static Action<int, Sprite> OnSelectedBridge;

    public static Action<int> OnMakeBridgeElement;
    public static Action<int> OnDontHaveMany;
}
