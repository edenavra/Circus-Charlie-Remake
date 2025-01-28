using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pool;
using Unity.VisualScripting;
using UnityEngine.Pool;

public class FireRingSpawner : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float ringPerSecond = 0.5f;
    [SerializeField] private float minDistanceBetweenRings = 2f;
    [SerializeField] private FireRingPool fireRingPool;
    [SerializeField] private SmallFireRingPool smallFireRingPool;
    [SerializeField] private List<FireRingConfig> ringConfigs;
    
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

        if (fireRingPool == null || smallFireRingPool == null)
        {
            fireRingPool = FireRingPool.Instance;
            smallFireRingPool = SmallFireRingPool.Instance;
            Debug.Log("FireRingPool or SmallFireRingPool is missing in FireRingSpawner!");
            return;
        }
    }

    private void Update()
    {
        if (fireRingPool == null || smallFireRingPool == null)
        {
            fireRingPool = FireRingPool.Instance;
            smallFireRingPool = SmallFireRingPool.Instance;

            if (fireRingPool == null || smallFireRingPool == null)
            {
                Debug.LogWarning("Pools not initialized. Skipping this frame.");
                return;
            }
        }
        screenRightWorldX = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width+3, 0, 0)).x;
        _timer += Time.deltaTime;
        if (_timer >= 1f / ringPerSecond)
        {
            //Debug.Log("Spawning ring");
            SpawnFireRing();
            _timer -= 1f / ringPerSecond;
        }
    }
    
    /*private IEnumerator DelayedSpawn(FireRing ring, Vector3 position)
    {
        yield return new WaitForEndOfFrame(); // השהה עד לסוף הפריים
        ring.transform.position = position;
        ring.Reset();
    }*/
    private void SpawnFireRing()
    {
        FireRingConfig selectedConfig = GetRandomFireRingConfig();
        if (selectedConfig == null)
        {
            Debug.LogError("Failed to select FireRing: No valid config found!");
            return;
        }
        // בוחרים סוג טבעת וגובה
        /*int ringIndex = GetRandomFireRingIndex();
        if (ringIndex == -1)
        {
            Debug.LogError("Failed to select FireRing: Index out of range!");
            return;
        }

        float height = heights[ringIndex];*/
        float height = selectedConfig.height;
        Vector3 spawnPosition = new Vector3(screenRightWorldX, height, 0);
        // בדיקת מרחק מהטבעת האחרונה
        if (prevFireRing != null && Vector3.Distance(spawnPosition, prevFireRing.position) < minDistanceBetweenRings)
        {
            Debug.Log("Skipping spawn: rings would be too close.");
            return;
        }
        if(selectedConfig.fireRingType == FireRingType.Regular)
        {
            FireRing fireRing = fireRingPool.Get();
            if (fireRing == null || fireRing.GetFireRingType()!= selectedConfig.fireRingType)
            {
                Debug.LogError("Failed to spawn FireRing: Pool returned null!");
                return;
            }
            //StartCoroutine(DelayedSpawn(fireRing, spawnPosition));
            fireRing.transform.position = spawnPosition;
            fireRing.Reset();
            prevFireRing = fireRing.transform;
            return;
        }
        else
        {
            SmallFireRing smallFireRing = smallFireRingPool.Get();
            if (smallFireRing == null)
            {
                Debug.LogError("Failed to spawn SmallFireRing: Pool returned null!");
                return;
            }
            //StartCoroutine(DelayedSpawn(smallFireRing, spawnPosition));
            smallFireRing.transform.position = spawnPosition;
            smallFireRing.Reset();
            prevFireRing = smallFireRing.transform;
        }
        /*FireRing fireRing = selectedConfig.pool.Get();
        if (fireRing == null)
        {
            Debug.LogError("Failed to spawn FireRing: Pool returned null!");
            return;
        }

        fireRing.transform.position = spawnPosition;
        fireRing.Reset();*/

        //lastSpawnPosition = spawnPosition;
        //prevFireRing = fireRing.transform;
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
    private FireRingConfig GetRandomFireRingConfig()
    {
        float totalWeight = 0f;

        // חישוב המשקל הכולל
        foreach (var config in ringConfigs)
        {
            totalWeight += config.weight;
        }

        // בחירה רנדומלית על בסיס המשקלים
        float randomWeight = Random.Range(0, totalWeight);
        foreach (var config in ringConfigs)
        {
            if (randomWeight < config.weight)
            {
                return config;
            }
            randomWeight -= config.weight;
        }

        return null; // אם לא נמצא, החזר null (לא אמור לקרות אם המשקלים תקינים)
    }
}