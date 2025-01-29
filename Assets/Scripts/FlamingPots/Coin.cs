using UnityEngine;

namespace FlamingPots
{
    /// <summary>
    /// Represents a collectible coin that awards bonus points when collected by the player.
    /// </summary>
    public class Coin : MonoBehaviour
    {
        [SerializeField] private int bonusPoints = 5000; 
        [SerializeField] private Rigidbody2D rb;
        /// <summary>
        /// Ensures the coin has a Rigidbody2D component assigned.
        /// </summary>
        private void Awake()
        {
            if (rb == null)
            {
                rb = GetComponent<Rigidbody2D>();
                if (rb == null)
                {
                    Debug.LogError("No Rigidbody2D found! Make sure the coin has a Rigidbody2D component.");
                }
            }
        }
        /// <summary>
        /// Handles collision logic when the coin collides with the player.
        /// Awards bonus points and plays a collection sound before destroying itself.
        /// </summary>
        /// <param name="other">The collision event data.</param>
        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Charlie"))
            {
                GameManager.Instance.GetUIPresenter().AddPoints(bonusPoints);
                Destroy(gameObject); 
                SoundManager.Instance.PlaySound(SoundManager.SoundType.MoneyCollection, transform, false, 0, 3f);
            }
        }
    }
}

