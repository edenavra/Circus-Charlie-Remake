using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIView : MonoBehaviour
{
    //[SerializeField] private Text scoreText;
    [SerializeField] private List<Image> lifeImages;

    public void UpdateScore(int score)
    {
        //scoreText.text = $"Score: {score}";
        //להשתמש בשורה למעלה אחרי שאבנה את ה-ui
        Debug.Log($"Score updated: {score}");
    }
    public void UpdateLives(int lives)
    {
        for (int i = 0; i < lifeImages.Count; i++)
        {
            lifeImages[i].enabled = i < lives-1;
        }
    }
}