using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int coins = 100;
    public TextMeshProUGUI uiCoins;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  // <-- key line
        }
        else
        {
            Destroy(gameObject);           // destroy duplicates in later scenes
            return;
        }
    }

    private void Start()
    {

        SyncFromPlayer();
        UpdateUI();
    }

    private void Update()
    {
        if (uiCoins == null)
        {
            GameObject coinUI = GameObject.FindGameObjectWithTag("Coins");

            if (coinUI != null)
            {
                uiCoins = coinUI.GetComponent<TextMeshProUGUI>();
                UpdateUI();
            }
        }
    }
    public bool TrySpend(int amount)
    {
        if (coins >= amount)
        {
            coins -= amount;
            UpdateUI();
            return true;
        }

        return false;
    }

    public void AddCoins(int amount)
    {
        coins += amount;
        UpdateUI();
    }

    private void SyncFromPlayer()
    {
        Player player = FindAnyObjectByType<Player>();

        if (player != null)
        {
            coins = player.money;
        }
    }


    private void UpdateUI()
    {
        if (uiCoins != null)
        {
            uiCoins.text = $"{coins}";
        }
    }
}

