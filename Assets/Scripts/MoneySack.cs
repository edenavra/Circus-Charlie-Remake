using Unity.VisualScripting;
using UnityEngine;

public class MoneySack : MonoBehaviour
{
    [SerializeField] private int bonusPoints = 500;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Charlie"))
        {
            if (Random.Range(0, 100) < 50)
            {
                bonusPoints = 1000;
            }
            else
            {
                bonusPoints = 500;
            }
            Debug.Log("Player collected the coin!");
            GameManager.Instance.AddScore(bonusPoints); 
            Destroy(gameObject); 
        }
    }
}
