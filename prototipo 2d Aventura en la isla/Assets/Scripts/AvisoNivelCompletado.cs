using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AvisoNivelCompletado : MonoBehaviour
{
    [Header("Interfaz de Imagen")]
    public GameObject objetoImagenCompletado; // El objeto Image dentro del Canvas
    public Image componenteImagen;            // Componente Image donde se pone el Sprite
    public Sprite spriteNivelCompletado;      // Imagen específica de este nivel

    [Header("Tiempo y Transición")]
    public float segundosEnPantalla = 3f;
    public bool esUltimoNivel = false;        // Marca TRUE solo en Nivel3
    public string escenaSiguiente;            // Ej: "Nivel2" en Nivel 1, "Nivel3" en Nivel 2
    public string escenaMenuPrincipal = "MainMenu";

    private bool yaMostrado = false;

    private void Start()
    {
        Time.timeScale = 1f;

        if (objetoImagenCompletado != null)
        {
            objetoImagenCompletado.SetActive(false);
        }
    }

    // Llama a este método cuando el jugador llegue a la puerta con las monedas necesarias
    public void MostrarCompletado()
    {
        if (yaMostrado) return;
        yaMostrado = true;

        if (componenteImagen != null && spriteNivelCompletado != null)
        {
            componenteImagen.sprite = spriteNivelCompletado;
        }

        if (objetoImagenCompletado != null)
        {
            objetoImagenCompletado.SetActive(true);
        }

        StartCoroutine(EsperarYContinuar());
    }

    private IEnumerator EsperarYContinuar()
    {
        yield return new WaitForSecondsRealtime(segundosEnPantalla);
        Time.timeScale = 1f;

        if (esUltimoNivel)
        {
            SceneManager.LoadScene(escenaMenuPrincipal);
        }
        else if (!string.IsNullOrEmpty(escenaSiguiente))
        {
            SceneManager.LoadScene(escenaSiguiente);
        }
    }
}