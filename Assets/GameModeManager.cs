using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameModeManager : MonoBehaviour
{
    public static GameModeManager Instance; // Singleton to access from other scripts

    [SerializeField] private GameMode currentGameMode = GameMode.SinglePlayer; // Default value: SinglePlayer

    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ChangeGameMode(GameMode mode)
    {
        currentGameMode = mode;

        if (currentGameMode == GameMode.SinglePlayer)
        {
            Debug.Log("Game mode changed to Single Player");
            // Additional setup for Single Player mode if needed
        }
        else if (currentGameMode == GameMode.Multiplayer)
        {
            Debug.Log("Game mode changed to Multiplayer");
            // Additional setup for Multiplayer mode if needed
        }
    }

    public GameMode GetCurrentMode()
    {
        return currentGameMode;
    }
}

// Enum for game modes
public enum GameMode { SinglePlayer, Multiplayer }