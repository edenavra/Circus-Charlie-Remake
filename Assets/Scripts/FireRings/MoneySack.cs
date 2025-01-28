using System;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class MoneySack : MonoBehaviour
{
    [SerializeField] private int bonusPoints = 500;
    SoundManager soundManager;

    private void Start()
    {
        soundManager = SoundManager.Instance;
        ResetSack();
    }

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
            //GameManager.Instance.AddScore(bonusPoints); 
            GameManager.Instance.GetUIPresenter().AddPoints(bonusPoints);
            soundManager.PlayMoneyCollectionSound(transform);
            gameObject.SetActive(false);
        }
    }

    public void ResetSack()
    {
        gameObject.SetActive(true);
    }
}
