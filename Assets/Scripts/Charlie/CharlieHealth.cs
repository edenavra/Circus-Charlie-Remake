using System.Collections;
using UnityEngine;

namespace Charlie
{
    public class CharlieHealth : MonoBehaviour
    {
        private readonly int _startingLives = 5;
        private int _currentLives;
        private static readonly int IsHurt = Animator.StringToHash("IsHurt");
        private static readonly int Reset = Animator.StringToHash("Reset");
        [SerializeField] private Animator animator;
        private SoundManager soundManager;
        private bool _isTakingDamage = false;
    
        private void Start()
        {
            _currentLives = _startingLives;
            soundManager = SoundManager.Instance;
        }
    
        /// <summary>
        /// Handles taking damage, triggering animations and sounds.
        /// Prevents damage if already in a damage state.
        /// </summary>
        public void TakeDamage()
        {
            if (_isTakingDamage) return;
            _isTakingDamage = true;

            animator.SetTrigger(IsHurt);
            soundManager.PlaySound(SoundManager.SoundType.Hit, transform, false, 0, 1f);
            StartCoroutine(HandleDamageAnimation());
        }
    
        /// <summary>
        /// Coroutine to manage damage animation delay and life reduction.
        /// </summary>
        private IEnumerator HandleDamageAnimation()
        {
            yield return new WaitForSecondsRealtime(0.5f); 
            soundManager.PlaySound(SoundManager.SoundType.LevelReset, transform, false, 0.5f, 3f);
            ReduceLife();
            _isTakingDamage = false;
        }
    
        /// <summary>
        /// Reduces the character's life count and handles game over or level reset.
        /// </summary>
        public void ReduceLife()
        {
            if (_currentLives > 0)
            {
                _currentLives--;
            
                if (_currentLives == 0)
                {
                    HandleGameOver();
                    animator.SetTrigger(Reset);
                    ResetLives();
                }
                else
                {
                    ResetLevel();
                    animator.SetTrigger(Reset);
                }
            }
        }
        
        /// <summary>
        /// Returns the current life count.
        /// </summary>
        public int GetCurrentLives()
        {
            return _currentLives;
        }
        
        /// <summary>
        /// Handles game over logic by calling the GameManager.
        /// </summary>
        private void HandleGameOver()
        {
            GameManager.Instance.GameOver();
        }
        
        /// <summary>
        /// Resets the level via the GameManager.
        /// </summary>
        private void ResetLevel()
        {
            GameManager.Instance.ResetLevel();
        }
        
        /// <summary>
        /// Resets the character's lives to the starting amount.
        /// </summary>
        private void ResetLives()
        {
            _currentLives = _startingLives;
        }
    }
}
