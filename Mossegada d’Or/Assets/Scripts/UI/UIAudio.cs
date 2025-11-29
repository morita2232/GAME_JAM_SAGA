using UnityEngine;

public class UIAudio : MonoBehaviour
{
    public static UIAudio Instance;

    [Header("Audio Clips")]
    public AudioClip hoverSound;
    public AudioClip clickSound;

    private AudioSource source;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        source = GetComponent<AudioSource>();
    }

    public void PlayHover()
    {
        source.PlayOneShot(hoverSound);
    }

    public void PlayClick()
    {
        source.PlayOneShot(clickSound);
    }
}

