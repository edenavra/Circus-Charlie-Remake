using UnityEngine;
using UnityEngine.UI;

public class UIView : MonoBehaviour
{
    //[SerializeField] private Text scoreText;

    public void UpdateScore(int score)
    {
        //scoreText.text = $"Score: {score}";
        //להשתמש בשורה למעלה אחרי שאבנה את ה-ui
        Debug.Log($"Score updated: {score}");
    }
}