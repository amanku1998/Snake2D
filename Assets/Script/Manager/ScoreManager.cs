using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance; // Singleton to access from other scripts

    [Header("Single Player UI")]
    [SerializeField] private TextMeshProUGUI scoreText; // Reference to the UI Text element
    [SerializeField] private TextMeshProUGUI highScoreText; // Reference to the high score UI in the Game Over Panel
    private int currentScore = 0; // Player's current score
    private int highScore = 0; // Player's current score

    [Header("Multiplayer UI")]
    [SerializeField] private TextMeshProUGUI player1ScoreText; // Reference to the UI Text element
    [SerializeField] private TextMeshProUGUI player2ScoreText; // Reference to the UI Text element

    private int player1CurrentScore = 0; // Player's current score
    private int player2CurrentScore = 0; // Player's current score

    [SerializeField] private int scoreVal = 5;
    [SerializeField] private int increamentScoreMultiplier = 2;

    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        if(GameModeManager.Instance.GetCurrentMode() == GameMode.SinglePlayer)
        {
            // Load the high score from PlayerPrefs
            highScore = PlayerPrefs.GetInt("HI Score", 0);
            UpdateHighScoreText();
        }
    }

    public int GetScoreVal()
    {
        return scoreVal;
    }

    public int GetIncreamentScoreMultiplierVal()
    {
        return increamentScoreMultiplier;
    }

    private void Start()
    {
        UpdateScoreText();
    }

    // Method to add score
    public void AddScore(int points, SnakeController snake)
    {
        if (GameModeManager.Instance.GetCurrentMode() == GameMode.SinglePlayer)
        {
            currentScore += points;
            UpdateHighScore(); // Check and update high score if necessary
            UpdateScoreText();
        }
        else if (GameModeManager.Instance.GetCurrentMode() == GameMode.Multiplayer)
        {
            if (snake.tag == "Snake1")
            {
                player1CurrentScore += points;
            }
            else if (snake.tag == "Snake2")
            {
                player2CurrentScore += points;
            }
            UpdateScoreText();
        }
    }

    // Method to Reduce score
    public void ReduceScore(int points , SnakeController snake)
    {
        if (snake.GetSegmentOfSnakeBodyPartList().Count > snake.GetSnakeDefaultSize())
        {
            if (GameModeManager.Instance.GetCurrentMode() == GameMode.SinglePlayer)
            {
                currentScore -= points;
                UpdateScoreText();
            }
            else if (GameModeManager.Instance.GetCurrentMode() == GameMode.Multiplayer)
            {
                if (snake.tag == "Snake1")
                {
                    player1CurrentScore -= points;
                    UpdateScoreText();
                }
                else if (snake.tag == "Snake2")
                {
                    player2CurrentScore -= points;
                    UpdateScoreText();
                }
            }
        }
    }

    // Method to reset score
    public void ResetScore()
    {
        if (GameModeManager.Instance.GetCurrentMode() == GameMode.SinglePlayer)
        {
            currentScore = 0;
        }
        else if (GameModeManager.Instance.GetCurrentMode() == GameMode.Multiplayer)
        {
            player1CurrentScore = 0;
            player2CurrentScore = 0;
        }
        UpdateScoreText();
    }

    // Update the score UI
    private void UpdateScoreText()
    {
        if (GameModeManager.Instance.GetCurrentMode() == GameMode.SinglePlayer)
        {
            if (scoreText != null)
            {
                scoreText.text = "Score : " + currentScore;
            }
        }
        else if(GameModeManager.Instance.GetCurrentMode() == GameMode.Multiplayer)
        {
            player1ScoreText.text = "Score : " + player1CurrentScore;
            player2ScoreText.text = "Score : " + player2CurrentScore;
        }
    }
    
    // Get the current score
    public int GetPlayer1CurrentScore(){    return player1CurrentScore; }  
    
    // Get the current score
    public int GetPlayer2CurrentScore(){    return player2CurrentScore; }

    // Update high score
    private void UpdateHighScore()
    {
        if (currentScore > highScore)
        {
            highScore = currentScore;

            // Save the high score to PlayerPrefs
            PlayerPrefs.SetInt("HI Score", highScore);
            PlayerPrefs.Save();

            highScoreText.text = "HI Score : " + currentScore;
        }
    }

    // Get the current score
    public int GetCurrentScore()
    {
        return currentScore;
    }

    // Get the current score
    public int GetHighScore()
    {
        return highScore;
    }

    // Update the score UI
    private void UpdateHighScoreText()
    {
        int highScore = PlayerPrefs.GetInt("HI Score");
        if (highScore != 0)
        {
            highScoreText.text = "HI Score : " + highScore;
        }
    }
}
