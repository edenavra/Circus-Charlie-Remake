using System.Collections.Generic;
using UnityEngine;
using Pool;
using Unity.VisualScripting;

public class FireRingSpawner : MonoBehaviour
{
    [SerializeField] private Camera mainCamera; 
   // [SerializeField] private float ringHeight = -0.41f; 
    [SerializeField] private float ringPerSecond = 0.5f; 
    [SerializeField] private float minDistanceBetweenRings = 2f;
    [SerializeField] private FireRingPool fireRingPool; 
    [SerializeField] private List<FireRingPool> fireRingPools; 
    [SerializeField] private List<float> spawnWeights;
    [SerializeField] private List<float> heights;
    
    private float screenRightWorldX; 
    private Vector3 lastSpawnPosition = Vector3.positiveInfinity;
    private float _timer = 0;
    private Transform prevFireRing = null;
    
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
    }

    private void Update()
    {
        screenRightWorldX = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width+3, 0, 0)).x;
        _timer += Time.deltaTime;
        if (_timer >= 1f / ringPerSecond)
        {
            Debug.Log("Spawning ring");
            SpawnFireRing();
            _timer -= 1f / ringPerSecond;
        }
    }
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
        if (prevFireRing != null)
        {
            if (Vector3.Distance(spawnPosition, prevFireRing.position) < minDistanceBetweenRings)
            {
                Debug.Log("Skipping spawn: rings would be too close.");
                return;
            }
        }
        FireRing fireRing = fireRingPools[ringIndex].Get();
        if (fireRing == null)
        {
            Debug.LogError("Failed to spawn FireRing: Pool returned null!");
            return;
        }

        fireRing.transform.position = spawnPosition;
        fireRing.Reset();

        //lastSpawnPosition = spawnPosition;
        prevFireRing = fireRing.transform;
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