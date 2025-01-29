using System;
using Charlie;
using UnityEngine;
/// <summary>
/// Represents an abstract obstacle that interacts with the player's shield mechanics.
/// Handles enabling and disabling collisions when the player's shield is activated or deactivated.
/// </summary>
public abstract class Obstacle : MonoBehaviour
{
    private Collider2D[] _obstacleColliders;
    /// <summary>
    /// Initializes obstacle colliders.
    /// </summary>
    protected virtual void Start()
    {
        _obstacleColliders = GetComponents<Collider2D>();
    }
    /// <summary>
    /// Subscribes to shield activation events to handle collision interactions.
    /// </summary>
    protected virtual void OnEnable()
    {
        if (GameManager.Instance?.Charlie == null) return; 
        var charlieShild = GameManager.Instance.Charlie.GetComponent<CharlieShild>();
        if (charlieShild == null) return; 

        charlieShild.OnShieldActivated += IgnoreCollisionWithPlayer;
        charlieShild.OnShieldDeactivated += RestoreCollisionWithPlayer;
    }
    /// <summary>
    /// Unsubscribes from shield activation events when the obstacle is disabled.
    /// </summary>
    protected virtual void OnDisable()
    {
        if (GameManager.Instance?.Charlie == null) return; 
        var charlieShild = GameManager.Instance.Charlie.GetComponent<CharlieShild>();
        if (charlieShild == null) return; 

        charlieShild.OnShieldActivated -= IgnoreCollisionWithPlayer;
        charlieShild.OnShieldDeactivated -= RestoreCollisionWithPlayer;
    }

    /// <summary>
    /// Disables collisions with the player when their shield is activated.
    /// </summary>
    /// <param name="playerCollider">The player's collider.</param>    
    private void IgnoreCollisionWithPlayer(Collider2D playerCollider)
    {
        foreach (var collider in _obstacleColliders)
        {
            if (!collider.isTrigger)
            {
                collider.enabled = false;
            }
        }
    }
    /// <summary>
    /// Restores collisions with the player when their shield is deactivated.
    /// </summary>
    /// <param name="playerCollider">The player's collider.</param>
    private void RestoreCollisionWithPlayer(Collider2D playerCollider)
    {
        foreach (var collider in _obstacleColliders)
        {
            if (!collider.isTrigger)
            {
                collider.enabled = true;
            }
        }
    }
}
