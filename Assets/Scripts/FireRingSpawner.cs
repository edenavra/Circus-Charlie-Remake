using System.Collections.Generic;
using UnityEngine;
using Pool;
using Unity.VisualScripting;

public class FireRingSpawner : MonoBehaviour
{
    [SerializeField] private Camera mainCamera; 
   // [SerializeField] private float ringHeight = -0.41f; 
    [SerializeField] private float spawnInterval = 2f; 
    [SerializeField] private float minDistanceBetweenRings = 1.5f;
    [SerializeField] private FireRingPool fireRingPool; 
    [SerializeField] private List<FireRingPool> fireRingPools; 
    [SerializeField] private List<float> spawnWeights;
    [SerializeField] private List<float> heights;
    
    private float screenRightWorldX; 
    private Vector3 lastSpawnPosition = Vector3.positiveInfinity;
    
    private void Start()
    {
        if (mainCamera == null)
        {
            Debug.LogError("Main Camera reference is missing in FireRingSpawner!");
            return;
        }

        if (fireRingPools == null || fireRingPools.Count == 0 || spawnWeights == null || fireRingPools.Count != spawnWeights.Count || heights == null || fireRingPools.Count != heights.Count)
        {
            Debug.LogError("FireRingPools or spawnWeights are not properly configured!");
            return;
        }

        InvokeRepeating(nameof(SpawnFireRing), 0f, spawnInterval);
    }

    private void Update()
    {
        screenRightWorldX = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).x;
    }

    /*private void SpawnFireRing()
    {
        Vector3 spawnPosition = new Vector3(screenRightWorldX, ringHeight, 0);
        if (Vector3.Distance(spawnPosition, lastSpawnPosition) < minDistanceBetweenRings)
        {
            print("Skipping spawn: rings would be too close.");
            return;
        }
        
        FireRing fireRing = GetRandomFireRing(); 
        if (fireRing == null)
        {
            Debug.LogError("Failed to spawn FireRing: Pool returned null!");
            return;
        }
        
        fireRing.transform.position = spawnPosition;
        fireRing.Reset();
    }
    
    private FireRing GetRandomFireRing()
    {
        float totalWeight = 0f;
        for (int i = 0; i < spawnWeights.Count; i++)
        {
            totalWeight += spawnWeights[i];
        }

        float randomWeight = Random.Range(0, totalWeight);
        for (int i = 0; i < fireRingPools.Count; i++)
        {
            if (randomWeight < spawnWeights[i])
            {
                return fireRingPools[i].Get();
            }
            randomWeight -= spawnWeights[i];
        }

        return null; 
    }*/
    private void SpawnFireRing()
    {
        // בוחרים סוג טבעת וגובה
        int ringIndex = GetRandomFireRingIndex();
        if (ringIndex == -1)
        {
            Debug.LogError("Failed to select FireRing: Index out of range!");
            return;
        }

        float height = heights[ringIndex];

        Vector3 spawnPosition = new Vector3(screenRightWorldX, height, 0);

        // בדיקת מרחק מהטבעת האחרונה
        if (Vector3.Distance(spawnPosition, lastSpawnPosition) < minDistanceBetweenRings)
        {
            Debug.Log("Skipping spawn: rings would be too close.");
            return;
        }

        FireRing fireRing = fireRingPools[ringIndex].Get();
        if (fireRing == null)
        {
            Debug.LogError("Failed to spawn FireRing: Pool returned null!");
            return;
        }

        fireRing.transform.position = spawnPosition;
        fireRing.Reset();

        lastSpawnPosition = spawnPosition;
    }

    private int GetRandomFireRingIndex()
    {
        /* float totalWeight = 0f;
         for (int i = 0; i < spawnWeights.Count; i++)
         {
             totalWeight += spawnWeights[i];
         }

         float randomWeight = Random.Range(0, totalWeight);
         for (int i = 0; i < fireRingPools.Count; i++)
         {
             if (randomWeight < spawnWeights[i])
             {
                 return i;
             }
             randomWeight -= spawnWeights[i];
         }

         return -1; // לא אמור להגיע לכאן אם הכל מוגדר נכון
     }*/
        float random = Random.Range(0f, 1f);
        if (random > 0.2)
        {
            return 0;
        }
        else
        {
            return 1;
        }
    }
}