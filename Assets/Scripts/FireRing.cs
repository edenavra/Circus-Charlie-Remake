using System;
using Pool;
using UnityEngine;

public class FireRing : MonoBehaviour, IPoolable
{
    //private GameObject charlie;
    private Rigidbody2D _rb;
    private bool hasCollided = false;
    private int jumpPoints = 100;
    private float ringSpeed = -1f;
    private Camera mainCamera;
    

    private void Start()
    {
        mainCamera = GameManager.Instance.MainCamera;
        if (mainCamera == null)
        {
            Debug.LogError("Main Camera reference is missing in FireRing!");
            return;
        }
        
        _rb = GetComponent<Rigidbody2D>();   
        _rb.gravityScale = 0;
        _rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        
    }

    private void Update()
    {
        if (mainCamera == null) return;

        // חשב את גבול המסך השמאלי בעולם
        Vector3 screenLeft = new Vector3(0, Screen.height / 2f, 0f);
        Vector3 worldLeft = mainCamera.ScreenToWorldPoint(screenLeft);

        // בדוק האם הטבעת יצאה מגבול המסך
        if (transform.position.x < worldLeft.x)
        {
            // החזר לבריכה
            OnEndOfScreen();
        }
    }

    private void FixedUpdate()
    {
        if (_rb.linearVelocity.magnitude != ringSpeed)
        {
            _rb.linearVelocity = new Vector2(ringSpeed, 0f);
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
                GameManager.Instance.AddScore(jumpPoints);
            }
            hasCollided = false;
        }
    }

    public void Reset()
    {
        if (GameManager.Instance == null)
        {
            Debug.LogError("GameManager.Instance is null in FireRing.Reset");
            return;
        }
        mainCamera = GameManager.Instance.MainCamera;
  
        if (mainCamera == null )
        {
            Debug.LogError("FireRing.Reset: Missing references to Main Camera or Charlie");
            return;
        }
        hasCollided = false;
        //_rb.simulated = true;
    }

    public void OnEndOfScreen()
    {
        //_rb.simulated = false;
        FireRingPool.Instance.Return(this);
    }
}
