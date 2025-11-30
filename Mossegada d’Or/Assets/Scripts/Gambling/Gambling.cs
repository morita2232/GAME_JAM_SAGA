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
    public int pityThreshold = 10;      // after 10 fails, guarantee a new unlock
    private int spinsSinceLastNewUnlock = 0;

    public GameObject lever;
    private SpriteRenderer leverRenderer;
    public Sprite leverUp;
    public Sprite leverDown;

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
        leverRenderer = lever.GetComponent<SpriteRenderer>();

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

        //BAJAR PALANCA DURANTE 0.5s
        leverRenderer.sprite = leverDown;
        yield return new WaitForSeconds(0.5f);
        //SUBIR PALANCA
        leverRenderer.sprite = leverUp;

        statusText.text = "Decidint...";

        // Animación de gambling
        while (timer < spinTime)
        {
            for (int i = 0; i < comidas.Length; i++)
            {
                int rand = Random.Range(0, gm.foods.Count);
                var prefab = gm.foods[rand];
                Debug.Log(prefab.name);
                comidas[i].GetComponent<SpriteRenderer>().sprite = prefab.GetComponent<SpriteRenderer>().sprite;
            }

            timer += 0.1f;
            yield return new WaitForSeconds(0.1f);
        }

        // Resultados finales normales
        int[] finalResults = new int[comidas.Length];

        for (int i = 0; i < comidas.Length; i++)
        {
            finalResults[i] = Random.Range(0, gm.foods.Count);
            var prefab = gm.foods[finalResults[i]];
            comidas[i].GetComponent<SpriteRenderer>().sprite = prefab.GetComponent<SpriteRenderer>().sprite;
        }

        // ¿Coinciden todos?
        bool allMatch = true;
        for (int i = 1; i < finalResults.Length; i++)
        {
            if (finalResults[i] != finalResults[0])
            {
                allMatch = false;
                break;
            }
        }

        GameObject wonFood = null;
        Comida comidaComp = null;
        bool newUnlockThisSpin = false;

        if (allMatch)
        {
            wonFood = gm.foods[finalResults[0]];
            comidaComp = wonFood.GetComponent<Comida>();

            if (comidaComp != null && !comidaComp.unlocked)
            {
                comidaComp.unlocked = true;
                newUnlockThisSpin = true;
                statusText.text = $"HAS GUANYAT: {wonFood.name} (NOU!)";
            }
            else
            {
                statusText.text = $"Ja el tens: {wonFood.name}";
            }
        }
        else
        {
            statusText.text = "No has guanyat, intenta de nou!";
        }

        // --- SISTEMA DE PITY (RIG) ---

        if (newUnlockThisSpin)
        {
            // reset contador si hubo nuevo desbloqueo
            spinsSinceLastNewUnlock = 0;
        }
        else
        {
            spinsSinceLastNewUnlock++;

            // si hemos fallado 'pityThreshold' veces seguidas, forzar nuevo desbloqueo
            if (spinsSinceLastNewUnlock >= pityThreshold)
            {
                // buscar todas las comidas bloqueadas
                List<int> lockedIndices = new List<int>();
                for (int i = 0; i < gm.foods.Count; i++)
                {
                    var c = gm.foods[i].GetComponent<Comida>();
                    if (c != null && !c.unlocked)
                        lockedIndices.Add(i);
                }

                if (lockedIndices.Count > 0)
                {
                    // elegir una comida bloqueada al azar
                    int forcedIndex = lockedIndices[Random.Range(0, lockedIndices.Count)];
                    wonFood = gm.foods[forcedIndex];
                    comidaComp = wonFood.GetComponent<Comida>();

                    // marcar como desbloqueada
                    comidaComp.unlocked = true;

                    // mostrarla en los 3 slots para que se note el premio
                    for (int i = 0; i < comidas.Length; i++)
                    {
                        comidas[i].GetComponent<SpriteRenderer>().sprite =
                            wonFood.GetComponent<SpriteRenderer>().sprite;
                    }

                    statusText.text = $"¡SORT ASSEGURADA! Desbloquejat: {wonFood.name}";
                    newUnlockThisSpin = true;
                    spinsSinceLastNewUnlock = 0;
                }
                else
                {
                    // no quedan comidas bloqueadas, ya lo tienes todo
                    statusText.text = "¡Ja tens tots els menjars desbloquejats!";
                    // podrías seguir dejando el contador como está, o resetearlo
                    spinsSinceLastNewUnlock = 0;
                }
            }
        }

        isSpinning = false;

    }
}