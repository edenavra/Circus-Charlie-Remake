using UnityEngine;

public class UIPresenter
{
    private readonly UIModel model;
    private readonly UIView view;
    
    private int lastScore = -1;
    private int lastLives = -1;
    private int lastBonusPoints = -1;

    public UIPresenter(UIModel model, UIView view)
    {
        this.model = model;
        this.view = view;
        UpdateView();
        StartFlashing();
    }

    public void AddPoints(int points)
    {
        model.AddPoints(points);
        UpdateScore();
    }
    
    public void ReduceLife()
    {
        model.ReduceLife();
        UpdateLives();
    }
    
    public void SetLives(int lives)
    {
        model.SetLives(lives);
        UpdateLives();
    }
    public void ReduceBonusPoints(int amount)
    {
        model.ReduceBonusPoints(amount);
        UpdateBonusPoints();
    }
    private void UpdateView()
    {
        UpdateScore();
        UpdateLives();
        UpdateBonusPoints();
    }
    
    private void UpdateBonusPoints()
    {
        if (model.BonusPoints != lastBonusPoints)
        {
            view.UpdateBonusPoints(model.BonusPoints);
            lastBonusPoints = model.BonusPoints;
        }
    }
    private void UpdateScore()
    {
        if (model.Score != lastScore)
        {
            view.UpdateScore(model.Score);
            lastScore = model.Score;
        }
    }

    public void UpdateLives()
    {
        if (model.Lives != lastLives)
        {
            view.UpdateLives(model.Lives);
            lastLives = model.Lives;
        }
    }

    public void ResetScore()
    {
        model.ResetScore();
        UpdateView();
    }
    
    public void StartFlashing()
    {
        view.StartFlashing();
    }

    public void StopFlashing()
    {
        view.StopFlashing();
    }
    
    public int GetBonusPoints()
    {
        return model.BonusPoints;
    }
}
