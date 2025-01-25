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

    public Camera MainCamera => mainCamera;
    
    [FormerlySerializedAs("scoreView")] [SerializeField] private UIView uiView;
    private UIPresenter _uiPresenter;
    
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
        var scoreModel = new UIModel();
        _uiPresenter = new UIPresenter(scoreModel, uiView);
        _lastCheckpointPosition = charlie.transform.position;
        //Debug.Log($"Camera starting position: {Camera.main.transform.position}");
    }
    
    private void LateUpdate()
    {
        Debug.Log($"Camera position in LateUpdate: {Camera.main.transform.position}");
    }


    public void AddScore(int score)
    {
        _uiPresenter.AddPoints(score); 
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