using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SettingManager : MonoBehaviour
{
    [SerializeField] private GameObject settingsPopup; // Reference to the settings panel

    private bool isGamePaused = false; // Tracks whether the game is paused

    [SerializeField] private Button resumeButton;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button quitButton;

    private void Start()
    {
        // Ensure the settings popup is hidden at the start
        settingsPopup.SetActive(false);

        resumeButton.onClick.AddListener(ResumeGame);
        restartButton.onClick.AddListener(RestartGame);
        quitButton.onClick.AddListener(QuitGame);
    }

    // Pause the game and display the settings popup
    public void DisplaySettingPopup()
    {
        Time.timeScale = 0; // Pause the game
        settingsPopup.SetActive(true); // Show the popup
        isGamePaused = true;
    }

    // Resume the game and hide the settings popup
    public void ResumeGame()
    {
        Time.timeScale = 1; // Resume the game
        settingsPopup.SetActive(false); // Hide the popup
        isGamePaused = false;
    }

    // Restart the game
    public void RestartGame()
    {
        Time.timeScale = 1; // Ensure the game is running
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Reload the current scene
    }

    // Quit the game
    public void QuitGame()
    {
        Time.timeScale = 1; // Ensure the game is running
        SceneManager.LoadScene(0); //GoTo menuScene
    }
}
