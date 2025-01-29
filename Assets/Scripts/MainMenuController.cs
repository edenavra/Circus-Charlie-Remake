using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private Image flashingImage;  // התמונה שצריכה להבהב
    [SerializeField] private float flashDuration = 0.3f; // משך ההבהוב
    //[SerializeField] private AudioClip startSound; // הסאונד שיתנגן
    private PlayerControls controls;
    private bool hasStarted = false;
    private void Awake()
    {
        controls = new PlayerControls();

        // חיבור הכפתור Enter
        controls.Menue.Submit.performed += ctx => OnEnterPressed();
    }
    private void Start()
    {
        // אם התמונה לא הופיעה, נגריל אותה
        if (flashingImage == null)
        {
            flashingImage = GetComponent<Image>();
        }
        flashingImage.enabled = false;
    }

    private void OnEnable()
    {
        controls.Enable();
        hasStarted = false;
    }
    
    private void OnDisable()
    {
        controls.Disable();
    }
    
    private void OnEnterPressed()
    {
        if (!hasStarted)
        {
            hasStarted = true;
            StartCoroutine(FlashAndStartGame());
        }
    }
    private IEnumerator FlashAndStartGame()
    {
        for (int i = 0; i < 5; i++)
        {
            flashingImage.enabled = true;
            yield return new WaitForSecondsRealtime(flashDuration / 2);
            flashingImage.enabled = false;
            yield return new WaitForSecondsRealtime(flashDuration / 2);
           
        }

        SoundManager.Instance.PlayWinSound(transform);

        yield return new WaitForSecondsRealtime(1f);

        ScreenManager.Instance.StartGameSequence();        
        
    }
    
}