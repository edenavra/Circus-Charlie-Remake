using System;
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    [SerializeField] private Camera mainCamera;
    [SerializeField] private GameObject charlie;
    private Vector3 _lastCheckpointPosition;

    public Camera MainCamera => mainCamera;
    
    [SerializeField] private ScoreView scoreView;
    private ScorePresenter scorePresenter;
    
    private void Awake()
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
        
        if(charlie == null)
        {
            Debug.LogError("No Charlie found! Make sure to assign the Charlie GameObject in the Inspector.");
        }
        
    }

    private void Start()
    {
        var scoreModel = new ScoreModel();
        scorePresenter = new ScorePresenter(scoreModel, scoreView);
        _lastCheckpointPosition = charlie.transform.position;
    }

    public void AddScore(int score)
    {
        scorePresenter.AddPoints(score); 
    }
    
    public void UpdateCheckpoint(Vector3 checkpointPosition)
    {
        _lastCheckpointPosition = checkpointPosition;
    }

    public void ResetLevel()
    {
        StartCoroutine(RestartFromCheckpoint());
    }
    
    private IEnumerator RestartFromCheckpoint()
    {
        // Display black screen
        //blackScreenCanvas.gameObject.SetActive(true);

        // Wait for 1 second
        yield return new WaitForSecondsRealtime(1);

        // Hide black screen
        //blackScreenCanvas.gameObject.SetActive(false);

        // Reset player position
        
        charlie.transform.position = _lastCheckpointPosition;

        // Resume game
        Time.timeScale = 1;
    }

    private void GameOver()
    {
        Debug.Log("Game Over!");
        // Optionally: Load a Game Over screen or restart the level
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}