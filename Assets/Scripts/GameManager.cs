using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private ScoreView scoreView;
    private ScorePresenter scorePresenter;

    private void Start()
    {
        var scoreModel = new ScoreModel();
        scorePresenter = new ScorePresenter(scoreModel, scoreView);
    }

    public void OnFlamingPotJump()
    {
        scorePresenter.AddPoints(200); // מוסיפים 200 נקודות בקפיצה מוצלחת
    }
}