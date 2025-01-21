using UnityEngine;
using Pool;

public class FireRingSpawner : MonoBehaviour
{
    [SerializeField] private Camera mainCamera; 
    [SerializeField] private float ringHeight = -0.41f; 
    [SerializeField] private float spawnInterval = 2f; 
    [SerializeField] private FireRingPool fireRingPool; 
    
    private float screenRightWorldX; 

    private void Start()
    {
        if (mainCamera == null)
        {
            Debug.LogError("Main Camera reference is missing in FireRingSpawner!");
            return;
        }

        if (fireRingPool == null)
        {
            Debug.LogError("FireRingPool reference is missing in FireRingSpawner!");
            return;
        }

        InvokeRepeating(nameof(SpawnFireRing), 0f, spawnInterval);
    }

    private void Update()
    {
        screenRightWorldX = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).x;
    }

    private void SpawnFireRing()
    {
        FireRing fireRing = fireRingPool.Get(); 
        if (fireRing == null)
        {
            Debug.LogError("Failed to spawn FireRing: Pool returned null!");
            return;
        }
        
        if (GameManager.Instance == null || GameManager.Instance.MainCamera == null)
        {
            Debug.LogError("GameManager references are not initialized. Skipping FireRing setup.");
            return;
        }
        Vector3 spawnPosition = new Vector3(screenRightWorldX, ringHeight, 0);
        fireRing.transform.position = spawnPosition;
        
        fireRing.Reset();
    }
}