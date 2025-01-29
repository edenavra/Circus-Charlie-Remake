using UnityEngine;
using UnityEngine.InputSystem;

namespace Charlie
{
    public class CharlieMovement : MonoBehaviour
    {
        private PlayerControls _controls;
        private Vector2 _moveInput;
        private float _moveDirection = 0f;
        private bool _isGrounded = true;
        private Rigidbody2D _rb; 

        private static readonly int Speed = Animator.StringToHash("Speed");
        private static readonly int IsGrounded = Animator.StringToHash("IsGrounded");
        private static readonly int Win = Animator.StringToHash("Win");
        
        [SerializeField] private  float moveSpeed = 2f; 
        [SerializeField] private  float jumpForce = 5f; 
        [SerializeField] private  Animator animator; 
        
    
        private void Awake()
        {
            _controls = new PlayerControls();
            _rb = GetComponent<Rigidbody2D>();
        
            _controls.Player.Move.performed += ctx => _moveDirection = ctx.ReadValue<float>();
            _controls.Player.Move.canceled += ctx => _moveDirection = 0f;
            _controls.Player.Jump.performed += ctx => Jump();

        }
        void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
            _rb.freezeRotation = true;
        }
        private void OnEnable()
        {
            if (_controls == null)
            {
                Debug.LogError("CharlieMovement: _controls is null in OnEnable(). Make sure it is initialized in Awake().");
                return;
            }
            _controls.Enable();
        }
        
        private void OnDisable()
        {
            _controls.Disable();
        }
        void Update()
        {
            if (!GameManager.Instance.IsGameActive) return;
            HandleMovement();
        }
        
        /// <summary>
        /// Handles the jump mechanic, allowing the player to jump if grounded.
        /// </summary>
        private void Jump()
        {
            if (_isGrounded)
            {
                _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, jumpForce); // משנה רק את Y
                _isGrounded = false;
                animator.SetBool(IsGrounded, _isGrounded);
                SoundManager.Instance.PlaySound(SoundManager.SoundType.Jump, transform, false, 0, 3f);
            }
        }
        
        /// <summary>
        /// Handles the movement of the player character based on input direction.
        /// </summary>
        private void HandleMovement()
        {
            _rb.linearVelocity = new Vector2(_moveDirection * moveSpeed, _rb.linearVelocity.y);
            animator.SetFloat(Speed, Mathf.Abs(_moveDirection));
        }
        
        /// <summary>
        /// Handles collision logic for grounding and win conditions.
        /// </summary>
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Floor"))
            {
                _isGrounded = true;
                animator.SetBool(IsGrounded, _isGrounded);
            }

            if (collision.gameObject.CompareTag("Podium"))
            { 
                animator.SetTrigger(Win);
                GameManager.Instance.Win();
            }
        }
        
        /// <summary>
        /// Handles logic for when the player exits a collision with the ground.
        /// </summary>
        private void OnCollisionExit2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Floor"))
            {
                _isGrounded = false;
                animator.SetBool(IsGrounded, _isGrounded);
            }
        }
    
    }
}
