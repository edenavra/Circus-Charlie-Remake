using UnityEngine;

public class UIPresenter
{
    private readonly UIModel model;
    private readonly UIView view;

    public UIPresenter(UIModel model, UIView view)
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
