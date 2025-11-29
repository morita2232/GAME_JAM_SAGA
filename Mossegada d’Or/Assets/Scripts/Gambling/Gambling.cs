using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using System.Collections;
using TMPro;

public class Gambling : MonoBehaviour
{
    [Header("Configuración")]
    public List<Color> colores = new List<Color>();
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
        if (colores.Count == 0)
        {
            colores.Add(Color.red);
            colores.Add(Color.green);
            colores.Add(Color.blue);
        }

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
                int rand = Random.Range(0, colores.Count);
                comidas[i].GetComponent<Renderer>().material.color = colores[rand];
            }

            timer += 0.1f;
            yield return new WaitForSeconds(0.1f);

        }

        //Logica de si has ganado o no
        if (AllSameColor())
        {
            GameManager.Instance.AddCoins(20);
            statusText.text = $"Has guanyat +{20} monedas!";
        }
        else
        {
            statusText.text = "Mala sort... No has guanyat.";
        }

        isSpinning = false;

    }


    bool AllSameColor()
    {
        Color firstColor = comidas[0].GetComponent<Renderer>().material.color;

        for (int i = 1; i < comidas.Length; i++)
        {
            if (comidas[i].GetComponent<Renderer>().material.color != firstColor)
            {
                return false;
            }
        }

        return true;
    }

}