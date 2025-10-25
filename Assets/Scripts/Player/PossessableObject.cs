using UnityEngine;

public class PossessableObject : MonoBehaviour
{
    [Header("Movimiento del objeto poseído")]
    public float moveSpeed = 3f;
    public float jumpForce = 10f;
    public float maxJumpTime = 0.3f;
    public float jumpMultiplier = 1.5f;

    [Header("Detección del suelo")]
    public Transform groundCheck;
    public float checkRadius = 0.2f;
    public LayerMask whatIsGround;
    private bool isGrounded;

    private bool isPossessed = false;
    private PlayerMov currentGhost;
    private float possessionTimer;

    private Rigidbody2D rb;
    private SpriteRenderer sr;

    private bool isJumping;
    private float jumpTimeCounter;

    private RigidbodyType2D originalBodyType;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        originalBodyType = rb.bodyType;
    }

    void Update()
    {
        if (isPossessed)
        {
            possessionTimer -= Time.deltaTime;

            // --- Detección de suelo ---
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);

            // --- Movimiento lateral ---
            float moveX = Input.GetAxisRaw("Horizontal");
            rb.linearVelocity = new Vector2(moveX * moveSpeed, rb.linearVelocity.y);

            // Flip sprite según dirección
            if (moveX > 0) sr.flipX = false;
            else if (moveX < 0) sr.flipX = true;

            // --- Salto ---
            if (isGrounded && Input.GetKeyDown(KeyCode.Space))
            {
                isJumping = true;
                jumpTimeCounter = maxJumpTime;
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            }

            if (Input.GetKey(KeyCode.Space) && isJumping)
            {
                if (jumpTimeCounter > 0)
                {
                    rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce * jumpMultiplier);
                    jumpTimeCounter -= Time.deltaTime;
                }
                else
                {
                    isJumping = false;
                }
            }

            if (Input.GetKeyUp(KeyCode.Space))
            {
                isJumping = false;
            }

            // --- Fin del tiempo de posesión ---
            if (possessionTimer <= 0f)
            {
                EndPossession();
            }
        }
        else
        {
            // Si no está poseído, queda estático (no se cae)
            rb.bodyType = RigidbodyType2D.Static;
        }
    }

    public void StartPossession(PlayerMov ghost, float duration)
    {
        isPossessed = true;
        currentGhost = ghost;
        possessionTimer = duration;

        rb.bodyType = RigidbodyType2D.Dynamic; // activar físicas mientras se controla
        rb.gravityScale = 3f; // ajustá la gravedad a gusto
    }

    public void EndPossession()
    {
        isPossessed = false;
        rb.linearVelocity = Vector2.zero;

        if (currentGhost != null)
        {
            currentGhost.EndPossession(transform.position);
            currentGhost = null;
        }

        rb.bodyType = RigidbodyType2D.Static; // el objeto queda quieto sobre el suelo
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!isPossessed && collision.CompareTag("Player"))
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                collision.GetComponent<PlayerMov>().PossessObject(gameObject);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Muestra el área donde detecta el suelo
        if (groundCheck != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(groundCheck.position, checkRadius);
        }
    }
}
