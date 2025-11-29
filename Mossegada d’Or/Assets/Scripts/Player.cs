using UnityEngine;

public class Player : MonoBehaviour
{
    public int money;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            if (GameManager.Instance != null)
            {
                money = GameManager.Instance.coins;   // sync from GameManager
            }
            else
            {
                Debug.LogWarning("GameManager.Instance is null in Player.Start!");
            }
        }

        // Update is called once per frame
        void Update()
    {

        
    }
}
