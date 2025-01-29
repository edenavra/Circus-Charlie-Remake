using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages the user interface elements such as score, lives, and bonus points display.
/// </summary>
public class UIView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI bonusText;
    [SerializeField] private List<Image> lifeImages;
    
    private bool _isFlashing = false;
    private Coroutine _flashingCoroutine;
    /// <summary>
    /// Updates the displayed score in a formatted string.
    /// </summary>
    /// <param name="score">The player's current score.</param>
    public void UpdateScore(int score)
    {
        string formattedScore = score.ToString("D6");
        scoreText.text = $"1P-{formattedScore} HI-020000 STAGE-01";
    }
    /// <summary>
    /// Starts the flashing animation of the score display.
    /// </summary>
    public void StartFlashing()
    {
        if (!_isFlashing)
        {
            _flashingCoroutine = StartCoroutine(Flash1P());
        }
    }
    /// <summary>
    /// Stops the flashing animation of the score display and resets the text.
    /// </summary>
    public void StopFlashing()
    {
        if (_isFlashing && _flashingCoroutine != null)
        {
            StopCoroutine(_flashingCoroutine);
        }

        _isFlashing = false;

        if (scoreText != null)
        {
            scoreText.text = scoreText.text.Replace("   ", "1P-");
        }
    }
    /// <summary>
    /// Coroutine that alternates the score display between "1P-" and blank for a flashing effect.
    /// </summary>
    private IEnumerator Flash1P()
    {
        _isFlashing = true;

        while (true)
        {
            if (scoreText != null)
            {
                if (scoreText.text.StartsWith("1P-"))
                {
                    scoreText.text = scoreText.text.Replace("1P-", "   ");
                }
                else
                {
                    scoreText.text = scoreText.text.Replace("   ", "1P-");
                }
            }

            yield return new WaitForSeconds(0.5f);
        }
    }
    /// <summary>
    /// Updates the displayed number of lives based on the given value.
    /// </summary>
    /// <param name="lives">The number of lives to display.</param>
    public void UpdateLives(int lives)
    {
        for (int i = 0; i < lifeImages.Count; i++)
        {
            lifeImages[i].enabled = i < lives-1;
        }
    }
    /// <summary>
    /// Updates the bonus points display with formatted text.
    /// </summary>
    /// <param name="bonusPoints">The current bonus points value.</param>
    public void UpdateBonusPoints(int bonusPoints)
    {
        string bonusStaticPart = "<color=#E91C63>BONUS</color>"; 
        string bonusDynamicPart = $"<color=#FFFFFF>-{bonusPoints:D4}</color>"; 

        bonusText.text = $"{bonusStaticPart}{bonusDynamicPart}";
    }
}