using UnityEngine;

public class CharlieHealth : MonoBehaviour
{
    private int _startingLives = 5;
    private int currentLives;
    private static readonly int IsHurt = Animator.StringToHash("IsHurt");
    [SerializeField] private Animator animator;
    private SoundManager soundManager;
    private bool _isTakingDamage = false;
    
    private void Start()
    {
        currentLives = _startingLives;
        soundManager = SoundManager.Instance;
    }
    

    public void TakeDamage()
    {
        if (_isTakingDamage) return;
        _isTakingDamage = true;

        animator.SetTrigger(IsHurt);
        soundManager.PlayHitSound(transform);
        ReduceLife();

        _isTakingDamage = false;
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
        soundManager.PlayLevelResetSound(transform);
    }
}
