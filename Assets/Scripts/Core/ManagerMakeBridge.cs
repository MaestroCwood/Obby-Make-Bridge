using StarterAssets;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ManagerMakeBridge : MonoBehaviour
{
    [SerializeField] ThirdPersonController playerController;

    [SerializeField] GameObject[] bridgePrefab;
    [SerializeField] int freeCountBridgeElement = 1;
   
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

        float distance = Vector3.Distance(startPosBridgeCreated.position, playerController.transform.position);
        if(GameManager.instance.curentCountCoin < 5000)
        {
            int donHaveMany = 5000 - GameManager.instance.curentCountCoin;
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
        bridgesList.Add(go);
        GameManager.instance.DecreamenteCoin(5000);

        GameEvents.OnMakeBridgeElement?.Invoke(cointCurrentMakeBridgeElement + 1);
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
}
