using UnityEngine;
using UnityEngine.SceneManagement;

public class GamblingButtons : MonoBehaviour
{
    public string mainMenu;
    public string gameplay;

    public void LoadGame(){
        SceneManager.LoadScene(gameplay);
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene(mainMenu);
    }
}
