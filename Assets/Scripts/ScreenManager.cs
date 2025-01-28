using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ScreenManager : MonoBehaviour
{
    public static ScreenManager Instance { get; private set; }

    [Header("Canvas References")]
    [SerializeField] private Canvas mainMenuCanvas;   // מסך הפתיחה
    [SerializeField] private Canvas stageCanvas;      // מסך "Stage 1"
    [SerializeField] private Canvas gameCanvas;       // מסך המשחק
    [SerializeField] private Canvas gameOverCanvas;   // מסך "Game Over"

    //[Header("Audio Clips")]
    //[SerializeField] private AudioClip startSound;  // סאונד פתיחה

    private bool gameStarted = false; 

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        
    }

    /*private void Update()
    {
        // מאפשר להתחיל את המשחק בלחיצת Enter (New Input System)
        if (Keyboard.current.enterKey.wasPressedThisFrame && !gameStarted)
        {
            StartGame();
        }
    }*/

    public void ShowMainMenu()
    {
        mainMenuCanvas.gameObject.SetActive(true);
        stageCanvas.gameObject.SetActive(false);
        gameCanvas.gameObject.SetActive(false);
        gameOverCanvas.gameObject.SetActive(false);
    }

    public void StartGame()
    {
        //mainMenuCanvas.gameObject.SetActive(false);
        gameStarted = true;
        ShowStageScreen();
    }

    public void ShowStageScreen()
    {
        stageCanvas.gameObject.SetActive(true);
        mainMenuCanvas.gameObject.SetActive(false);
        gameCanvas.gameObject.SetActive(true);
        gameOverCanvas.gameObject.SetActive(false);
        
    }

    public void ShowGameOver()
    {
        gameCanvas.gameObject.SetActive(true);
        gameOverCanvas.gameObject.SetActive(true);

        // אחרי 3 שניות חוזרים לתפריט הראשי
        StartCoroutine(ReturnToMainMenuAfterDelay(3f));
    }

    private IEnumerator ReturnToMainMenuAfterDelay(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        gameStarted = false;
        ShowMainMenu();
    }
    
    public void ShowGameScreen()
    { 
        mainMenuCanvas.gameObject.SetActive(false);
        gameCanvas.gameObject.SetActive(true);
        stageCanvas.gameObject.SetActive(false);
        gameOverCanvas.gameObject.SetActive(false);
    }
    
    public void StartGameSequence()
    {
        StartCoroutine(GameSequenceCoroutine());
    }

    private IEnumerator GameSequenceCoroutine()
    {
        // הצגת מסך השלב
        ShowStageScreen();

        // המתנה של 2 שניות
        yield return new WaitForSecondsRealtime(2f);

        // מעבר למסך המשחק והפעלת המשחק
        ShowGameScreen();
        GameManager.Instance.StartGame();
    }

}
