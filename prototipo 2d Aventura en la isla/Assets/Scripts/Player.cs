using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 5;
    private Rigidbody2D rb2D;

    private float move;
    public float jumpForce = 4;
    private bool isGrounded;
    public Transform groundCheck;
    public float groundRadius = 0.1f;
    public LayerMask groundLayer;

    private Animator animator;

    private int coins;
    public TMP_Text textCoins;

    public AudioSource audioSource;
    public AudioClip coinClip;
    public AudioClip barrelClip;

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        // Respaldos por si se pierde una referencia al crear/duplicar escenas
        if (textCoins == null)
        {
            GameObject coinTextObj = GameObject.Find("TextCoin");
            if (coinTextObj != null) textCoins = coinTextObj.GetComponent<TMP_Text>();
            else Debug.LogWarning("Player: falta asignar 'Text Coins' y no se encontro un objeto llamado 'TextCoin'.");
        }

        if (audioSource == null) audioSource = GetComponent<AudioSource>();
        if (groundCheck == null) Debug.LogWarning("Player: falta asignar 'Ground Check'.");
    }

    void Update()
    {
        move = Input.GetAxisRaw("Horizontal");
        rb2D.linearVelocity = new Vector2(move * speed, rb2D.linearVelocity.y);

        if (move != 0)
            transform.localScale = new Vector3(Mathf.Sign(move), 1, 1);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb2D.linearVelocity = new Vector2(rb2D.linearVelocity.x, jumpForce);
        }

        animator.SetFloat("Speed", Mathf.Abs(move));
        animator.SetFloat("VerticalVelocity", rb2D.linearVelocity.y);
        animator.SetBool("IsGrounded", isGrounded);
    }

    private void FixedUpdate()
    {
        if (groundCheck != null)
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, groundLayer);
    }

    // Reproduce un sonido; avisa en la Console si falta el clip y usa un respaldo si falta el AudioSource
    private void PlaySound(AudioClip clip, string nombre)
    {
        if (clip == null)
        {
            Debug.LogWarning("Player: falta asignar '" + nombre + "' en el Inspector (escena: " + gameObject.scene.name + ").");
            return;
        }

        if (audioSource != null) audioSource.PlayOneShot(clip);
        else AudioSource.PlayClipAtPoint(clip, transform.position);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Coin"))
        {
            PlaySound(coinClip, "Coin Clip");
            Destroy(collision.gameObject);
            coins++;
            if (textCoins != null) textCoins.text = coins.ToString();
        }

        if (collision.transform.CompareTag("Spikes"))
        {
            PlayerHealth health = GetComponent<PlayerHealth>();
            if (health != null) health.RecibirDano();
        }

        if (collision.transform.CompareTag("Barrel"))
        {
            PlaySound(barrelClip, "Barrel Clip");

            Vector2 knockbackDir = (rb2D.position - (Vector2)collision.transform.position).normalized;
            rb2D.linearVelocity = Vector2.zero;
            rb2D.AddForce(knockbackDir * 3, ForceMode2D.Impulse);

            BoxCollider2D[] colliders = collision.gameObject.GetComponents<BoxCollider2D>();
            foreach (BoxCollider2D col in colliders)
            {
                col.enabled = false;
            }

            Animator barrelAnimator = collision.GetComponent<Animator>();
            if (barrelAnimator != null) barrelAnimator.enabled = true;

            Destroy(collision.gameObject, 0.5f);
        }
    }
}
