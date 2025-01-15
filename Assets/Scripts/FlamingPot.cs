using UnityEngine;

public class FlamingPot : MonoBehaviour
{
    [SerializeField] private GameObject lion;
    //[SerializeField] private GameObject charlie;
    private void Start()
    {
        if (lion == null)
        {
            Debug.LogError("Lion or Charlie reference is missing in FlamingPot!");
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Lion"))
        {
            CharlieHealth health = collision.gameObject.GetComponent<CharlieHealth>();
            if (health != null)
            {
                health.TakeDamage();
            }
        }
    }
}
