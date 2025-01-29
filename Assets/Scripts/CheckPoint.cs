using UnityEngine;

/// <summary>
/// Represents a checkpoint that updates the player's respawn position upon collision.
/// </summary>
public class CheckPoint : MonoBehaviour
{
    /// <summary>
    /// Detects when the player enters the checkpoint and updates their respawn position.
    /// </summary>
    /// <param name="other">The collider that triggered the checkpoint.</param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Charlie"))
        {
            GameManager.Instance.UpdateCheckpoint(transform.position);
        }
    }
}
