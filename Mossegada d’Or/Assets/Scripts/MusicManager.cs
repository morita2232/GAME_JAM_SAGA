using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;

    [Header("Music Tracks")]
    public AudioClip gameplayMusic;
    public AudioClip gamblingMusic;

    [Header("Scene Names")]
    public string gameplaySceneName = "GameplayScene";
    public string gamblingSceneName = "GamblingScene";

    [Range(0f, 1f)]
    public float volume = 0.3f;

    private AudioSource source;

    private void Awake()
    {
        // Singleton + keep across scenes
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        source = GetComponent<AudioSource>();

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    private void Start()
    {
        // Play the correct music for the first scene
        PlayMusicForScene(SceneManager.GetActiveScene().name);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlayMusicForScene(scene.name);
    }

    private void PlayMusicForScene(string sceneName)
    {
        AudioClip clipToPlay = null;

        if (sceneName == gameplaySceneName)
        {
            clipToPlay = gameplayMusic;
        }
        else if (sceneName == gamblingSceneName)
        {
            clipToPlay = gamblingMusic;
        }

        if (clipToPlay != null && source.clip != clipToPlay)
        {
            source.Stop();
            source.clip = clipToPlay;
            source.volume = volume;
            source.loop = true;
            source.Play();
        }
    }
}



