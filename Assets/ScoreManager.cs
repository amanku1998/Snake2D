using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance; // Singleton to access from other scripts

    [SerializeField] private TextMeshProUGUI scoreText; // Reference to the UI Text element
    private int currentScore = 0; // Player's current score

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
    }

    private void Start()
    {
        UpdateScoreText();
    }

    // Method to add score
    public void AddScore(int points)
    {
        currentScore += points;
        UpdateScoreText();
    }

    // Method to add score
    public void ReduceScore(int points)
    {
        currentScore -= points;
        UpdateScoreText();
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

    // Get the current score
    public int GetCurrentScore()
    {
        return currentScore;
    }
}
