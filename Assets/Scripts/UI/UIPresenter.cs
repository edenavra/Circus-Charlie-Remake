using UnityEngine;

public class UIPresenter
{
    /// <summary>
    /// Manages the interaction between the UI model and the UI view, ensuring updates are reflected correctly.
    /// </summary>
    private readonly UIModel _model;
    private readonly UIView _view;
    
    private int _lastScore = -1;
    private int _lastLives = -1;
    private int _lastBonusPoints = -1;
    /// <summary>
    /// Initializes the UI presenter with a model and a view, updating the view immediately.
    /// </summary>
    public UIPresenter(UIModel model, UIView view)
    {
        this._model = model;
        this._view = view;
        UpdateView();
        StartFlashing();
    }
    /// <summary>
    /// Adds points to the model and updates the score in the view.
    /// </summary>
    public void AddPoints(int points)
    {
        _model.AddPoints(points);
        UpdateScore();
    }
    /// <summary>
    /// Sets the number of lives in the model and updates the view.
    /// </summary>
    public void SetLives(int lives)
    {
        _model.SetLives(lives);
        UpdateLives();
    }
    /// <summary>
    /// Reduces the bonus points in the model and updates the view.
    /// </summary>
    public void ReduceBonusPoints(int amount)
    {
        _model.ReduceBonusPoints(amount);
        UpdateBonusPoints();
    }
    /// <summary>
    /// Updates the entire UI view to reflect the current model state.
    /// </summary>
    private void UpdateView()
    {
        UpdateScore();
        UpdateLives();
        UpdateBonusPoints();
    }
    /// <summary>
    /// Updates the bonus points display if there has been a change.
    /// </summary>
    private void UpdateBonusPoints()
    {
        if (_model.BonusPoints != _lastBonusPoints)
        {
            _view.UpdateBonusPoints(_model.BonusPoints);
            _lastBonusPoints = _model.BonusPoints;
        }
    }
    /// <summary>
    /// Updates the score display if there has been a change.
    /// </summary>
    private void UpdateScore()
    {
        if (_model.Score != _lastScore)
        {
            _view.UpdateScore(_model.Score);
            _lastScore = _model.Score;
        }
    }
    /// <summary>
    /// Updates the lives display if there has been a change.
    /// </summary>
    public void UpdateLives()
    {
        if (_model.Lives != _lastLives)
        {
            _view.UpdateLives(_model.Lives);
            _lastLives = _model.Lives;
        }
    }
    /// <summary>
    /// Starts the UI flashing effect.
    /// </summary>
    public void StartFlashing()
    {
        _view.StartFlashing();
    }
    /// <summary>
    /// Stops the UI flashing effect.
    /// </summary>
    public void StopFlashing()
    {
        _view.StopFlashing();
    }
    /// <summary>
    /// Retrieves the current bonus points from the model.
    /// </summary>
    public int GetBonusPoints()
    {
        return _model.BonusPoints;
    }
    /// <summary>
    /// Resets the UI state to its initial values and updates the view.
    /// </summary>
    public void ResetUI()
    {
        _model.ResetUIState();
        UpdateView();
    }
}
