using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    [SerializeField] private ScoreView scoreView;
    private ScorePresenter scorePresenter;private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    
    

    private void Start()
    {
        var scoreModel = new ScoreModel();
        scorePresenter = new ScorePresenter(scoreModel, scoreView);
    }

    public void AddScore(int score)
    {
        scorePresenter.AddPoints(score); // מוסיפים 200 נקודות בקפיצה מוצלחת
    }
}