using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class LevelSelectionController : MonoBehaviour
{
    public GameObject buttonPrefab; // Assign this in the inspector
    public Sprite lockedButtonSprite; // Assign this in the inspector
    public string[] scenes; // Assign this in the inspector

    void Start()
    {
        // Save button sprite to use it later
        Sprite buttonSprite = buttonPrefab.GetComponent<Image>().sprite;
        
        for (int i = 1; i <= scenes.Length; i++)
        {
            GameObject buttonObj = Instantiate(buttonPrefab, gameObject.transform);
            buttonObj.name = "LevelButton" + (i);

            TMP_Text buttonText = buttonObj.GetComponentInChildren<TMP_Text>();
            buttonText.text = "Уровень " + (i);

            Button button = buttonObj.GetComponent<Button>();
            int levelIndex = i; // Capture index in local variable for delegate
            button.onClick.AddListener(() => LoadLevel(levelIndex));

            if (!FindObjectOfType<GameManager>().IsLevelUnlocked(i))
            {
                button.interactable = false;
                button.GetComponent<Image>().sprite = lockedButtonSprite;
            }
        }
    }

    void LoadLevel(int levelIndex)
    {
        AudioManager.instance.PlayButtonOnClickSound();
        SceneManager.LoadScene(scenes[levelIndex]);
    }
}