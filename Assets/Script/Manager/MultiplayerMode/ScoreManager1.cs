using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ScoreManager1 : MonoBehaviour
{
    public static ScoreManager1 Instance; // Singleton to access from other scripts

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
    public void AddScore(int points, SnakeController1 snake)
    {
        if (snake.tag == "Snake1")
        {
            player1CurrentScore += points;
            UpdateScoreText();
        }
        else
        {
            player2CurrentScore += points;
            UpdateScoreText();
        }
    }

    // Method to add score
    public void ReduceScore(int points , SnakeController1 snake)
    {
        if (snake.GetSegmentOfSnakeBodyPartList().Count > snake.GetSnakeDefaultSize())
        {
            //if(snake.segmentOfSnakeBodyPartList.Count > initialSize)
            if (snake.tag == "Snake1")
            {
                player1CurrentScore -= points;
                UpdateScoreText();
            }
            else
            {
                player2CurrentScore -= points;
                UpdateScoreText();
            }
        }

    }

    // Method to reset score
    public void ResetScore()
    {
        player1CurrentScore = 0;
        player2CurrentScore = 0;
        UpdateScoreText();
    }

    // Update the score UI
    private void UpdateScoreText()
    {
        player1ScoreText.text = "Score : " + player1CurrentScore;
        player2ScoreText.text = "Score : " + player2CurrentScore;
    }
    
    // Get the current score
    public int GetPlayer1CurrentScore(){    return player1CurrentScore; }  
    
    // Get the current score
    public int GetPlayer2CurrentScore(){    return player2CurrentScore; }  
}
