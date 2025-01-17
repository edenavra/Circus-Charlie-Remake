using UnityEngine;

public class ScorePresenter
{
    private readonly ScoreModel model;
    private readonly ScoreView view;

    public ScorePresenter(ScoreModel model, ScoreView view)
    {
        this.model = model;
        this.view = view;
        UpdateView();
    }

    public void AddPoints(int points)
    {
        model.AddPoints(points);
        UpdateView();
    }

    private void UpdateView()
    {
        view.UpdateScore(model.Score);
    }

    public void ResetScore()
    {
        model.ResetScore();
        UpdateView();
    }
}
