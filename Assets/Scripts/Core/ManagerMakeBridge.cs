using StarterAssets;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerMakeBridge : MonoBehaviour
{
    [SerializeField] ThirdPersonController playerController;

    [SerializeField] GameObject[] bridgePrefab;
    [SerializeField] int freeCountBridgeElement = 1;
    [SerializeField] int priceOneMakeElement = 500;
   
    [SerializeField] Transform startPosBridgeCreated;
    public List<GameObject> bridgesList = new List<GameObject>();
    public int maxMakeElementBridge = 10;

    [SerializeField] float minDistanceToMake = 15f;
    Vector3 sizeBridge;

    int currentIDBridgePrefabs = 0;

    int cointCurrentMakeBridgeElement;


    public event EventHandler<float> OnWarningMessgeDistance;
    private void Start()
    {
        sizeBridge = bridgePrefab[currentIDBridgePrefabs].GetComponent<Renderer>().bounds.size;


        GameEvents.OnSelectedBridge += OnSelectedBridge;

        GameEvents.OnMakeBridgeElement?.Invoke(cointCurrentMakeBridgeElement);

        LoadBridges();
    }

    private void OnDisable()
    {
        GameEvents.OnSelectedBridge -= OnSelectedBridge;
    }

    private void OnSelectedBridge(int ID, Sprite ico)
    {
        currentIDBridgePrefabs = ID;
        Debug.Log("OnSelected!");
    }

    public void CreateBridgeElement()
    {
        if(bridgesList.Count >= maxMakeElementBridge) return;

        float distance = Vector3.Distance(startPosBridgeCreated.position, playerController.transform.position);
        if(GameManager.instance.curentCountCoin < priceOneMakeElement)
        {
            int donHaveMany = priceOneMakeElement - GameManager.instance.curentCountCoin;
            GameEvents.OnDontHaveMany?.Invoke(donHaveMany);
            return;
        }
        if (distance > minDistanceToMake)
        {
            ShowMessegeMake(distance);
            return;
        }
            

        cointCurrentMakeBridgeElement = bridgesList.Count; 
        Vector3 pos = startPosBridgeCreated.position + Vector3.forward * (cointCurrentMakeBridgeElement * sizeBridge.z);

        GameObject go = Instantiate(bridgePrefab[currentIDBridgePrefabs], pos, Quaternion.identity);
        BridgeElement element = go.AddComponent<BridgeElement>();
        element.prefabID = currentIDBridgePrefabs;
        bridgesList.Add(go);
        GameManager.instance.DecreamenteCoin(priceOneMakeElement);

        GameEvents.OnMakeBridgeElement?.Invoke(cointCurrentMakeBridgeElement + 1);
        SaveBridges();
    }

    void ShowMessegeMake(float distance)
    {
        OnWarningMessgeDistance?.Invoke(this, distance);
    }

    private void Update()
    {
        if(Input.GetKeyUp(KeyCode.E))
        {
            CreateBridgeElement();
        }

    }


    void SaveBridges()
    {
        List<BridgeSaveData> saveData = new List<BridgeSaveData>();

        for (int i = 0; i < bridgesList.Count; i++)
        {
            BridgeElement element = bridgesList[i].GetComponent<BridgeElement>();

            saveData.Add(new BridgeSaveData
            {
                prefabID = element.prefabID,
                position = bridgesList[i].transform.position
            });
        }

        string json = JsonUtility.ToJson(new Wrapper<BridgeSaveData>(saveData));
        PlayerPrefs.SetString("BRIDGES_SAVE", json);
        PlayerPrefs.Save();
    }


    void LoadBridges()
    {
        if (!PlayerPrefs.HasKey("BRIDGES_SAVE"))
            return;

        string json = PlayerPrefs.GetString("BRIDGES_SAVE");
        Wrapper<BridgeSaveData> wrapper = JsonUtility.FromJson<Wrapper<BridgeSaveData>>(json);

        foreach (var data in wrapper.items)
        {
            GameObject go = Instantiate(
                bridgePrefab[data.prefabID],
                data.position,
                Quaternion.identity
            );
            BridgeElement element = go.AddComponent<BridgeElement>();
            bridgesList.Add(go);
        }

        GameEvents.OnMakeBridgeElement?.Invoke(bridgesList.Count);
    }

    public void WrapperDestroyElementBridge()
    {
        StartCoroutine(ClearBridge());
    }
    IEnumerator ClearBridge()
    {
        for (int i = bridgesList.Count - 1; i >=0; i--)
        {
            GameEvents.OnDestrouBridgeElement?.Invoke(bridgesList[i].transform.position);
            var bridge = bridgesList[i];
            Destroy(bridge.gameObject);
            
            yield return new WaitForSeconds(0.3f);
        }

   
        cointCurrentMakeBridgeElement = 0;
        bridgesList.Clear();
        GameEvents.OnMakeBridgeElement?.Invoke(bridgesList.Count);
    }
}
[Serializable]
public class BridgeSaveData
{
    public int prefabID;
    public Vector3 position;
}

[Serializable]
public class Wrapper<T>
{
    public List<T> items;

    public Wrapper(List<T> list)
    {
        items = list;
    }
}