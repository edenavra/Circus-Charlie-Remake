using System.Collections;
using Charlie;
using UnityEngine;

namespace FlamingPots
{
    /// <summary>
    /// Represents a flaming pot obstacle that interacts with the player and can spawn coins.
    /// </summary>
    public class FlamingPot : Obstacle
    {
        [SerializeField] private GameObject coinPrefab;
        [SerializeField] private Transform coinSpawnPoint;
        [SerializeField] private float launchForce = 5f;
        [SerializeField] private float rotationSpeed = 300f;
        private bool _hasCollided = false;
        private int _jumpCount = 0;
        private readonly int _jumpPoints = 200;
        private bool _isCameraReady = false;
        private Camera _mainCamera;
        /// <summary>
        /// Initializes the FlamingPot, registers it in the GameManager, and waits for the camera to be ready.
        /// </summary>
        protected override void Start()
        {
            base.Start();
            if (coinPrefab == null)
            {
                Debug.LogError("Coin prefab is missing in FlamingPot! Please assign it in the Inspector.");
            }
            if (coinSpawnPoint == null)
            {
                Debug.LogError("Coin spawn point is missing in FlamingPot! Please assign it in the Inspector.");
            }
            _mainCamera = GameManager.Instance.MainCamera;
            GameManager.Instance.RegisterPot(this);
            StartCoroutine(WaitForCameraReady());

        }
        /// <summary>
        /// Resets internal values when the object is enabled.
        /// </summary>
        protected override void OnEnable()
        {
            base.OnEnable();
            _jumpCount = 0;
            _hasCollided = false;
        }
        
        /// <summary>
        /// Ensures the camera is properly positioned before enabling gameplay interactions.
        /// </summary>
        private IEnumerator WaitForCameraReady()
        {
            yield return new WaitForEndOfFrame();
            yield return new WaitUntil(() => _mainCamera.transform.position != Vector3.zero);

            _isCameraReady = true;
        }
        
        /// <summary>
        /// Handles the movement of the FlamingPot and disables it when it moves off-screen.
        /// </summary>
        private void Update()
        {
            if (!_isCameraReady || _mainCamera == null) return;

            Vector3 screenLeft = new Vector3(0, Screen.height / 2f, _mainCamera.nearClipPlane);
            Vector3 worldLeft = _mainCamera.ScreenToWorldPoint(screenLeft);
        
            if (transform.position.x < worldLeft.x-3)
            {
                gameObject.SetActive(false);
            }
        }
        
        /// <summary>
        /// Handles collision with the player, checking for shield activation and applying damage.
        /// </summary>
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Charlie"))
            {
                CharlieShild shild = collision.gameObject.GetComponent<CharlieShild>();
                if (shild != null && shild.IsShieldActive()) // אם המגן פעיל, התעלם
                {
                    return;
                }
                if (shild != null)
                {
                    shild.ResetPassCounter();
                }
                _hasCollided = true;
                CharlieHealth health = collision.gameObject.GetComponent<CharlieHealth>();
                if (health != null)
                {
                    health.TakeDamage();
                }
            
            }
        }
    
        /// <summary>
        /// Handles the player's exit from the trigger collider, adding jump points and spawning a coin if needed.
        /// </summary>
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
           
                if (!_hasCollided && (playerMin.x > triggerMax.x || playerMax.x < triggerMin.x))
                {
                    _jumpCount++;
                    CharlieShild shild = other.GetComponent<CharlieShild>();
                    if (shild != null && !shild.IsShieldActive())
                    {
                        shild.AddPass();
                    }
                    GameManager.Instance.GetUIPresenter().AddPoints(_jumpPoints);
               
               
                    if (_jumpCount >= 5)
                    {
                        SpawnCoin();
                        _jumpCount = 0; 
                    }
                }
                _hasCollided = false;
           
            }
        }
        /// <summary>
        /// Spawns a coin at a predefined spawn point and applies a launch force.
        /// </summary>
        public void SpawnCoin()
        {
        
            if (coinPrefab != null && coinSpawnPoint != null)
            {
                GameObject coin = Instantiate(coinPrefab, coinSpawnPoint.position, Quaternion.identity);
                Rigidbody2D coinRb = coin.GetComponent<Rigidbody2D>();
                if (coinRb != null)
                {
                    Vector2 launchDirection = new Vector2(-Mathf.Cos(60 * Mathf.Deg2Rad), Mathf.Sin(60 * Mathf.Deg2Rad)); 
                    coinRb.AddForce(launchDirection * launchForce, ForceMode2D.Impulse);
                    coinRb.angularVelocity = rotationSpeed;
                    SoundManager.Instance.PlaySound(SoundManager.SoundType.CoinSpawn, transform, false, 0, 50f);
                }
            }
        }
    }
}
