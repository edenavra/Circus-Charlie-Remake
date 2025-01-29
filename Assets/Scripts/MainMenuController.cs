using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class MainMenuController : MonoBehaviour
{
    /// <summary>
    /// Controls the main menu behavior, including flashing visuals and handling user input.
    /// </summary>
    [SerializeField] private Image flashingImage; 
    [SerializeField] private float flashDuration = 0.3f;
    private PlayerControls _controls;
    private bool _hasStarted = false;
    /// <summary>
    /// Initializes player controls and binds the Enter key to starting the game.
    /// </summary>
    private void Awake()
    {
        _controls = new PlayerControls();
        _controls.Menue.Submit.performed += ctx => OnEnterPressed();
    }
    /// <summary>
    /// Ensures the flashing image is assigned and initializes its state.
    /// </summary>
    private void Start()
    {
        if (flashingImage == null)
        {
            flashingImage = GetComponent<Image>();
        }
        flashingImage.enabled = false;
    }
    /// <summary>
    /// Enables input controls when the menu is active.
    /// </summary>
    private void OnEnable()
    {
        _controls.Enable();
        _hasStarted = false;
    }
    /// <summary>
    /// Disables input controls when the menu is deactivated.
    /// </summary>
    private void OnDisable()
    {
        _controls.Disable();
    }
    /// <summary>
    /// Handles the Enter key press event to start the game sequence.
    /// </summary>
    private void OnEnterPressed()
    {
        if (!_hasStarted)
        {
            _hasStarted = true;
            StartCoroutine(FlashAndStartGame());
        }
    }
    /// <summary>
    /// Coroutine that flashes the screen before starting the game.
    /// </summary>
    private IEnumerator FlashAndStartGame()
    {
        for (int i = 0; i < 5; i++)
        {
            flashingImage.enabled = true;
            yield return new WaitForSecondsRealtime(flashDuration / 2);
            flashingImage.enabled = false;
            yield return new WaitForSecondsRealtime(flashDuration / 2);
           
        }

        SoundManager.Instance.PlaySound(SoundManager.SoundType.Clap, transform, false, 0, 100f);

        yield return new WaitForSecondsRealtime(2f);

        ScreenManager.Instance.StartGameSequence();        
        
    }
    
}