using System;
using Charlie;
using Pool;
using UnityEngine;
public enum FireRingType
{
    Regular,
    WithMoneySack
}
public class FireRing : Obstacle, IPoolable
{
    private protected FireRingType RingType = FireRingType.Regular;
    
    private Rigidbody2D _rb;
    private bool _hasCollided = false;
    private int _jumpPoinזts = 100;
    private float _ringSpeed = -1f;
    private Camera _mainCamera;
    
    /// <summary>
    /// Initializes the FireRing object and configures physics constraints.
    /// </summary>
    protected override void Start()
    {
        base.Start();
        _mainCamera = GameManager.Instance.MainCamera;
        if (_mainCamera == null)
        {
            return;
        }
        _rb = GetComponent<Rigidbody2D>();   
        _rb.gravityScale = 0;
        _rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        
    }

    private void Update()
    {
        if (_mainCamera == null) return;
        if (!GameManager.Instance.IsGameActive) return;

        Vector3 screenLeft = new Vector3(0, Screen.height / 2f, 0f);
        Vector3 worldLeft = _mainCamera.ScreenToWorldPoint(screenLeft);

        if (transform.position.x < worldLeft.x)
        {
            OnEndOfScreen();
        }
    }

    private void FixedUpdate()
    {
        if (_rb.linearVelocity.magnitude != _ringSpeed)
        {
            _rb.linearVelocity = new Vector2(_ringSpeed, 0f);
        }
        
    }
    
    /// <summary>
    /// Handles collisions with Charlie and walls, triggering health loss or returning to pool.
    /// </summary>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Charlie"))
        {
            CharlieShild shild = collision.gameObject.GetComponent<CharlieShild>();
            if (shild != null && shild.IsShieldActive())
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

        if (collision.gameObject.CompareTag("Wall"))
        {
            ReturnToPool();
        }
    }
    
    /// <summary>
    /// Handles logic when the player exits the fire ring's collider.
    /// Awards points if Charlie successfully jumps through the ring.
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
                GameManager.Instance.GetUIPresenter().AddPoints(_jumpPoinזts);
                CharlieShild shild = other.GetComponent<CharlieShild>();
                if (shild != null && !shild.IsShieldActive())
                {
                    shild.AddPass();
                }
            }
            _hasCollided = false;
            
        }
    }
    
    /// <summary>
    /// Resets FireRing state when reusing from the object pool.
    /// </summary>
    public virtual void Reset()
    {
        if (GameManager.Instance == null)
        {
            return;
        }
        _mainCamera = GameManager.Instance.MainCamera;
  
        if (_mainCamera == null )
        {
            return;
        }
        _hasCollided = false;
    }
    
    /// <summary>
    /// Handles when the fire ring reaches the end of the screen.
    /// </summary>
    public virtual void OnEndOfScreen()
    {
        ReturnToPool();
    }
    /// <summary>
    /// Returns the fire ring object back to the pool.
    /// </summary>
    protected virtual void ReturnToPool()
    {
        FireRingPool.Instance.Return(this);
    }
    /// <summary>
    /// Retrieves the type of the fire ring.
    /// </summary>
    public FireRingType GetFireRingType()
    {
        return RingType;
    }
}
