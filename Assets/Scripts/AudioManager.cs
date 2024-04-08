using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioSource audioSource;
    public AudioClip buttonOnHoverClip;
    public AudioClip buttonOnClickClip;
    public AudioClip foodConsumedClip;
    public AudioClip snakeMoveClip;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayButtonOnHoverSound()
    {
        PlaySound(buttonOnHoverClip);
    }

    public void PlayButtonOnClickSound()
    {
        PlaySound(buttonOnClickClip);
    }

    public void PlayFoodConsumedSound()
    {
        PlaySound(foodConsumedClip);
    }

    public void PlaySnakeMoveSound()
    {
        PlaySound(snakeMoveClip);
    }

    private void PlaySound(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
    }
}