using System.Collections;
using Charlie;
using UnityEngine;

public class CharlieHealth : MonoBehaviour
{
    private int _startingLives = 5;
    private int currentLives;
    private static readonly int IsHurt = Animator.StringToHash("IsHurt");
    private static readonly int Reset = Animator.StringToHash("Reset");
    [SerializeField] private Animator animator;
    private SoundManager soundManager;
    private bool _isTakingDamage = false;
    
    private void Start()
    {
        currentLives = _startingLives;
        soundManager = SoundManager.Instance;
        //GameManager.Instance.UpdateLives(currentLives);
    }
    
    public void TakeDamage()
    {
        if (_isTakingDamage) return;
        _isTakingDamage = true;

        animator.SetTrigger(IsHurt);
        soundManager.PlaySound(SoundManager.SoundType.Hit, transform, false, 0, 1f);
        StartCoroutine(HandleDamageAnimation());
    }
    
    private IEnumerator HandleDamageAnimation()
    {
        yield return new WaitForSecondsRealtime(0.5f); 
        soundManager.PlaySound(SoundManager.SoundType.LevelReset, transform, false, 0.5f, 3f);
        ReduceLife();
        _isTakingDamage = false;
    }
    
    public void ReduceLife()
    {
        if (currentLives > 0)
        {
            currentLives--;
            
            if (currentLives == 0)
            {
                HandleGameOver();
                animator.SetTrigger(Reset);
                //GetComponent<CharlieShild>().ResetPassCounter();
                ResetLives();
            }
            else
            {
                ReasetLevel();
                animator.SetTrigger(Reset);
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
        //soundManager.PlayLevelResetSound(transform);
        GameManager.Instance.GameOver();
    }
    private void ReasetLevel()
    {
//        print("level reaset");
        //soundManager.PlayLevelResetSound(transform);
        GameManager.Instance.ResetLevel();
    }
    
    public int getCurrentLives()
    {
        return currentLives;
    }
    
    private void ResetLives()
    {
        currentLives = _startingLives;
    }
}
