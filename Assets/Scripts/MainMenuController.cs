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

    private bool hasStarted = false;
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
        hasStarted = false;
    }

    private void Update()
    {
        // בדיקה אם השחקן לחץ על Enter (New Input System)
        if (Keyboard.current.enterKey.wasPressedThisFrame && !hasStarted)
        {
            hasStarted = true; // מונע לחיצות נוספות
            StartCoroutine(FlashAndStartGame());
        }
    }

    private IEnumerator FlashAndStartGame()
    {
        // הפעלת אפקט ההבהוב
        for (int i = 0; i < 5; i++)
        {
            flashingImage.enabled = true;
            yield return new WaitForSeconds(flashDuration / 2);
            flashingImage.enabled = false;
            yield return new WaitForSeconds(flashDuration / 2);
           
        }

        // הפעלת הסאונד
        SoundManager.Instance.PlayWinSound(transform);

        // מחכים מעט לפני המעבר לשלב
        yield return new WaitForSeconds(1f);

        // קריאה לפונקציה שתתחיל את המשחק
        ScreenManager.Instance.StartGameSequence();        
        
    }
    
}