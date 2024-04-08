using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuController : MenuController
{
    // Get pause button from editor to hide it when it's pressed, and show it when resume button is pressed
    public GameObject pauseButton;
    public bool isPaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TogglePauseMenu();
        }
    }
    
    public void ResumeGame()
    {
        TogglePauseMenu();
    }

    public void TogglePauseMenu()
    {
        AudioManager.instance.PlayButtonOnClickSound();
        
        isPaused = !isPaused;
        gameObject.SetActive(isPaused);
        Time.timeScale = isPaused ? 0 : 1;
        pauseButton.SetActive(!isPaused);
    }
}