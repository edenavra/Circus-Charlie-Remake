using UnityEngine;
using UnityEngine.InputSystem;

namespace Charlie
{
    public class CharlieMovement : MonoBehaviour
    {
        private PlayerControls controls;
        private Vector2 moveInput;
        private float moveDirection = 0f;
        private static readonly int Speed = Animator.StringToHash("Speed");
        private static readonly int IsGrounded = Animator.StringToHash("IsGrounded");
        private static readonly int Win = Animator.StringToHash("Win");
        [SerializeField] private  float moveSpeed = 2f; 
        [SerializeField] private  float jumpForce = 5f; 
        [SerializeField] private  Animator animator; 
        private Rigidbody2D rb; 
        private bool isGrounded = true;
    
        private void Awake()
        {
            controls = new PlayerControls();
            rb = GetComponent<Rigidbody2D>();
        
            controls.Player.Move.performed += ctx => moveDirection = ctx.ReadValue<float>();
            //Debug.Log("Move Performed: " + moveDirection);
            controls.Player.Move.canceled += ctx => moveDirection = 0f;
            
            controls.Player.Jump.performed += ctx => Jump();

        }
        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            rb.freezeRotation = true;
        }
        private void OnEnable()
        {
            controls.Enable();
        }
        
        private void OnDisable()
        {
            controls.Disable();
        }
        void Update()
        {
            if (!GameManager.Instance.IsGameActive) return;
            HandleMovement();
        }
        private void Jump()
        {
            if (isGrounded)
            {
                rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                isGrounded = false;
                SoundManager.Instance.PlaySound(SoundManager.SoundType.Jump, transform, false, 0, 3f);
            }
            animator.SetBool(IsGrounded, isGrounded);
        }

        private void HandleMovement()
        {
            //Debug.Log($"MoveDirection: {moveDirection}");
            transform.position += new Vector3(moveDirection, 0, 0) * moveSpeed * Time.deltaTime;
            animator.SetFloat(Speed, moveDirection);
        }
    
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Floor"))
            {
                isGrounded = true;
                animator.SetBool(IsGrounded, isGrounded);
            }

            if (collision.gameObject.CompareTag("Podium"))
            { 
                animator.SetTrigger(Win);
                GameManager.Instance.Win();
            }
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Floor"))
            {
                isGrounded = false;
                animator.SetBool(IsGrounded, isGrounded);
            }
        }
    
    }
}
