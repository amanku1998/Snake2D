using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
public class GameManagerMultiplayer : MonoBehaviour
{
    public static GameManagerMultiplayer Instance; // Singleton to access from other scripts

    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TextMeshProUGUI gameResult;
    [SerializeField] private TextMeshProUGUI player1ScoreTextInGameOverPanel; // Reference to the high score UI in the Game Over Panel
    [SerializeField] private TextMeshProUGUI player2ScoreTextInGameOverPanel; // Reference to the high score UI in the Game Over Panel

    [SerializeField] private Button restartButton;
    [SerializeField] private Button menuButton;

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

        gameOverPanel.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        restartButton.onClick.AddListener(RestartGame);
        menuButton.onClick.AddListener(GoToMenu);
    }

    // Display high score in the Game Over Panel
    public void DisplayScore()
    {
        int player1Score = ScoreManager1.Instance.GetPlayer1CurrentScore();
        int player2Score = ScoreManager1.Instance.GetPlayer2CurrentScore();

        if(player1Score == player2Score)
        {
            gameResult.text = "Draw";
        }
        else if(player1Score > player2Score)
        {
            gameResult.text = "Player1 Win";
        }
        else
        {
            gameResult.text = "Player2 Win";
        }

        player1ScoreTextInGameOverPanel.text = "Player1 Score : " + player1Score;
        player2ScoreTextInGameOverPanel.text = "Player2 Score : " + player2Score;
    }

    public void DisplayGameOverPanel()
    {
        Time.timeScale = 0; // Ensure the game is running
        gameOverPanel.SetActive(true);

        DisplayScore();
    }

    // Restart the game
    public void RestartGame()
    {
        Time.timeScale = 1; // Ensure the game is running
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Reload the current scene
    }

    // Quit the game
    public void GoToMenu()
    {
        Time.timeScale = 1; // Ensure the game is running
        SceneManager.LoadScene(0); //GoTo menuScene
    }
}
