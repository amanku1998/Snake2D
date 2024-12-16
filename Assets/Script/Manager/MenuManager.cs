using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private Button SingleModeButton;
    [SerializeField] private Button MultiPlayerModeButton;
    [SerializeField] private Button QuitButton;


    // Start is called before the first frame update
    void Start()
    {
        SingleModeButton.onClick.AddListener(PlaySingleMode);
        MultiPlayerModeButton.onClick.AddListener(PlayMultiPlayerMode);
        QuitButton.onClick.AddListener(QuitGame);
    }

    private void PlaySingleMode()
    {
        SceneManager.LoadScene(1);
    }
    
    private void PlayMultiPlayerMode()
    {
        SceneManager.LoadScene(2);
    }

    private void QuitGame()
    {
        Application.Quit();
    }
}
