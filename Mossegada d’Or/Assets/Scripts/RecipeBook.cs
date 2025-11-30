using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RecipeBook : MonoBehaviour
{
    // Only these 4 need to be assigned in the Inspector
    public TextMeshProUGUI posNombreIzquierda;
    public TextMeshProUGUI posNombreDerecha;
    public TextMeshProUGUI posRecetaIzquierda;
    public TextMeshProUGUI posRecetaDerecha;
    public UnityEngine.UI.Image posIzquierda;
    public UnityEngine.UI.Image posDerecha;


    // Data is stored only in code
    private List<string> nombres;
    private List<string> recetas;
    public List<Sprite> sprites;

    // index of the LEFT recipe on the current “page”
    private int currentIndex = 0;

    void Start()
    {
        // --- Fill the data lists in code ---
        nombres = new List<string>
        {
            "Crema catalana",
            "Calçots",
            "Canelons",
            "Panellets",
            "Pa amb tomàquet"
        };

        recetas = new List<string>
        {
            "Escalfa la llet amb canyella i pell de llimona.\r\n\r\nBarreja rovells, sucre i midó.\r\n\r\nIncorpora la llet calenta i espesseix a foc lent.\r\n\r\nAboca en cassoletes i refreda.\r\n\r\nEspolvora sucre i crema’l amb un bufador.",
            "Cou els calçots sencers a flama viva.\r\n\r\nGira’ls fins que la capa exterior es cremi.\r\n\r\nDeixa’ls reposar embolicats amb paper de diari.\r\n\r\nPela la capa negra abans de menjar.\r\n\r\nServeix-los amb salsa romesco.",
            "Cou la pasta de caneló.\r\n\r\nSalteja carn picada amb ceba i tomàquet.\r\n\r\nFarceix els canelons i posa’ls en una safata.\r\n\r\nCobreix amb beixamel.\r\n\r\nGratina amb formatge.",
            "Barreja ametlla mòlta, sucre i clara d’ou.\r\n\r\nForma boletes.\r\n\r\nArrebossa amb pinyons, coco o ametlla.\r\n\r\nPinta amb rovell.\r\n\r\nEnforna fins que quedin dorats.",
            "Torra pa de pagès.\r\n\r\nFrega-hi all (opcional).\r\n\r\nFrega-hi tomàquet madur.\r\n\r\nAfegeix oli d’oliva verge.\r\n\r\nSala al gust."
        };

        MostrarPagina();
    }

    public void SiguientePag()
    {
        // Each page shows 2 recipes  move by 2
        currentIndex += 2;

        // Simple wrap-around (go back to first page at the end)
        if (currentIndex >= nombres.Count)
        {
            currentIndex = 0;
        }

        MostrarPagina();
    }

    private void MostrarPagina()
    {
        // ----- LEFT PAGE -----
        posNombreIzquierda.text = nombres[currentIndex];
        posRecetaIzquierda.text = recetas[currentIndex];
        posIzquierda.sprite = sprites[currentIndex];

        int rightIndex = currentIndex + 1;

        // ----- RIGHT PAGE -----
        if (rightIndex < nombres.Count)
        {
            posNombreDerecha.text = nombres[rightIndex];
            posRecetaDerecha.text = recetas[rightIndex];
            posDerecha.sprite = sprites[rightIndex];

            posNombreDerecha.gameObject.SetActive(true);
            posRecetaDerecha.gameObject.SetActive(true);
            posDerecha.gameObject.SetActive(true);
        }
        else
        {
            // Hide only the right page when there is no second item
            posNombreDerecha.gameObject.SetActive(false);
            posRecetaDerecha.gameObject.SetActive(false);
            posDerecha.gameObject.SetActive(false);
        }
    }

}


