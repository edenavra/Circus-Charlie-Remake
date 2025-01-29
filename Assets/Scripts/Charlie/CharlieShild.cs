using System;
using UnityEngine;

namespace Charlie
{
    public class CharlieShild : MonoBehaviour
    {
        /// <summary>
        /// Event triggered when the shield is activated.
        /// </summary>
        public event Action<Collider2D> OnShieldActivated;
        /// <summary>
        /// Event triggered when the shield is deactivated.
        /// </summary>
        public event Action<Collider2D> OnShieldDeactivated;
        
        [SerializeField] private GameObject shieldVisual;
        
        /// <summary>
        /// Duration for which the shield remains active.
        /// </summary>
        public float shieldDuration = 5f;
        private float _shieldTimer;
        private int _passCounter = 0;
        private bool _isShieldActive = false; 
        private Collider2D _charlieCollider;
    
        private void Start()
        {
            _charlieCollider = GetComponent<Collider2D>();
            if (_charlieCollider == null)
            {
                Debug.LogError("CharlieShild: No Collider2D found! Make sure the GameObject has a Collider2D component.");
            }
            shieldVisual.SetActive(false);
        }
    
        private void Update()
        {
            if (_isShieldActive)
            {
                _shieldTimer -= Time.deltaTime; 

                if (_shieldTimer <= 0)
                {
                    DeactivateShield(); 
                }
            }
        }
        
        /// <summary>
        /// Activates the shield, making the character invulnerable for a duration.
        /// </summary>
        public void ActivateShield()
        {
            if (_isShieldActive) return;
            OnShieldActivated?.Invoke(_charlieCollider); 
            _isShieldActive = true;
            _shieldTimer = shieldDuration;
            shieldVisual.SetActive(true);
        }
        
        /// <summary>
        /// Deactivates the shield, making the character vulnerable again.
        /// </summary>
        public void DeactivateShield()
        {
            if (OnShieldDeactivated != null)
            {
                OnShieldDeactivated.Invoke(_charlieCollider);
            }
            else
            {
                Debug.LogWarning("CharlieShild: OnShieldDeactivated has no listeners.");
            }
            _isShieldActive = false;
            shieldVisual.SetActive(false);
        }
        
        /// <summary>
        /// Increments the pass counter. If it reaches the threshold, activates the shield.
        /// </summary>
        public void AddPass()
        {
            _passCounter++;
            if (_passCounter >= 3)
            {
                ActivateShield();
                _passCounter = 0;
            }
        }
        
        /// <summary>
        /// Resets the pass counter to zero.
        /// </summary>
        public void ResetPassCounter()
        {
            _passCounter = 0;
        }
        
        /// <summary>
        /// Checks whether the shield is currently active.
        /// </summary>
        public bool IsShieldActive()
        {
            return _isShieldActive;
        }
    }
}
