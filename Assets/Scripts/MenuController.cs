using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public void LoadLevels()
    {
        AudioManager.instance.PlayButtonOnClickSound();
        // Load your levels scene
        SceneManager.LoadScene("LevelsScene");
    }

    public void QuitToDesktop()
    {
        // Quit the game
        Application.Quit();
    }
}