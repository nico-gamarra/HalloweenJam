using UnityEngine;
using System.Collections;

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

    [Header("Efectos visuales")]
    public Color possessedColor = Color.cyan;

    private Color originalColor;
    private bool isGrounded;
    private bool isPossessed = false;
    private bool isJumping;

    private float jumpTimeCounter;
    private float possessionTimer;

    private PlayerMov currentGhost;
    private Rigidbody2D rb;
    private SpriteRenderer sr;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        originalColor = sr.color;

        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = 3f;
        rb.freezeRotation = true;
    }

    void Update()
    {
        if (isPossessed && currentGhost != null && currentGhost.IsPossessing)
        {
            possessionTimer -= Time.deltaTime;
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);

            // --- Movimiento lateral ---
            float moveX = Input.GetAxisRaw("Horizontal");
            rb.linearVelocity = new Vector2(moveX * moveSpeed, rb.linearVelocity.y);

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
                    isJumping = false;
            }

            if (Input.GetKeyUp(KeyCode.Space))
                isJumping = false;

            // --- Fin del tiempo de posesión ---
            if (possessionTimer <= 0f)
                StartCoroutine(ReturnControl());
        }
        else if (!isPossessed)
        {
            // Cuando no está poseído, se detiene lateralmente
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        }
    }

    public void StartPossession(PlayerMov ghost, float duration)
    {
        isPossessed = true;
        currentGhost = ghost;
        possessionTimer = duration;

        sr.color = possessedColor;
    }

    private IEnumerator ReturnControl()
    {
        yield return null; // Espera un frame para evitar conflictos de física
        EndPossession();
    }

    public void EndPossession()
    {
        if (!isPossessed) return;

        isPossessed = false;
        rb.linearVelocity = Vector2.zero;
        sr.color = originalColor;

        if (currentGhost != null)
        {
            currentGhost.EndPossession(transform.position);
            currentGhost = null;
        }
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
        if (groundCheck != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(groundCheck.position, checkRadius);
        }
    }
}
