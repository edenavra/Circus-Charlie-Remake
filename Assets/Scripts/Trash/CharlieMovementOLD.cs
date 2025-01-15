using UnityEngine;

public class CharlieMovementOLD : MonoBehaviour
{
    private static readonly int Speed = Animator.StringToHash("Speed");
    private static readonly int IsGrounded = Animator.StringToHash("IsGrounded");
    private static readonly int IsHurt = Animator.StringToHash("IsHurt");
    //[SerializeField] private LionMovement lionMovement;
    [SerializeField] private Animator animator;
    
    void Start()
    {
        FixedJoint2D joint = gameObject.AddComponent<FixedJoint2D>();
        //joint.connectedBody = lionMovement.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        //SyncAnimations();
    }

    /*private void SyncAnimations()
    {
        // סנכרון מהירות האנימציה עם האריה
        animator.SetFloat(Speed, lionMovement.GetComponent<Animator>().GetFloat(Speed));
        animator.SetBool(IsGrounded, lionMovement.GetComponent<Animator>().GetBool(IsGrounded));
    }*/
    
}
