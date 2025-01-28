using System;
using UnityEngine;

namespace Charlie
{
    public class CharlieShild : MonoBehaviour
    {
        public event Action<Collider2D> OnShieldActivated;
        public event Action<Collider2D> OnShieldDeactivated;
        [SerializeField] private GameObject shieldVisual;
        private bool _isShieldActive = false; 
        public float shieldDuration = 5f;
        private float _shieldTimer;
        private int _passCounter = 0;
    
        private Collider2D charlieCollider;
    
        private void Start()
        {
            charlieCollider = GetComponent<Collider2D>();
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
    
        public void ActivateShield()
        {
            if (_isShieldActive) return;
            OnShieldActivated?.Invoke(charlieCollider); 
            _isShieldActive = true;
            _shieldTimer = shieldDuration;
            shieldVisual.SetActive(true);
        }

        public void DeactivateShield()
        {
            OnShieldDeactivated?.Invoke(charlieCollider); 
            _isShieldActive = false;
            shieldVisual.SetActive(false);
        }
        
        public void AddPass()
        {
            _passCounter++;
            if (_passCounter >= 3)
            {
                ActivateShield();
                _passCounter = 0;
            }
        }
        
        public void ResetPassCounter()
        {
            _passCounter = 0;
        }

        public bool IsShieldActive()
        {
            return _isShieldActive;
        }
    }
}
