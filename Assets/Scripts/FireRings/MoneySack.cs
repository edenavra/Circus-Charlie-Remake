using UnityEngine;
using Random = UnityEngine.Random;

namespace FireRings
{
    public class MoneySack : MonoBehaviour
    {
        [SerializeField] private int bonusPoints = 500;

        private void Start()
        {
            ResetSack();
        }
        
        /// <summary>
        /// Handles collision detection with the player. 
        /// Awards bonus points and plays a collection sound before deactivating the object.
        /// </summary>
        /// <param name="other">The collider that triggered the event.</param>
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Charlie"))
            {
                // Randomly determines the bonus points value.
                if (Random.Range(0, 100) < 50)
                {
                    bonusPoints = 1000;
                }
                else
                {
                    bonusPoints = 500;
                }
                // Updates the UI with the new score.
                GameManager.Instance.GetUIPresenter().AddPoints(bonusPoints);
                // Plays the money collection sound.
                SoundManager.Instance.PlaySound(SoundManager.SoundType.MoneyCollection, transform, false, 0, 2f);
                // Deactivates the money sack.
                gameObject.SetActive(false);
            }
        }
        
        /// <summary>
        /// Resets the Money Sack by making it active again.
        /// </summary>
        public void ResetSack()
        {
            gameObject.SetActive(true);
        }
    }
}
