using StarterAssets;
using System;
using UnityEngine;

public class SpawnManagerHead : MonoBehaviour
{
    [SerializeField] GameObject headPrefab;
    [SerializeField] int maxHeadCount = 3;
    [SerializeField] float delayToSpawn = 5;

    [SerializeField] ThirdPersonController playerController;

    Transform[] spawnPoints;


    float currentTime = 0;
    int currentCountEnemy;

    private void Awake()
    {
        int childCount = transform.childCount;
        spawnPoints = new Transform[childCount];
        for (int i = 0; i < childCount; i++)
        {
            spawnPoints[i] = transform.GetChild(i);
        }


        
    }

    private void OnEnable()
    {
        GameEvents.OnDiedEnemyHead += OnDiedEnemy;
    }

    private void OnDisable()
    {
        GameEvents.OnDiedEnemyHead -= OnDiedEnemy;
    }

    private void OnDiedEnemy()
    {
        currentCountEnemy--;
    }

    private void Update()
    {
        if(currentTime >= delayToSpawn && currentCountEnemy < maxHeadCount)
        {
            currentTime = 0;
            Spawn();
        }

        currentTime += Time.deltaTime;
    }
    
    void Spawn()
    {
        Instantiate(headPrefab, GeneratePos(), Quaternion.identity);
        currentCountEnemy++;
    }

    Vector3 GeneratePos()
    {
        // Если нет ни одной точки спавна – используем запасную позицию перед игроком
        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            return playerController.transform.position - Vector3.forward * 15;
        }

        Transform closestPoint = null;
        float minDistance = float.MaxValue;
        Vector3 playerPos = playerController.transform.position;

        foreach (Transform point in spawnPoints)
        {
            float distance = Vector3.Distance(playerPos, point.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestPoint = point;
            }
        }

        // closestPoint всегда будет не null, так как массив не пуст
        return closestPoint != null ? closestPoint.position : playerPos - Vector3.forward * 15;

    }   

}
