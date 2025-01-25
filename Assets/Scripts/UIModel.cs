using UnityEngine;

public class UIModel
{
    private int score;
    private int lives = 5;

    public int Score => score;
    public int Lives => lives;

    public void AddPoints(int points)
    {
        score += points;
    }

    public void ResetScore()
    {
        score = 0;
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
}
