using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

/// <summary>
/// Se pone en el objeto "Meta" (puerta del templo). Debe tener un Collider2D con Is Trigger activado.
/// Cuando el Player lo toca (y cumple el requisito de monedas) muestra la pantalla de victoria.
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class VictoryZone : MonoBehaviour
{
    [Header("UI (arrastrar desde la jerarquia)")]
    [SerializeField] private GameObject victoryPanel;     // Panel de victoria (empieza oculto)
    [SerializeField] private TMP_Text resultText;         // Opcional: "Monedas: 8/8"
    [SerializeField] private TMP_Text hintText;           // Opcional: mensaje si aun faltan monedas

    [Header("Reglas de la meta")]
    [Tooltip("Monedas necesarias para abrir la meta. 0 = no se exige ninguna.")]
    [SerializeField] private int requiredCoins = 0;
    [SerializeField] private string coinTag = "Coin";
    [SerializeField] private string playerTag = "Player";

    [Header("Escenas")]
    [SerializeField] private string mainMenuScene = "MainMenu";
    [Tooltip("Escena del siguiente nivel. DEJAR VACIO en el ultimo nivel para mostrar la pantalla de victoria.")]
    [SerializeField] private string nextSceneName = "";
    [Tooltip("Segundos de espera antes de cambiar de nivel (para que suene el sonido).")]
    [SerializeField] private float delayBeforeNextLevel = 1f;

    [Header("Audio (opcional)")]
    [SerializeField] private AudioClip victorySound;

    private int totalCoins;
    private bool hasWon;
    private float hintTimer;

    private void Start()
    {
        Time.timeScale = 1f; // por si se llega desde una pausa
        if (victoryPanel != null) victoryPanel.SetActive(false);
        if (hintText != null) hintText.gameObject.SetActive(false);

        totalCoins = CountCoinsInScene();
    }

    private void Update()
    {
        // Oculta el mensaje de "faltan monedas" despues de 2.5 segundos
        if (hintText != null && hintText.gameObject.activeSelf)
        {
            hintTimer -= Time.unscaledDeltaTime;
            if (hintTimer <= 0f) hintText.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasWon || !other.CompareTag(playerTag)) return;

        int collected = totalCoins - CountCoinsInScene();

        if (collected < requiredCoins)
        {
            ShowHint("Te faltan " + (requiredCoins - collected) + " monedas para abrir la puerta");
            return;
        }

        Win(collected);
    }

    private void Win(int collected)
    {
        hasWon = true;

        if (victorySound != null)
            AudioSource.PlayClipAtPoint(victorySound, Camera.main != null ? Camera.main.transform.position : transform.position);

        // Si hay siguiente nivel, pasa a el en vez de mostrar la victoria
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            Invoke(nameof(LoadNextLevel), delayBeforeNextLevel);
            return;
        }

        // Ultimo nivel: pantalla de victoria final
        if (resultText != null)
            resultText.text = "Monedas: " + collected + " / " + totalCoins;

        // Evita que se pueda pausar mientras se muestra la victoria
        var pause = FindFirstObjectByType<PauseMenuManager>();
        if (pause != null) pause.enabled = false;

        if (victoryPanel != null) victoryPanel.SetActive(true);
        Time.timeScale = 0f; // congela el juego
    }

    private void LoadNextLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(nextSceneName);
    }

    private void ShowHint(string message)
    {
        if (hintText == null) { Debug.Log(message); return; }
        hintText.text = message;
        hintText.gameObject.SetActive(true);
        hintTimer = 2.5f;
    }

    // Cuenta las monedas que siguen en la escena (las recogidas se destruyen/desactivan)
    private int CountCoinsInScene()
    {
        try { return GameObject.FindGameObjectsWithTag(coinTag).Length; }
        catch (UnityException)
        {
            Debug.LogWarning("El tag '" + coinTag + "' no existe. Revisa el nombre en Tags & Layers.");
            return 0;
        }
    }

    // ---------- Botones del panel de victoria (OnClick) ----------

    public void PlayAgain()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuScene);
    }

    public void QuitGame()
    {
        Debug.Log("Cerrando juego...");
        Application.Quit();
    }
}
