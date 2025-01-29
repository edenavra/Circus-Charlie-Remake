using UnityEngine;
/// <summary>
/// Represents the UI model that tracks score, lives, and bonus points.
/// </summary>
public class UIModel
{
    private int score = 0;
    private int lives = 5;
    private int bonusPoints = 5000;
    /// <summary>
    /// Gets the current score.
    /// </summary>
    public int Score => score;
    /// <summary>
    /// Gets the current number of lives.
    /// </summary>
    public int Lives => lives;
    /// <summary>
    /// Gets the current bonus points.
    /// </summary>
    public int BonusPoints => bonusPoints;
    /// <summary>
    /// Adds the specified number of points to the score.
    /// </summary>
    /// <param name="points">The number of points to add.</param>
    public void AddPoints(int points)
    {
        score += points;
    }
    /// <summary>
    /// Resets the UI state to its initial values.
    /// </summary>
    public void ResetUIState()
    {
        score = 0; 
        lives = 5;
        bonusPoints = 5000; 
    }
    /// <summary>
    /// Sets the number of lives, ensuring it remains within the valid range.
    /// </summary>
    /// <param name="lives">The number of lives to set.</param>
    public void SetLives(int lives)
    {
        this.lives = Mathf.Clamp(lives, 0, 5); // הגבלת מספר החיים
    }
    /// <summary>
    /// Reduces the number of lives by one, ensuring it does not go below zero.
    /// </summary>
    public void ReduceLife()
    {
        if (lives > 0)
        {
            lives--;
        }
    }
    // <summary>
    /// Reduces the bonus points by the specified amount, ensuring it does not go below zero.
    /// </summary>
    /// <param name="amount">The amount to subtract from bonus points.</param>
    public void ReduceBonusPoints(int amount)
    {
        bonusPoints = Mathf.Max(bonusPoints - amount, 0);
    }
}
