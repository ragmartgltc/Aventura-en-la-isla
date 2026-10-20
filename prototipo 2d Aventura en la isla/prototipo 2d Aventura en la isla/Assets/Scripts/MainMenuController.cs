using UnityEngine;
using UnityEngine.SceneManagement; 
using TMPro; 

public class MainMenuController : MonoBehaviour
{
    [Header("Tus Paneles de Unity")]
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject optionsPanel;
    [SerializeField] private GameObject lorePanel;

    [Header("Tu Texto de Mutear")]
    [SerializeField] private TextMeshProUGUI muteButtonText; 
    private bool isMuted = false;

    public void ShowLore()
    {
        if (mainMenuPanel != null && lorePanel != null)
        {
            mainMenuPanel.SetActive(false);
            lorePanel.SetActive(true);
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene("SampleScene"); 
    }

    public void OpenOptions()
    {
        if (optionsPanel != null) optionsPanel.SetActive(true);
    }

    public void CloseOptions()
    {
        if (optionsPanel != null) optionsPanel.SetActive(false);
    }

    public void ToggleSound()
    {
        isMuted = !isMuted;

        if (isMuted)
        {
            AudioListener.pause = true; 
            if (muteButtonText != null) muteButtonText.text = "SONIDO: NO";
        }
        else
        {
            AudioListener.pause = false; 
            if (muteButtonText != null) muteButtonText.text = "SONIDO: SÍ";
        }
    }

    public void QuitGame()
    {
        Debug.Log("El jugador presionó ExitButton. Cerrando juego...");
        Application.Quit(); 
    }
}
