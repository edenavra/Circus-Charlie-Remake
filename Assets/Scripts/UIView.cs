using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI bonusText;
    [SerializeField] private List<Image> lifeImages;
    
    private bool isFlashing = false;
    private Coroutine flashingCoroutine;

    public void UpdateScore(int score)
    {
        string formattedScore = score.ToString("D6");
        scoreText.text = $"1P-{formattedScore} HI-020000 STAGE-01";
        //Debug.Log($"Score updated: {score}");1P-000000 HI-020000 STAGE-01
    }
    
    public void StartFlashing()
    {
        if (!isFlashing)
        {
            flashingCoroutine = StartCoroutine(Flash1P());
        }
    }
    public void StopFlashing()
    {
        if (isFlashing && flashingCoroutine != null)
        {
            StopCoroutine(flashingCoroutine);
        }

        isFlashing = false;

        if (scoreText != null)
        {
            scoreText.text = scoreText.text.Replace("   ", "1P-");
        }
    }
    private IEnumerator Flash1P()
    {
        isFlashing = true;

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
    
    public void UpdateLives(int lives)
    {
        for (int i = 0; i < lifeImages.Count; i++)
        {
            lifeImages[i].enabled = i < lives-1;
        }
    }
    
    public void UpdateBonusPoints(int bonusPoints)
    {
        string bonusStaticPart = "<color=#E91C63>BONUS-</color>"; 
        string bonusDynamicPart = $"<color=#FFFFFF>{bonusPoints:D4}</color>"; 

        bonusText.text = $"{bonusStaticPart}{bonusDynamicPart}";
    }
}