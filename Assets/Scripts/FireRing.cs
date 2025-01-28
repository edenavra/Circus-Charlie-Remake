using System;
using Pool;
using UnityEngine;
public enum FireRingType
{
    Regular,
    WithMoneySack
}
public class FireRing : MonoBehaviour, IPoolable
{
    //private GameObject charlie;
    private protected FireRingType ringType = FireRingType.Regular;
    //[SerializeField] private bool hasMoneySack = false;
    //[SerializeField] private MoneySack moneySack;
    
    private Rigidbody2D _rb;
    private bool hasCollided = false;
    private int jumpPoints = 100;
    private float ringSpeed = -1f;
    private Camera mainCamera;
    

    protected virtual void Start()
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
        /*if (hasMoneySack && moneySack == null)
        {
            Debug.LogError("Money sack is missing for a ring that should have one.");
        }*/
        //ringType = FireRingType.Regular;
    }

    private void Update()
    {
        if (mainCamera == null) return;

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

    public virtual void Reset()
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
        /*if (hasMoneySack && moneySack != null)
        {
            moneySack.ResetSack();
        }
        else if (hasMoneySack && moneySack == null)
        {
            Debug.LogWarning($"Ring of type {ringType} is missing its MoneySack reference!");
        }
        hasMoneySack = (ringType == FireRingType.WithMoneySack);*/

    }

    public virtual void OnEndOfScreen()
    {
        //_rb.simulated = false;
        //FireRingPool.Instance.Return(this);
        /*switch (ringType)
        {
            case FireRingType.Regular:
                if (FireRingPool.Instance != null)
                    FireRingPool.Instance.Return(this);
                else
                    Debug.LogError("FireRingPool.Instance is null!");
                break;
            case FireRingType.WithMoneySack:
                if (moneySack == null)
                {
                    Debug.LogError("Ring with MoneySack is missing its MoneySack reference!");
                }
                if (SmallFireRingPool.Instance != null)
                    SmallFireRingPool.Instance.Return(this);
                else
                    Debug.LogError("SmallFireRingPool.Instance is null!");
                break;
            default:
                Debug.LogError("Unknown FireRingType: Cannot return to pool");
                break;
        }*/
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
