using UnityEngine;

using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private int bonusPoints = 500; // נקודות בונוס
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
            Debug.Log("Player collected the coin!");
            GameManager.Instance.AddScore(bonusPoints); // הוספת נקודות לשחקן
            Destroy(gameObject); // השמדת המטבע
        }
    }
}

