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
    private protected FireRingType ringType = FireRingType.Regular;
    
    private Rigidbody2D _rb;
    private bool hasCollided = false;
    private int jumpPoints = 100;
    private float ringSpeed = -1f;
    private Camera mainCamera;
    

    protected override void Start()
    {
        base.Start();
        mainCamera = GameManager.Instance.MainCamera;
        if (mainCamera == null)
        {
            return;
        }
        _rb = GetComponent<Rigidbody2D>();   
        _rb.gravityScale = 0;
        _rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        
    }

    private void Update()
    {
        if (mainCamera == null) return;
        if (!GameManager.Instance.IsGameActive) return;

        Vector3 screenLeft = new Vector3(0, Screen.height / 2f, 0f);
        Vector3 worldLeft = mainCamera.ScreenToWorldPoint(screenLeft);

        if (transform.position.x < worldLeft.x)
        {
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
            CharlieShild shild = collision.gameObject.GetComponent<CharlieShild>();
            if (shild != null && shild.IsShieldActive())
            {
                return;
            }
            if (shild != null)
            {
                shild.ResetPassCounter();
            }
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
                GameManager.Instance.GetUIPresenter().AddPoints(jumpPoints);
                CharlieShild shild = other.GetComponent<CharlieShild>();
                if (shild != null && !shild.IsShieldActive())
                {
                    shild.AddPass();
                }
            }
            hasCollided = false;
            
        }
    }

    public virtual void Reset()
    {
        if (GameManager.Instance == null)
        {
            return;
        }
        mainCamera = GameManager.Instance.MainCamera;
  
        if (mainCamera == null )
        {
            return;
        }
        hasCollided = false;
    }

    public virtual void OnEndOfScreen()
    {
        ReturnToPool();
    }
    protected virtual void ReturnToPool()
    {
        FireRingPool.Instance.Return(this);
    }
    
    public FireRingType GetFireRingType()
    {
        return ringType;
    }
}
