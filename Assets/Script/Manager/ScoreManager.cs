using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance; // Singleton to access from other scripts

    [SerializeField] private TextMeshProUGUI scoreText; // Reference to the UI Text element
    [SerializeField] private TextMeshProUGUI highScoreText; // Reference to the high score UI in the Game Over Panel

    private int currentScore = 0; // Player's current score
    private int highScore = 0; // Player's current score

    [SerializeField] private int scoreVal = 5;
    [SerializeField] private int increamentScoreMultiplier = 2;

    private SnakeController snake;

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

        // Load the high score from PlayerPrefs
        highScore = PlayerPrefs.GetInt("HI Score", 0);
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
        UpdateHighScoreText();

        snake = FindObjectOfType<SnakeController>();
    }

    // Method to add score
    public void AddScore(int points)
    {
        currentScore += points;
        UpdateHighScore(); // Check and update high score if necessary
        UpdateScoreText();
    }

    // Method to add score
    public void ReduceScore(int points)
    {
        if(snake.GetSegmentOfSnakeBodyPartList().Count > snake.GetSnakeDefaultSize())
        {
            currentScore -= points;
            UpdateScoreText();
        }
    }

    // Method to reset score
    public void ResetScore()
    {
        currentScore = 0;
        UpdateScoreText();
    }

    // Update the score UI
    private void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score : " + currentScore;
        }
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
}
