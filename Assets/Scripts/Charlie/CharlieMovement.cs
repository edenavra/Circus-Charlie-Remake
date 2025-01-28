using UnityEngine;

public class CharlieMovement : MonoBehaviour
{
    private static readonly int Speed = Animator.StringToHash("Speed");
    private static readonly int IsGrounded = Animator.StringToHash("IsGrounded");
    private static readonly int Win = Animator.StringToHash("Win");
    [SerializeField] private  float moveSpeed = 2f; 
    [SerializeField] private  float jumpForce = 5f; 
    [SerializeField] private  Animator animator; 
    private Rigidbody2D rb; 
    private bool isGrounded = true;
    private SoundManager soundManager;
    

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
        soundManager = SoundManager.Instance;
    }

    void Update()
    {
        if (!GameManager.Instance.IsGameActive) return;
        HandleMovement();
        HandleJump();
    }

    private void HandleMovement()
    {
        float moveDirection = Input.GetAxis("Horizontal");
        Vector3 movement = new Vector3(moveDirection, 0, 0) * moveSpeed * Time.deltaTime;
        transform.position += movement;
        animator.SetFloat(Speed, moveDirection);
    }

    private void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Z) && isGrounded)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            isGrounded = false; 
            soundManager.PlayCharlieJumpSound(transform);
        }

        animator.SetBool(IsGrounded, isGrounded);
       
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            
            isGrounded = true;
        }

        if (collision.gameObject.CompareTag("Podium"))
        { 
            animator.SetTrigger(Win);
            soundManager.PlayWinSound(transform);
            GameManager.Instance.AwardBonusPoints();
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            isGrounded = false;
        }
    }
    
}
