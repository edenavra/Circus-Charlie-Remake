using System;
using UnityEngine;

public class FlamingPot : MonoBehaviour
{
    [SerializeField] private GameObject charlie;
    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private Transform coinSpawnPoint;
    [SerializeField] private float launchForce = 5f; // עוצמת השיגור
    [SerializeField] private float rotationSpeed = 300f;
    private bool hasCollided = false;
    private int jumpCount = 0;
    private int jumpPoints = 200;
    private void Start()
    {
        if (charlie == null)
        {
            Debug.LogError("Charlie reference is missing in FlamingPot!");
        }
        if (coinPrefab == null)
        {
            Debug.LogError("Coin prefab is missing in FlamingPot! Please assign it in the Inspector.");
        }
        if (coinSpawnPoint == null)
        {
            Debug.LogError("Coin spawn point is missing in FlamingPot! Please assign it in the Inspector.");
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Charlie"))
        {
            hasCollided = true;
            CharlieHealth health = collision.gameObject.GetComponent<CharlieHealth>();
            if (health != null)
            {
                health.TakeDamage();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Charlie")) 
        {
            Collider2D triggerCollider = GetComponent<Collider2D>();
            Collider2D playerCollider = other.GetComponent<Collider2D>();
            
           Vector2 triggerMin = triggerCollider.bounds.min;
           Vector2 triggerMax = triggerCollider.bounds.max;
           Vector2 playerMin = playerCollider.bounds.min;
           Vector2 playerMax = playerCollider.bounds.max;
           
           if (!hasCollided && (playerMin.x > triggerMax.x || playerMax.x < triggerMin.x))
           {
               jumpCount++;
               GameManager.Instance.AddScore(jumpPoints);
               
               if (jumpCount >= 5)
               {
                   SpawnCoin();
                   jumpCount = 0; // אפס את מונה הקפיצות
               }
           }
           hasCollided = false;
        }
    }
    private void SpawnCoin()
    {
        
        if (coinPrefab != null && coinSpawnPoint != null)
        {
            GameObject coin = Instantiate(coinPrefab, coinSpawnPoint.position, Quaternion.identity);
            //coin
            Rigidbody2D coinRb = coin.GetComponent<Rigidbody2D>();
            if (coinRb != null)
            {
                Vector2 launchDirection = new Vector2(-Mathf.Cos(60 * Mathf.Deg2Rad), Mathf.Sin(60 * Mathf.Deg2Rad)); // 60 מעלות שמאלה
                coinRb.AddForce(launchDirection * launchForce, ForceMode2D.Impulse);
                coinRb.angularVelocity = rotationSpeed;
            }
            Debug.Log("Coin spawned!");
        }
    }
}
