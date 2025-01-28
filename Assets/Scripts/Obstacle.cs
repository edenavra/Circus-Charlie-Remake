using System;
using Charlie;
using UnityEngine;

public abstract class Obstacle : MonoBehaviour
{
    private Collider2D[] obstacleColliders;
    
    protected virtual void Start()
    {
        obstacleColliders = GetComponents<Collider2D>();
    }

    protected virtual void OnEnable()
    {
        if (GameManager.Instance?.Charlie == null) return; 
        var charlieShild = GameManager.Instance.Charlie.GetComponent<CharlieShild>();
        if (charlieShild == null) return; 

        charlieShild.OnShieldActivated += IgnoreCollisionWithPlayer;
        charlieShild.OnShieldDeactivated += RestoreCollisionWithPlayer;
    }

    protected virtual void OnDisable()
    {
        if (GameManager.Instance?.Charlie == null) return; 
        var charlieShild = GameManager.Instance.Charlie.GetComponent<CharlieShild>();
        if (charlieShild == null) return; 

        charlieShild.OnShieldActivated -= IgnoreCollisionWithPlayer;
        charlieShild.OnShieldDeactivated -= RestoreCollisionWithPlayer;
    }

    
    private void IgnoreCollisionWithPlayer(Collider2D playerCollider)
    {
        foreach (var collider in obstacleColliders)
        {
            if (!collider.isTrigger)
            {
                collider.enabled = false;
                Debug.Log($"{gameObject.name} is ignoring collision with {playerCollider.gameObject.name}");
            }
        }
    }
    
    private void RestoreCollisionWithPlayer(Collider2D playerCollider)
    {
        foreach (var collider in obstacleColliders)
        {
            if (!collider.isTrigger)
            {
                collider.enabled = true;
                Debug.Log($"{gameObject.name} restored collision with {playerCollider.gameObject.name}");
            }
        }
    }
}
