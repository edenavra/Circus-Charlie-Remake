using System.Collections.Generic;
using Charlie;
using FlamingPots;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;
/// <summary>
/// Implements cheat codes for debugging and testing purposes.
/// Various key combinations trigger different game actions.
/// </summary>
public class CheatCodes : MonoBehaviour
{
    [SerializeField] private GameObject Charlie;
    [SerializeField] private GameObject Poduim;
    /// <summary>
    /// Monitors keyboard input and triggers cheat codes when specific key combinations are pressed.
    /// </summary>
    private void Update()
    {
        // Move Charlie to the podium when Shift + 1 is pressed.
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

        // Start the game when Shift + 2 is pressed.
        if (Keyboard.current.leftShiftKey.isPressed && Keyboard.current.digit2Key.wasPressedThisFrame)
        {
            ScreenManager.Instance.ShowGameScreen();
            GameManager.Instance.StartGame();
            Debug.Log("Game started!");
        }
        // Pause the game when Shift + 3 is pressed.
        if(Keyboard.current.leftShiftKey.isPressed && Keyboard.current.digit3Key.wasPressedThisFrame)
        {
            ScreenManager.Instance.ShowStageScreen();
            GameManager.Instance.PauseGame();
            Debug.Log("Game paused!");
        }
        
        // Activate Charlie's shield when Shift + 4 is pressed.
        if(Keyboard.current.leftShiftKey.isPressed && Keyboard.current.digit4Key.wasPressedThisFrame)
        {
            GameManager.Instance.Charlie.GetComponent<CharlieShild>().ActivateShield();
        }
        
        // Deactivate Charlie's shield when Shift + 5 is pressed.
        if (Keyboard.current.leftShiftKey.isPressed && Keyboard.current.digit5Key.wasPressedThisFrame)
        {
            GameManager.Instance.Charlie.GetComponent<CharlieShild>().DeactivateShield();
        }
        
        // End the stage and return to the main menu when Shift + 6 is pressed.
        if(Keyboard.current.leftShiftKey.isPressed && Keyboard.current.digit6Key.wasPressedThisFrame)
        {
            ScreenManager.Instance.ShowMainMenu();
            GameManager.Instance.EndStage();
            Debug.Log("Game ended!");
        }
        
        // Spawn coins from all active Flaming Pots when Shift + 7 is pressed.
        if (Keyboard.current.leftShiftKey.isPressed && Keyboard.current.digit7Key.wasPressedThisFrame)
        {
            List<FlamingPot> flamingPots = GameManager.Instance.GetActivePots();
            foreach (var flamingPot in flamingPots)
            {
                flamingPot.SpawnCoin();
            }
        }
        
        // Reset the game when Shift + 8 is pressed.
        if(Keyboard.current.leftShiftKey.isPressed && Keyboard.current.digit8Key.wasPressedThisFrame)
        {
            GameManager.Instance.ResetGame();
        }
    }
}