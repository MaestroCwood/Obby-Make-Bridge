using System;
using UnityEngine;


[DefaultExecutionOrder(-100)]
public class GameManager : MonoBehaviour
{
    public static GameManager instance;


    public int curentCountCoin { get; private set; }


    public event EventHandler<int> OnUpdateCointCoin;
    
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else Destroy(gameObject);


        
    }

    private void Start()
    {
        curentCountCoin = PlayerPrefs.GetInt("Coin");
        OnUpdateCointCoin?.Invoke(this, curentCountCoin);
    }

    private void OnEnable()
    {
        GameEvents.OnGenerateCoin += OnGenerateCoin;
    }

    private void OnDisable()
    {
        GameEvents.OnGenerateCoin -= OnGenerateCoin;
    }

    private void OnGenerateCoin(int obj)
    {
        AddCoin(obj);

        
    }

    public void AddCoin(int coin)
    {
        curentCountCoin += coin;

        OnUpdateCointCoin?.Invoke(this, curentCountCoin);

        Save();
    }

    public void DecreamenteCoin(int coin)
    {
        curentCountCoin -= coin;
        OnUpdateCointCoin?.Invoke(this, curentCountCoin);
        Save();
    }

    void Save()
    {
        PlayerPrefs.SetInt("Coin", curentCountCoin);
    }


    public void BuyBridge()
    {

    }
}
