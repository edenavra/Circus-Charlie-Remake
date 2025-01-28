using UnityEngine;

public class UIModel
{
    private int score = 0;
    private int lives = 5;
    private int bonusPoints = 5000;

    public int Score => score;
    public int Lives => lives;
    public int BonusPoints => bonusPoints;

    public void AddPoints(int points)
    {
        score += points;
    }

    public void ResetUIState()
    {
        score = 0; 
        lives = 5;
        bonusPoints = 5000; 
    }
    public void SetLives(int lives)
    {
        this.lives = Mathf.Clamp(lives, 0, 5); // הגבלת מספר החיים
    }

    public void ReduceLife()
    {
        if (lives > 0)
        {
            lives--;
        }
    }
    
    public void ReduceBonusPoints(int amount)
    {
        bonusPoints = Mathf.Max(bonusPoints - amount, 0);
    }
}
