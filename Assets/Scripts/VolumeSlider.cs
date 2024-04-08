using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    private Slider volumeSlider;

    void Start()
    {
        volumeSlider = GetComponent<Slider>();
        // Set the slider value to the saved volume
        volumeSlider.value = PlayerPrefs.GetFloat("volume", 0.5f);
    }

    void Update()
    {
        AudioManager.instance.audioSource.volume = volumeSlider.value;
        // Save the volume value
        PlayerPrefs.SetFloat("volume", volumeSlider.value);
    }
}