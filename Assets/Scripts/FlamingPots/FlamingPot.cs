using System;
using System.Collections;
using UnityEngine;

public class FlamingPot : MonoBehaviour
{
    //[SerializeField] private GameObject charlie;
    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private Transform coinSpawnPoint;
    [SerializeField] private float launchForce = 5f; // עוצמת השיגור
    [SerializeField] private float rotationSpeed = 300f;
    private bool hasCollided = false;
    private int jumpCount = 0;
    private int jumpPoints = 200;
    private bool isCameraReady = false;
    private Camera mainCamera;
    private void Start()
    {
        /*if (charlie == null)
        {
            Debug.LogError("Charlie reference is missing in FlamingPot!");
        }*/
        if (coinPrefab == null)
        {
            Debug.LogError("Coin prefab is missing in FlamingPot! Please assign it in the Inspector.");
        }
        if (coinSpawnPoint == null)
        {
            Debug.LogError("Coin spawn point is missing in FlamingPot! Please assign it in the Inspector.");
        }
        mainCamera = GameManager.Instance.MainCamera;
        GameManager.Instance.RegisterPot(this);
        //Debug.Log($"Pot starting at position: {transform.position}");
        StartCoroutine(WaitForCameraReady());

    }
    
    private IEnumerator WaitForCameraReady()
    {
        // המתן עד שהמצלמה תגיע למיקום שלה
        yield return new WaitForEndOfFrame(); // המתנה לפריים אחד
        yield return new WaitUntil(() => mainCamera.transform.position != Vector3.zero);

        isCameraReady = true;
//        Debug.Log("Camera is ready, starting to track pots.");
    }
    
    private void Update()
    {
        if (!isCameraReady || mainCamera == null) return;

        // חשב את גבול המסך השמאלי בעולם
        Vector3 screenLeft = new Vector3(0, Screen.height / 2f, mainCamera.nearClipPlane);
        Vector3 worldLeft = mainCamera.ScreenToWorldPoint(screenLeft);
        //Debug.Log($"World Left: {worldLeft.x}, Pot Position: {transform.position.x}");

         // בדוק האם הטבעת יצאה מגבול המסך + 3 מטר
        if (transform.position.x < worldLeft.x-3)
        {
            //Debug.Log($"Destroying pot at position: {transform.position}");
            Destroy(gameObject);
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
    
    private void OnDestroy()
    {
        // הסר את הסיר מה-GameManager
        if (GameManager.Instance != null)
        {
            GameManager.Instance.UnregisterPot(this);
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
               //GameManager.Instance.AddScore(jumpPoints);
               GameManager.Instance.GetUIPresenter().AddPoints(jumpPoints);
               
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
