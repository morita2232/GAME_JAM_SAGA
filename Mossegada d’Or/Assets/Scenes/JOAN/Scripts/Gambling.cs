using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class Gambling : MonoBehaviour
{
    [Header("Configuracion")]
    public List<Color> colores = new List<Color>();
    public GameObject[] squares;
    public InputAction spinAction;

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
        colores.Add(Color.red);
        colores.Add(Color.green);
        colores.Add(Color.blue);
    }

    private void OnSpin(InputAction.CallbackContext context)
    {
        for (int i = 0; i < squares.Length; i++)
        {
            int numRand = Random.Range(0, colores.Count);
            squares[i].GetComponent<Renderer>().material.color = colores[numRand];
        }
    }
}
