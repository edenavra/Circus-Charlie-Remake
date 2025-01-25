using UnityEngine;

public class UIModel
{
    private int score;

    public int Score => score;

    public void AddPoints(int points)
    {
        score += points;
    }

    public void ResetScore()
    {
        score = 0;
    }
}
