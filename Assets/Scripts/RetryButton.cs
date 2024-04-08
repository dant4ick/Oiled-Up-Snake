using UnityEngine;
using UnityEngine.SceneManagement;

public class RetryButton : MonoBehaviour
{
    public void OnRetryButtonPressed()
    {
        // Get the level where the player last died
        int lastDiedLevel = PlayerPrefs.GetInt("lastDiedLevel", 1);
        
        AudioManager.instance.PlayButtonOnClickSound();
        
        // Load the level
        SceneManager.LoadScene(lastDiedLevel);
    }
}