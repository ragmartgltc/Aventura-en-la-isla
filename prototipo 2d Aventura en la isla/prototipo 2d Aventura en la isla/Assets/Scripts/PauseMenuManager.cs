using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; // Obligatorio para controlar el texto de tu MuteButtonPause

public class PauseMenuManager : MonoBehaviour
{
   // Tus variables originales (¡No pierden su conexión!)
   public GameObject pauseMenu;
   public GameObject pauseButton;

   // Las nuevas variables adaptadas exactamente a tus nombres de la jerarquía
   [Header("Tus nuevas opciones de pausa")]
   [SerializeField] private GameObject optionsPanelPause;
   [SerializeField] private TextMeshProUGUI muteButtonTextPause;
   private bool isMuted = false;

   public void PauseGame()
   {
       Time.timeScale = 0;
       pauseButton.SetActive(false);
       pauseMenu.SetActive(true);
   } 
   
   public void ResumeGame()      
   {
       Time.timeScale = 1;
       pauseButton.SetActive(true);
       pauseMenu.SetActive(false);
       if (optionsPanelPause != null) optionsPanelPause.SetActive(false); // Cierra opciones por seguridad al volver al juego
   } 

   public void RestartGame() 
   {
       SceneManager.LoadScene(SceneManager.GetActiveScene().name);
       Time.timeScale = 1; 
   }

   public void QuitGame() 
   {
      Debug.Log("Close Game");
      Application.Quit();
   } 

   // NUEVAS FUNCIONES 100% COMPATIBLES CON TU CANVAS DE JUEGO

   // Abre tu panel OptionsPanelPause
   public void OpenOptionsPause()
   {
       if (optionsPanelPause != null) optionsPanelPause.SetActive(true);
   }

   // Cierra tu panel OptionsPanelPause (Botón VOLVER)
   public void CloseOptionsPause()
   {
       if (optionsPanelPause != null) optionsPanelPause.SetActive(false);
   }

   // Controla el botón de mutear sonido en el juego mutando el AudioListener global
   public void ToggleSoundPause()
   {
       isMuted = !isMuted;

       if (isMuted)
       {
           AudioListener.pause = true; // Silencia música y efectos del tirón
           if (muteButtonTextPause != null) muteButtonTextPause.text = "SONIDO: NO";
       }
       else
       {
           AudioListener.pause = false; // Devuelve el sonido al nivel
           if (muteButtonTextPause != null) muteButtonTextPause.text = "SONIDO: SÍ";
       }
   }
}
