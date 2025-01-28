using Charlie;
using UnityEngine;
using UnityEngine.InputSystem;

public class CheatCodes : MonoBehaviour
{
    [SerializeField] private GameObject Charlie;
    [SerializeField] private GameObject Poduim;

    private void Update()
    {
        // בדיקה אם Shift + 1 לחוצים
        if (Keyboard.current.leftShiftKey.isPressed && Keyboard.current.digit1Key.wasPressedThisFrame)
        {
            if (Charlie != null && Poduim != null)
            {
                Charlie.transform.position = Poduim.transform.position;
                Debug.Log("Charlie moved to podium!");
            }
            else
            {
                Debug.LogError("Charlie or Podium is not assigned!");
            }
        }

        // בדיקה אם Shift + 2 לחוצים
        if (Keyboard.current.leftShiftKey.isPressed && Keyboard.current.digit2Key.wasPressedThisFrame)
        {
            ScreenManager.Instance.ShowGameScreen();
            GameManager.Instance.StartGame();
            Debug.Log("Game started!");
        }
        
        if(Keyboard.current.leftShiftKey.isPressed && Keyboard.current.digit3Key.wasPressedThisFrame)
        {
            ScreenManager.Instance.ShowStageScreen();
            GameManager.Instance.PauseGame();
            Debug.Log("Game paused!");
        }
        
        if(Keyboard.current.leftShiftKey.isPressed && Keyboard.current.digit4Key.wasPressedThisFrame)
        {
            GameManager.Instance.Charlie.GetComponent<CharlieShild>().ActivateShield();
        }

        if (Keyboard.current.leftShiftKey.isPressed && Keyboard.current.digit5Key.wasPressedThisFrame)
        {
            GameManager.Instance.Charlie.GetComponent<CharlieShild>().DeactivateShield();
        }
        
    }
}