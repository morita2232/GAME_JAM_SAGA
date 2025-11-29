using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [Header("Scenes")]
    [SerializeField] private string gameplaySceneName = "Gameplay";   // put your scene name here

    [Header("UI")]
    [SerializeField] private GameObject instructionsPanel;
    [SerializeField] private GameObject creditsPanel;

    private void Start()
    {
        // Hide instructions when the menu loads
        if (instructionsPanel != null)
            instructionsPanel.SetActive(false);

        if (creditsPanel != null)
            creditsPanel.SetActive(false);
    }

    // --- BUTTON FUNCTIONS ---

    public void PlayGame()
    {
        SceneManager.LoadScene(gameplaySceneName);
    }

    public void OpenInstructions()
    {
        if (instructionsPanel != null)
            instructionsPanel.SetActive(true);
    }

    public void OpenCredits()
    {
        if (creditsPanel !=null) 
            creditsPanel.SetActive(true);

    }

    public void CloseInstructions()
    {
        if (instructionsPanel != null)
            instructionsPanel.SetActive(false);
    }

    public void CloseCredits()
    {
        if (creditsPanel != null)
            creditsPanel.SetActive(false);
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;   // stop play mode in editor
#else
        Application.Quit();                                // close the game in a build
#endif
    }
}

