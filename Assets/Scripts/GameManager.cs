using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    [SerializeField] private Camera mainCamera;
    [SerializeField] private GameObject charlie;
    private Vector3 _lastCheckpointPosition;
    private List<FlamingPot> activePots = new List<FlamingPot>();
    private int currentLives = 5;

    public Camera MainCamera => mainCamera;
    
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
       
    }

    private void Start()
    {
        var scoreModel = new UIModel();
        _uiPresenter = new UIPresenter(scoreModel, uiView);
        _lastCheckpointPosition = charlie.transform.position;
        _uiPresenter.UpdateLives();
        _uiPresenter.StartFlashing();//todo: change this according to different screens
        StartStage();//todo: change this according to different screens
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
            yield return new WaitForSeconds(0.5f); // כל חצי שנייה
        }
    }


    public void AddScore(int score)
    {
        _uiPresenter.AddPoints(score); 
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
        //currentLives = lives;
        //UpdateUILives();
    }
    
    private void UpdateUILives()
    {
        if (uiView != null)
        {
            uiView.UpdateLives(currentLives);
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
                Destroy(pot.gameObject);
            }
        }
    }

    public void ResetLevel()
    {
        StartCoroutine(RestartFromCheckpoint());
    }
    
    private IEnumerator RestartFromCheckpoint()
    {
        //charlie.DisableMovement();
        Time.timeScale = 0f;
        // Display black screen
        //blackScreenCanvas.gameObject.SetActive(true);

        // Wait for 1 second
        yield return new WaitForSecondsRealtime(2);
        
        DestroyPotsOnScreen();        
        ResetAllRings();
        
        // Reset player position
        
        charlie.transform.position = _lastCheckpointPosition;
        
        UpdateLives(charlie.GetComponent<CharlieHealth>().GetCurrentLives());
        // Hide black screen
        //blackScreenCanvas.gameObject.SetActive(false);
        
        

        // Resume game
        Time.timeScale = 1;
    }

    private void GameOver()
    {
        Debug.Log("Game Over!");
        // Optionally: Load a Game Over screen or restart the level
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    

    private void ResetAllRings()
    {
        var rings = FireRingPool.Instance.GetAllActiveObjects();
        foreach (var ring in rings)
        {
            ring.OnEndOfScreen();
        }
        //var smallRings = SmallFireRingPool.Instance.GetAllActiveObjects();
    }
}