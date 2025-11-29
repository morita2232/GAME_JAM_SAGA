using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ComidaManager : MonoBehaviour
{
    public List<GameObject> foods;      // Food prefab GameObjects
    public Transform foodSpawn;
    public Player player;

    public TextMeshProUGUI scoreVisual;
    public TextMeshProUGUI timeVisual;

    private float timer;

    public bool canSpawnNext = true;
    public float spawnInterval = 5f;

    private float spawnTimer = 0f;
    private Comida currentFoodInstance;

    [Header("Audio Clips")]
    public AudioClip eatSound;
    public AudioClip throwSound;
    public AudioClip correctSound;
    public AudioClip wrongSound;
    private AudioSource audioSource;

    private void Start()
    {
        timer = 60;
        audioSource = GetComponent<AudioSource>();

    }
    void Update()
    {
        // STOP all game logic if paused
        if (PauseMenu.GameIsPaused)
        {

            Debug.Log("PAUSED");
            return;
        }
        else
        {
            timeVisual.SetText("Temps restant: " + (int)timer);

            if (timer > 0)
            {
                timer -= Time.deltaTime;

                HandleInput();
                HandleSpawning();
            }
            else
            {
                SceneManager.LoadScene("GamblingScene");
            }
        }
    }


    void HandleSpawning()
    {
        if (!canSpawnNext || currentFoodInstance != null)
            return;

        spawnTimer += Time.deltaTime;

        if (spawnTimer >= spawnInterval)
        {
            spawnTimer = 0f;
            SpawnRandomFood();
        }
    }

    void HandleInput()
    {
        if (currentFoodInstance == null) return;

        if (Input.GetKeyDown(KeyCode.Q))
        {
            audioSource.PlayOneShot(throwSound);

            Debug.Log("Has tirado");
            if (currentFoodInstance.catalan)
            {
                audioSource.PlayOneShot(wrongSound);

                player.money -= currentFoodInstance.points;

            }
            else if (!currentFoodInstance.catalan)
            {
                audioSource.PlayOneShot(correctSound);

                player.money += currentFoodInstance.points;
            }

            if(player.money < 0)
            {
                player.money = 0;
            }
            scoreVisual.SetText("Puntuació: " + player.money);
            NextFood();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            audioSource.PlayOneShot(eatSound);

            Debug.Log("Has comido");
            player.money += currentFoodInstance.points;
           
            if (currentFoodInstance.catalan)
            {
                audioSource.PlayOneShot(correctSound);

            }
            else if (!currentFoodInstance.catalan)
            {
                audioSource.PlayOneShot(wrongSound);
            }
           
            if (player.money < 0)
            {
                player.money = 0;
            }
            scoreVisual.SetText("Puntuació: " + player.money);
            NextFood();
        }
    }

    void SpawnRandomFood()
    {
        if (foods == null || foods.Count == 0)
        {
            Debug.LogWarning("No foods assigned in ComidaManager.");
            return;
        }

        int index = Random.Range(0, foods.Count);
        GameObject prefabGO = foods[index];

        if (prefabGO == null)
        {
            Debug.LogError("foods[" + index + "] is null!");
            return;
        }

        // Get the Comida script ON the prefab
        Comida prefab = prefabGO.GetComponent<Comida>();
        if (prefab == null)
        {
            Debug.LogError("Food prefab " + prefabGO.name + " has NO Comida script!");
            return;
        }

        if (!prefab.unlocked)
        {
            Debug.Log("Chosen food is locked, skipping spawn.");
            return;
        }

        Debug.Log("Spawning food: " + prefabGO.name);

        // Spawn the prefab
        GameObject instanceGO = Instantiate(prefabGO, foodSpawn.position, Quaternion.identity);

        // Get Comida on the spawned instance
        currentFoodInstance = instanceGO.GetComponent<Comida>();
        currentFoodInstance.alive = true;

        canSpawnNext = false;
    }

    void NextFood()
    {
        if (currentFoodInstance != null)
        {
            currentFoodInstance.alive = false;
            Destroy(currentFoodInstance.gameObject);
            currentFoodInstance = null;
        }

        canSpawnNext = true;
        spawnTimer = 0f;
    }
}


