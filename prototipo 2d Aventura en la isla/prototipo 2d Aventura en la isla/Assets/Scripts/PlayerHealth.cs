using UnityEngine;
using UnityEngine.SceneManagement; // Requerido para reiniciar el nivel

public class PlayerHealth : MonoBehaviour
{
    // Las casillas donde arrastraremos nuestros 4 corazones de la pantalla
    [Header("Tus Corazones de la UI")]
    [SerializeField] private GameObject corazon1;
    [SerializeField] private GameObject corazon2;
    [SerializeField] private GameObject corazon3;
    [SerializeField] private GameObject corazon4;

    private int vidasActuales = 4; // Empezamos con 4 vidas

    // Esta función se activa automáticamente cuando tocamos las lanzas o pinchos
    public void RecibirDano()
    {
        vidasActuales--; // Restamos una vida

        // Apagamos el corazón correspondiente según las vidas que nos queden
        if (vidasActuales == 3)
        {
            if (corazon4 != null) corazon4.SetActive(false);
        }
        else if (vidasActuales == 2)
        {
            if (corazon3 != null) corazon3.SetActive(false);
        }
        else if (vidasActuales == 1)
        {
            if (corazon2 != null) corazon2.SetActive(false);
        }
        else if (vidasActuales <= 0)
        {
            if (corazon1 != null) corazon1.SetActive(false);
            
            // ¡PERDIMOS TODO! Reiniciamos el nivel desde cero
            ReiniciarNivel();
        }
    }

    private void ReiniciarNivel()
    {
        Time.timeScale = 1f; // Asegura que el tiempo no esté congelado
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Recarga la escena actual
    }
}
