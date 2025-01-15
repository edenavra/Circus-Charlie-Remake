using UnityEngine;

public class CharlieHealth : MonoBehaviour
{
    private int _startingLives = 5;
    private int currentLives;
    private static readonly int IsHurt = Animator.StringToHash("IsHurt");
    [SerializeField] private Animator animator;
    
    private void Start()
    {
        currentLives = _startingLives;
    }
    public void TakeDamage()
    {
        animator.SetTrigger(IsHurt);
        ReduceLife();
    }
    
    public void ReduceLife()
    {
        if (currentLives > 0)
        {
            currentLives--;
            print("Lives: " + currentLives);  
            
            if (currentLives == 0)
            {
                HandleGameOver();
            }
            else
            {
                ReasetLevel();
            }
        }
    }

    public void AddLives()
    {
        currentLives++;
    }
    
    public int GetCurrentLives()
    {
        return currentLives;
    }
    
    private void HandleGameOver()
    {
        Debug.Log("Game Over!");
        // כאן אפשר להוסיף לוגיקה להפסקת המשחק, מסך סיום וכו'.
    }
    private void ReasetLevel()
    {
        print("level reaset");
    }
}
