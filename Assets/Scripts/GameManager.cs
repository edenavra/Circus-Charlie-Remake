using System;
using System.Collections;
using System.Collections.Generic;
using Charlie;
using FlamingPots;
using Pool;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Serialization;
/// <summary>
/// Manages the game's core mechanics, including checkpoints, active objects, UI updates, and game state transitions.
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public bool IsGameActive { get; set; } = false;
    
    [SerializeField] private Camera mainCamera;
    [SerializeField] private GameObject charlie;
    [FormerlySerializedAs("scoreView")] [SerializeField] private UIView uiView;
    private Vector3 _firstCheckpointPosition;
    private Vector3 _lastCheckpointPosition;
    private List<FlamingPot> _activePots = new List<FlamingPot>();
    private UIModel _uiModel= new UIModel();
    private UIPresenter _uiPresenter; 
    private Coroutine _bonusCoroutine;
    private bool _isStageActive = false;
    public Camera MainCamera => mainCamera;
    public GameObject Charlie => charlie;
    /// <summary>
    /// Initializes the singleton instance and UI components.
    /// </summary>
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
        if (uiView == null)
        {
            Debug.LogError("UIView is not assigned in GameManager. Please assign it in the Inspector.");
            return;
        }
        if(charlie == null)
        {
            Debug.LogError("No Charlie found! Make sure to assign the Charlie GameObject in the Inspector.");
        }
        _uiPresenter= new UIPresenter(_uiModel,uiView);
        if (_uiPresenter == null)
        {
            Debug.LogError("UIPresenter is not initialized!");
        }
    }
    // <summary>
    /// Sets up initial UI values, checkpoint, and game state.
    /// </summary>
    private void Start()
    {
        var scoreModel = new UIModel();
        _uiPresenter = new UIPresenter(scoreModel, uiView);
        _lastCheckpointPosition = charlie.transform.position;
        _firstCheckpointPosition = _lastCheckpointPosition;
        _uiPresenter.UpdateLives();
        Time.timeScale = 0f;
        IsGameActive = false;
        ScreenManager.Instance.ShowMainMenu();
    }
    /// <summary>
    /// Starts the game, resets time scale, and plays background music.
    /// </summary>
    public void StartGame()
    {
        IsGameActive = true; 
        Time.timeScale = 1f;
        SoundManager.Instance.PlayBackgroundMusic();
        _uiPresenter.StartFlashing();
        StartStage();
    }
    /// <summary>
    /// Pauses the game and stops background music.
    /// </summary>
    public void PauseGame()
    {
        IsGameActive = false;
        Time.timeScale = 0f;
        SoundManager.Instance.StopBackgroundMusic();
        _uiPresenter.StopFlashing();
        EndStage();
    }
    /// <summary>
    /// Resumes the game and restarts background music.
    /// </summary>
    public void ResumeGame()
    {
        IsGameActive = true; 
        ScreenManager.Instance.ShowGameScreen();
        _uiPresenter.StartFlashing();
        StartStage();
        Time.timeScale = 1f;
        SoundManager.Instance.PlayBackgroundMusic();
    }
    /// <summary>
    /// Starts a stage and begins reducing bonus points over time.
    /// </summary>
    public void StartStage()
    {
        if (_bonusCoroutine != null)
            StopCoroutine(_bonusCoroutine);
        
        _isStageActive = true;
        _bonusCoroutine = StartCoroutine(ReduceBonusOverTime());
    }
    /// <summary>
    /// Ends a stage and stops the bonus reduction coroutine.
    /// </summary>
    public void EndStage()
    {
        if (_bonusCoroutine != null)
            StopCoroutine(_bonusCoroutine);

        _isStageActive = false;
    }
    /// <summary>
    /// Reduces bonus points periodically while the stage is active.
    /// </summary>
    private IEnumerator ReduceBonusOverTime()
    {
        while (_isStageActive)
        {
            _uiPresenter.ReduceBonusPoints(10);
            yield return new WaitForSeconds(0.5f); 
        }
    }
    /// <summary>
    /// Updates the player's checkpoint position.
    /// </summary>
    public void UpdateCheckpoint(Vector3 checkpointPosition)
    {
        _lastCheckpointPosition = checkpointPosition;
    }
    
    /// <summary>
    /// Resets the game state and UI.
    /// </summary>
    public void ResetGame()
    {
        ResetAllRings();
        ResetPots();
        charlie.transform.position = _firstCheckpointPosition;
        _uiPresenter.ResetUI();
        IsGameActive = false;
    }
    /// <summary>
    /// Handles game over sequence and resets the game.
    /// </summary>
    public void GameOver()
    {
        StartCoroutine(HandleGameOver());
    }
    private IEnumerator HandleGameOver()
    {
        PauseGame();
        ScreenManager.Instance.ShowGameOver();
        yield return new WaitForSecondsRealtime(5);
        ResetGame();
    }
    /// <summary>
    /// Registers a flaming pot to the active pots list.
    /// </summary>
    public void RegisterPot(FlamingPot pot)
    {
        if (!_activePots.Contains(pot))
        {
            _activePots.Add(pot);
        }
    }
    /// <summary>
    /// Disables all flaming pots currently on screen.
    /// </summary>
    public void DestroyPotsOnScreen()
    {
        Camera mainCamera = Camera.main;
        if (mainCamera == null) return;

        Vector3 screenBottomLeft = mainCamera.ScreenToWorldPoint(new Vector3(0, 0, 0));
        Vector3 screenTopRight = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));

        for (int i = _activePots.Count - 1; i >= 0; i--)
        {
            FlamingPot pot = _activePots[i];
            Vector3 potPosition = pot.transform.position;

            if (potPosition.x > screenBottomLeft.x && potPosition.x < screenTopRight.x &&
                potPosition.y > screenBottomLeft.y && potPosition.y < screenTopRight.y)
            {
                pot.gameObject.SetActive(false);
            }
        }
    }
    /// <summary>
    /// Reactivates all registered flaming pots.
    /// </summary>
    private void ResetPots()
    {
        for(int i = _activePots.Count - 1; i >= 0; i--)
        {
            FlamingPot pot = _activePots[i];
            pot.gameObject.SetActive(true);
        }
    }
    /// <summary>
    /// Resets the game level by restarting from the last checkpoint.
    /// </summary>
    public void ResetLevel()
    {
        StartCoroutine(RestartFromCheckpoint());
    }
    /// <summary>
    /// Coroutine that handles restarting the level from the last checkpoint.
    /// </summary>
    private IEnumerator RestartFromCheckpoint()
    {
        PauseGame();
        ScreenManager.Instance.ShowStageScreen();
        yield return new WaitForSecondsRealtime(5);
        DestroyPotsOnScreen();        
        ResetAllRings();
        charlie.transform.position = _lastCheckpointPosition;
        _uiPresenter.SetLives(charlie.GetComponent<CharlieHealth>().GetCurrentLives());
        ResumeGame();
    }
    
    /// <summary>
    /// Handles the win sequence and awards bonus points before resetting the game.
    /// </summary>
    public void Win()
    {
        StartCoroutine(HandleWin());
    }
    /// <summary>
    /// Coroutine that plays the win animation and awards bonus points before restarting.
    /// </summary>
    private IEnumerator HandleWin()
    {
        PauseGame();
        SoundManager.Instance.PlaySound(SoundManager.SoundType.Clap, transform, false, 0, 100f);
        yield return new WaitForSecondsRealtime(3);
        yield return  StartCoroutine(AwardBonusPointsCoroutine());
        yield return new WaitForSecondsRealtime(1);
        ScreenManager.Instance.ShowMainMenu();
        ResetGame();
    }
    
    /// <summary>
    /// Resets all fire rings in the game.
    /// </summary>
    private void ResetAllRings()
    {
        if (FireRingPool.Instance == null)
        {
            Debug.LogError("FireRingPool.Instance is not initialized!");
        }
        else
        {
            var activeRings = FireRingPool.Instance.GetAllActiveObjects();
            foreach (var ring in activeRings)
            {
                ring.OnEndOfScreen();
            }
        }

        if (SmallFireRingPool.Instance == null)
        {
            Debug.LogError("SmallFireRingPool.Instance is not initialized!");
        }
        else
        {
            var activeRings = SmallFireRingPool.Instance.GetAllActiveObjects();
            foreach (var ring in activeRings)
            {
                ring.OnEndOfScreen();
            }
        }
        
    }

    
    /// <summary>
    /// Awards bonus points to the player over time.
    /// </summary>
    private IEnumerator AwardBonusPointsCoroutine()
    {
        while (_uiPresenter.GetBonusPoints() > 0)
        {
            if (_uiPresenter.GetBonusPoints() % 10 == 0)
            {
                SoundManager.Instance.PlaySound(SoundManager.SoundType.PointUp, transform, false, 0, 100f);
            }
            _uiPresenter.ReduceBonusPoints(1);
            _uiPresenter.AddPoints(1);
            yield return new WaitForSecondsRealtime(0.005f); 
        }
    }
    /// <summary>
    /// Retrieves the UI presenter instance.
    /// </summary>
    public UIPresenter GetUIPresenter()
    {
        return _uiPresenter;
    }
    /// <summary>
    /// Returns a list of active flaming pots.
    /// </summary>
    public List<FlamingPot> GetActivePots()
    {
        return _activePots;
    }
    
}