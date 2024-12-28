using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // Singleton to access from other scripts
    [SerializeField] private GameObject gameOverPanel;

    [Header("Single Player UI")]
    [SerializeField] private TextMeshProUGUI scoreTextInGameOverPanel;
    [SerializeField] private TextMeshProUGUI highScoreTextInGameOverPanel;

    [Header("Multiplayer UI")]
    [SerializeField] private TextMeshProUGUI gameResultText;
    [SerializeField] private TextMeshProUGUI player1ScoreTextInGameOverPanel; // Reference to the high score UI in the Game Over Panel
    [SerializeField] private TextMeshProUGUI player2ScoreTextInGameOverPanel; // Reference to the high score UI in the Game Over Panel

    [Header("Buttons")]
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
        if(GameModeManager.Instance.GetCurrentMode() == GameMode.SinglePlayer)
        {
            scoreTextInGameOverPanel.text = "Score : "+ ScoreManager.Instance.GetCurrentScore();
            highScoreTextInGameOverPanel.text = "High Score : " + ScoreManager.Instance.GetHighScore();
        }
        else if(GameModeManager.Instance.GetCurrentMode() == GameMode.Multiplayer)
        {
            int player1Score = ScoreManager.Instance.GetPlayer1CurrentScore();
            int player2Score = ScoreManager.Instance.GetPlayer2CurrentScore();

            if (player1Score == player2Score)
            {
                gameResultText.text = "Draw";
            }
            else if (player1Score > player2Score)
            {
                gameResultText.text = "Player1 Win";
            }
            else
            {
                gameResultText.text = "Player2 Win";
            }

            player1ScoreTextInGameOverPanel.text = "Player1 Score : " + player1Score;
            player2ScoreTextInGameOverPanel.text = "Player2 Score : " + player2Score;
        }
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
