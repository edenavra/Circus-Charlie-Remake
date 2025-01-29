using System.Collections.Generic;
using Pool;
using UnityEngine;

namespace FireRings
{
    public class FireRingSpawner : MonoBehaviour
    {
        [SerializeField] private Camera mainCamera;
        [SerializeField] private float ringPerSecond = 0.5f;
        [SerializeField] private float minDistanceBetweenRings = 2f;
        [SerializeField] private FireRingPool fireRingPool;
        [SerializeField] private SmallFireRingPool smallFireRingPool;
        [SerializeField] private List<FireRingConfig> ringConfigs;
    
        private float _screenRightWorldX; 
        private float _timer = 0;
        private Transform _prevFireRing = null;
    
        private void Start()
        {
            if (mainCamera == null)
            {
                return;
            }

            if (fireRingPool == null || smallFireRingPool == null)
            {
                fireRingPool = FireRingPool.Instance;
                smallFireRingPool = SmallFireRingPool.Instance;
                return;
            }
        }

        private void Update()
        {
            if (!GameManager.Instance.IsGameActive) return;
            if (fireRingPool == null || smallFireRingPool == null)
            {
                fireRingPool = FireRingPool.Instance;
                smallFireRingPool = SmallFireRingPool.Instance;

                if (fireRingPool == null || smallFireRingPool == null)
                {
                    return;
                }
            }
            _screenRightWorldX = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width+3, 0, 0)).x;
            _timer += Time.deltaTime;
            if (_timer >= 1f / ringPerSecond)
            {
                SpawnFireRing();
                _timer -= 1f / ringPerSecond;
            }
        }
    
        /// <summary>
        /// Spawns a fire ring based on random selection from available configurations.
        /// Ensures proper spacing between consecutive fire rings.
        /// </summary>
        private void SpawnFireRing()
        {
            FireRingConfig selectedConfig = GetRandomFireRingConfig();
            if (selectedConfig == null)
            {
                return;
            }
        
            float height = selectedConfig.height;
            Vector3 spawnPosition = new Vector3(_screenRightWorldX, height, 0);
            if (_prevFireRing != null && Vector3.Distance(spawnPosition, _prevFireRing.position) < minDistanceBetweenRings)
            {
                return;
            }
            if(selectedConfig.fireRingType == FireRingType.Regular)
            {
                FireRing fireRing = fireRingPool.Get();
                if (fireRing == null || fireRing.GetFireRingType()!= selectedConfig.fireRingType)
                {
                    return;
                }
                fireRing.transform.position = spawnPosition;
                _prevFireRing = fireRing.transform;
                return;
            }
            else
            {
                SmallFireRing smallFireRing = smallFireRingPool.Get();
                if (smallFireRing == null)
                {
                    return;
                }
                smallFireRing.transform.position = spawnPosition;
                _prevFireRing = smallFireRing.transform;
            }
        }
    
        /// <summary>
        /// Selects a random fire ring configuration based on weighted probability.
        /// </summary>
        private FireRingConfig GetRandomFireRingConfig()
        {
            float totalWeight = 0f;
            foreach (var config in ringConfigs)
            {
                totalWeight += config.weight;
            }

            float randomWeight = Random.Range(0, totalWeight);
            foreach (var config in ringConfigs)
            {
                if (randomWeight < config.weight)
                {
                    return config;
                }
                randomWeight -= config.weight;
            }

            return null; 
        }
    }
}