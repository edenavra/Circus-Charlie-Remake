using System;
using System.Collections;
using System.Collections.Generic;
using FlamingPots;
using Pool;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public bool IsGameActive { get; set; } = false;
    
    [SerializeField] private Camera mainCamera;
    [SerializeField] private GameObject charlie;
    private Vector3 _firstCheckpointPosition;
    private Vector3 _lastCheckpointPosition;
    private List<FlamingPot> activePots = new List<FlamingPot>();
    
    public Camera MainCamera => mainCamera;
    public GameObject Charlie => charlie;
    
    [FormerlySerializedAs("scoreView")] [SerializeField] private UIView uiView;
    public UIModel _UIModel= new UIModel();
    private UIPresenter _uiPresenter; 
    
    private Coroutine bonusCoroutine;
    private bool isStageActive = false;
    
    
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
        _uiPresenter= new UIPresenter(_UIModel,uiView);
        if (_uiPresenter == null)
        {
            Debug.LogError("UIPresenter is not initialized!");
        }
       
    }

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
    public void StartGame()
    {
        //Debug.Log("Game Started!");
        IsGameActive = true; // מפעיל את המשחק
        //ScreenManager.Instance.ShowGameScreen();
        Time.timeScale = 1f;
        _uiPresenter.StartFlashing();
        StartStage();
    }
    
    public void PauseGame()
    {
        IsGameActive = false; // משהה את המשחק
        Time.timeScale = 0f;
        //ScreenManager.Instance.ShowStageScreen();
        _uiPresenter.StopFlashing();
        EndStage();
    }
    
    public void ResumeGame()
    {
        IsGameActive = true; // מפעיל את המשחק
        ScreenManager.Instance.ShowGameScreen();
        _uiPresenter.StartFlashing();
        StartStage();
        Time.timeScale = 1f;
    }
    public void StartStage()
    {
        if (bonusCoroutine != null)
            StopCoroutine(bonusCoroutine);
        
        isStageActive = true;
        bonusCoroutine = StartCoroutine(ReduceBonusOverTime());
    }

    public void EndStage()
    {
        if (bonusCoroutine != null)
            StopCoroutine(bonusCoroutine);

        isStageActive = false;
    }
    
    private IEnumerator ReduceBonusOverTime()
    {
        while (isStageActive)
        {
            _uiPresenter.ReduceBonusPoints(10);
            yield return new WaitForSeconds(0.5f); 
        }
    }
    
    
    public void UpdateLives(int lives)
    {
        if (_uiPresenter != null)
        {
            _uiPresenter.SetLives(lives);
        }
        else
        {
            Debug.LogError("UIPresenter is not initialized. Cannot update lives.");
        }
        
    }
    
    public void UpdateCheckpoint(Vector3 checkpointPosition)
    {
        _lastCheckpointPosition = checkpointPosition;
    }
    public void RegisterPot(FlamingPot pot)
    {
        if (!activePots.Contains(pot))
        {
            activePots.Add(pot);
        }
    }
    
    public void UnregisterPot(FlamingPot pot)
    {
        if (activePots.Contains(pot))
        {
            activePots.Remove(pot);
        }
    }
    
    public void DestroyPotsOnScreen()
    {
        Camera mainCamera = Camera.main;
        if (mainCamera == null) return;

        Vector3 screenBottomLeft = mainCamera.ScreenToWorldPoint(new Vector3(0, 0, 0));
        Vector3 screenTopRight = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));

        for (int i = activePots.Count - 1; i >= 0; i--)
        {
            FlamingPot pot = activePots[i];
            Vector3 potPosition = pot.transform.position;

            if (potPosition.x > screenBottomLeft.x && potPosition.x < screenTopRight.x &&
                potPosition.y > screenBottomLeft.y && potPosition.y < screenTopRight.y)
            {
                //Destroy(pot.gameObject);
                pot.gameObject.SetActive(false);
            }
        }
    }

    private void ResetPots()
    {
        for(int i = activePots.Count - 1; i >= 0; i--)
        {
            FlamingPot pot = activePots[i];
            pot.gameObject.SetActive(true);
        }
    }

    public void ResetLevel()
    {
        StartCoroutine(RestartFromCheckpoint());
    }
    
    private IEnumerator RestartFromCheckpoint()
    {
        PauseGame();
        ScreenManager.Instance.ShowStageScreen();
        // Display black screen
        //blackScreenCanvas.gameObject.SetActive(true);
        
        // Wait for 1 second
        yield return new WaitForSecondsRealtime(2);
        
        DestroyPotsOnScreen();        
        ResetAllRings();
        
        // Reset player position
        
        charlie.transform.position = _lastCheckpointPosition;
        _uiPresenter.SetLives(charlie.GetComponent<CharlieHealth>().GetCurrentLives());
        //UpdateLives(charlie.GetComponent<CharlieHealth>().GetCurrentLives());
        // Hide black screen
        //blackScreenCanvas.gameObject.SetActive(false);
        
        

        // Resume game
        ResumeGame();
    }

    public void Win()
    {
        StartCoroutine(HandleWin());
    }
    
    private IEnumerator HandleWin()
    {
        PauseGame();
        SoundManager.Instance.PlayWinSound(transform);
        yield return  StartCoroutine(AwardBonusPointsCoroutine());
        yield return new WaitForSecondsRealtime(1);
        //PauseGame();
        ScreenManager.Instance.ShowMainMenu();
        
        ResetGame();
    }

    public void GameOver()
    {
        StartCoroutine(HandleGameOver());
    }

    private IEnumerator HandleGameOver()
    {
        PauseGame();
        ScreenManager.Instance.ShowGameOver();
        yield return new WaitForSecondsRealtime(2);
        ResetGame();
    }
    private void ResetGame()
    {
        // Reset all rings
        ResetAllRings();
        
        // Reset all pots
        ResetPots();
        
        // Reset player position
        charlie.transform.position = _firstCheckpointPosition;
        
        // Reset UI
        _uiPresenter.ResetUI();
        
        //charlie.ResetHealth();
        // Reset game state
        IsGameActive = false;
    }
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

    
    public void AwardBonusPoints()
    {
        StartCoroutine(AwardBonusPointsCoroutine());
    }

    private IEnumerator AwardBonusPointsCoroutine()
    {
        while (_uiPresenter.GetBonusPoints() > 0)
        {
            _uiPresenter.ReduceBonusPoints(1);
            _uiPresenter.AddPoints(1);
            yield return new WaitForSecondsRealtime(0.005f); 
        }
    }
    
    public UIPresenter GetUIPresenter()
    {
        return _uiPresenter;
    }
    
    
}