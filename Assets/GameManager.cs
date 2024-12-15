using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // Singleton to access from other scripts

    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TextMeshProUGUI scoreTextInGameOverPanel; // Reference to the high score UI in the Game Over Panel
    [SerializeField] private TextMeshProUGUI highScoreTextInGameOverPanel; // Reference to the high score UI in the Game Over Panel

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
        scoreTextInGameOverPanel.text = "Score : " + ScoreManager.Instance.GetCurrentScore();
        highScoreTextInGameOverPanel.text = "High Score : " + ScoreManager.Instance.GetHighScore();
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
