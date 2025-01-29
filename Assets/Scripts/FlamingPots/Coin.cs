using UnityEngine;

namespace FlamingPots
{
    public class Coin : MonoBehaviour
    {
        [SerializeField] private int bonusPoints = 5000; 
        [SerializeField] private Rigidbody2D rb;
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
    
        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Charlie"))
            {
                GameManager.Instance.GetUIPresenter().AddPoints(bonusPoints);
                Destroy(gameObject); 
                SoundManager.Instance.PlaySound(SoundManager.SoundType.MoneyCollection, transform, false, 0, 2f);
            }
        }
    }
}

