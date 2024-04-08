using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private float levelStartTime;

    // Call this when the level starts
    public void StartLevel()
    {
        levelStartTime = Time.time;
    }

    // Call this when the player completes a level
    public void CompleteLevel()
    {
        // Get the current level index
        int currentLevelIndex = SceneManager.GetActiveScene().buildIndex;

        // Get the level the player has reached
        int levelReached = PlayerPrefs.GetInt("levelReached", 1);

        // Save the next level index only if it's greater than the level already reached
        if (currentLevelIndex + 1 > levelReached)
        {
            PlayerPrefs.SetInt("levelReached", currentLevelIndex + 1);
        }

        // Calculate the time it took to complete the level
        float levelCompletionTime = Time.time - levelStartTime;

        // Get the best time for the current level
        float bestTime = PlayerPrefs.GetFloat("bestTime" + currentLevelIndex, float.MaxValue);

        // If the level completion time is less than the best time, save it
        if (levelCompletionTime < bestTime)
        {
            PlayerPrefs.SetFloat("bestTime" + currentLevelIndex, levelCompletionTime);
        }
        
        // Load level complete scene
        SceneManager.LoadScene("LevelCompleteScene");
    }
    // Call this when the player fails a level
    public void GameOver()
    {
        // Save the current level index
        PlayerPrefs.SetInt("lastDiedLevel", SceneManager.GetActiveScene().buildIndex);

        // Load the game over scene
        SceneManager.LoadScene("GameOverScene");
    }

    // Call this when the game starts
    public void LoadPlayerProgress()
    {
        // Get the level the player has reached
        int levelReached = PlayerPrefs.GetInt("levelReached", 1);

        // Load the level
        SceneManager.LoadScene(levelReached);
    }

    // Call this to check if a level is unlocked
    public bool IsLevelUnlocked(int levelIndex)
    {
        // Get the level the player has reached
        int levelReached = PlayerPrefs.GetInt("levelReached", 1);

        // A level is unlocked if its index is less than or equal to the level reached
        return levelIndex <= levelReached;
    }
    
    public void ResetProgress()
    {
        AudioManager.instance.PlayButtonOnClickSound();
        // Set the level reached back to 1
        PlayerPrefs.SetInt("levelReached", 1);

        // Remove the last died level key
        PlayerPrefs.DeleteKey("lastDiedLevel");

        // Loop through all levels and reset the best time
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            PlayerPrefs.DeleteKey("bestTime" + i);
        }

        // Load the Levels scene
        SceneManager.LoadScene("LevelsScene");
    }
}