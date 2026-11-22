using System.Collections;
using UnityEngine;
using TMPro;

public class AvisoInicioNivel : MonoBehaviour
{
    [Header("Interfaz")]
    public TextMeshProUGUI textoAviso;

    [Header("Configuración del Nivel")]
    public int monedasNecesarias = 5;
    public float duracionSegundos = 5f;

    private void Start()
    {
        StartCoroutine(MostrarAvisoInicial());
    }

    private IEnumerator MostrarAvisoInicial()
    {
        if (textoAviso != null)
        {
            textoAviso.gameObject.SetActive(true);
            textoAviso.text = $"Recolecta {monedasNecesarias} monedas y llega a la puerta al final del nivel";
        }

        yield return new WaitForSeconds(duracionSegundos);

        if (textoAviso != null)
        {
            textoAviso.gameObject.SetActive(false);
        }
    }
}