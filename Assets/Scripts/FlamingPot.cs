using System;
using UnityEngine;

public class FlamingPot : MonoBehaviour
{
    [SerializeField] private GameObject charlie;
    [SerializeField] private GameManager gameManager;
    private bool hasCollided = false;
    private void Start()
    {
        if (charlie == null)
        {
            Debug.LogError("Charlie reference is missing in FlamingPot!");
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
               gameManager.OnFlamingPotJump();
           }
           hasCollided = false;
        }
    }
}
