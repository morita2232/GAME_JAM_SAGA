using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using System.Collections;
using TMPro;

public class Gambling : MonoBehaviour
{
    [Header("GameManager")]
    public GameManager gm; //le pasara la lista modificada a game manager

    [Header("Configuración")]
    public GameObject[] comidas;
    public InputAction spinAction;
    public int spinCost = 10;
    public float spinTime = 2f;

    [Header("UI")]
    public TextMeshProUGUI statusText;

    private bool isSpinning = false;

    private void OnEnable()
    {
        spinAction.Enable();
        spinAction.performed += OnSpin;
    }

    private void OnDisable()
    {
        spinAction.performed -= OnSpin;
        spinAction.Disable();
    }

    void Start()
    {
        statusText.text = $"Pren espai per girar ({spinCost} monedas)";
    }

    public void OnSpin(InputAction.CallbackContext context)
    {
        if (isSpinning) return;

        if (GameManager.Instance.TrySpend(spinCost))
        {
            StartCoroutine(SpinRoutine());
        }
        else
        {
            statusText.text = "No tens monedas suficients";
        }
    }

    IEnumerator SpinRoutine()
    {
        isSpinning = true;
        float timer = 0f;

        statusText.text = "Decidint...";

        //Animacion de gambling
        while (timer < spinTime)
        {
            for (int i = 0; i < comidas.Length; i++)
            {
                int rand = Random.Range(0, gm.foods.Count);
                var prefab = gm.foods[rand];
                comidas[i].GetComponent<SpriteRenderer>().sprite = prefab.GetComponent<SpriteRenderer>().sprite;
            }

            timer += 0.1f;
            yield return new WaitForSeconds(0.1f);

        }

        // Elegir resultado final
        int finalRand = Random.Range(0, gm.foods.Count);
        GameObject result = gm.foods[finalRand];

        // Desbloquear comida si no estaba desbloqueada
        Comida comidaComp = result.GetComponent<Comida>();
        if (comidaComp != null && !comidaComp.unlocked)
        {
            comidaComp.unlocked = true;
            statusText.text = $"Nuevo desbloqueo: {result.name}";
        }
        else
        {
            statusText.text = $"Repetido: {result.name}";
        }

        isSpinning = false;
    }
}