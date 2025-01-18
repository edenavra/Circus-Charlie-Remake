using UnityEngine;

public class FireRing : MonoBehaviour
{
    [SerializeField] private GameObject charlie;
    private Rigidbody2D _rb;
    private bool hasCollided = false;
    private int jumpPoints = 100;
    private float ringSpeed = -1f;
    
    private void Start()
    {
        if (charlie == null)
        {
            Debug.LogError("Charlie reference is missing in FlamingPot!");
        }
        _rb = GetComponent<Rigidbody2D>();   
        _rb.gravityScale = 0;
        _rb.constraints = RigidbodyConstraints2D.FreezeRotation;
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
}
