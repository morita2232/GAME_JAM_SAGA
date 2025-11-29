using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;

    [Header("Music")]
    public AudioClip musicTrack;

    [Range(0f, 1f)]
    public float volume = 0.3f;   // soft volume

    private AudioSource source;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // keep music when changing scenes
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        source = GetComponent<AudioSource>();
        SetupMusic();
    }

    private void SetupMusic()
    {
        source.clip = musicTrack;
        source.loop = true;           // loop forever
        source.volume = volume;       // set soft volume
        source.playOnAwake = true;    // start immediately
        source.Play();
    }
}

