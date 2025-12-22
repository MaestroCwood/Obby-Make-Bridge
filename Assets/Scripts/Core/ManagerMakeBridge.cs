using System.Collections.Generic;
using UnityEngine;

public class ManagerMakeBridge : MonoBehaviour
{
    [SerializeField] GameObject bridgePrefab;
    [SerializeField] int freeCountBridgeElement = 10;
    [SerializeField] Transform startPosBridgeCreated;
    public List<GameObject> bridgesList = new List<GameObject>();

    Vector3 sizeBridge;

    private void Start()
    {
        sizeBridge = bridgePrefab.transform.localScale;
    }

    public void CreateBridgeElement()
    {
    
        int index = bridgesList.Count; 
        Vector3 pos = startPosBridgeCreated.position + Vector3.forward * (index * sizeBridge.z);

        GameObject go = Instantiate(bridgePrefab, pos, Quaternion.identity);
        bridgesList.Add(go);
    }



    private void Update()
    {
        if(Input.GetKeyUp(KeyCode.F))
        {
            CreateBridgeElement();
        }
    }
}
