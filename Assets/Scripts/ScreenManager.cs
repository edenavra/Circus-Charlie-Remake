using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
/// <summary>
/// Manages the display of different UI screens in the game, including the main menu, game screen, stage screen, and game over screen.
/// </summary>
public class ScreenManager : MonoBehaviour
{
    public static ScreenManager Instance { get; private set; }

    [Header("Canvas References")]
    [SerializeField] private Canvas mainMenuCanvas;   
    [SerializeField] private Canvas stageCanvas;      
    [SerializeField] private Canvas gameCanvas;      
    [SerializeField] private Canvas gameOverCanvas;   
    /// <summary>
    /// Initializes the singleton instance of the ScreenManager.
    /// </summary>
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
    
    /// <summary>
    /// Displays the main menu screen and hides other screens.
    /// </summary>
    public void ShowMainMenu()
    {
        mainMenuCanvas.gameObject.SetActive(true);
        stageCanvas.gameObject.SetActive(false);
        gameCanvas.gameObject.SetActive(false);
        gameOverCanvas.gameObject.SetActive(false);
    }
    /// <summary>
    /// Displays the stage screen while keeping the game screen active, hiding others.
    /// </summary>
    public void ShowStageScreen()
    {
        stageCanvas.gameObject.SetActive(true);
        mainMenuCanvas.gameObject.SetActive(false);
        gameCanvas.gameObject.SetActive(true);
        gameOverCanvas.gameObject.SetActive(false);
    }
    /// <summary>
    /// Displays the game over screen and returns to the main menu after a delay.
    /// </summary>
    public void ShowGameOver()
    {
        gameCanvas.gameObject.SetActive(true);
        gameOverCanvas.gameObject.SetActive(true);
        StartCoroutine(ReturnToMainMenuAfterDelay(3f));
    }
    /// <summary>
    /// Returns to the main menu after a specified delay.
    /// </summary>
    private IEnumerator ReturnToMainMenuAfterDelay(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        ShowMainMenu();
    }
    /// <summary>
    /// Displays the main game screen, hiding all other screens.
    /// </summary>
    public void ShowGameScreen()
    { 
        mainMenuCanvas.gameObject.SetActive(false);
        gameCanvas.gameObject.SetActive(true);
        stageCanvas.gameObject.SetActive(false);
        gameOverCanvas.gameObject.SetActive(false);
    }
    /// <summary>
    /// Starts the game sequence, showing the stage screen first before transitioning to the game screen.
    /// </summary>
    public void StartGameSequence()
    {
        StartCoroutine(GameSequenceCoroutine());
    }
    
    /// <summary>
    /// Coroutine that displays the stage screen for a short duration before starting the game.
    /// </summary>
    private IEnumerator GameSequenceCoroutine()
    {
        ShowStageScreen();
        yield return new WaitForSecondsRealtime(2f);
        ShowGameScreen();
        GameManager.Instance.StartGame();
    }

}
