using UnityEngine;

public class PlayerMov : MonoBehaviour
{
    [Header("Movimiento del fantasma")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 12f;
    [SerializeField] private float maxJumpTime = 0.4f;
    [SerializeField] private float jumpMultiplier = 1.5f;

    [SerializeField] private PlayerController playerController;
    
    private float moveInput;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    private float jumpTimeCounter;
    private bool isJumping;
    private bool isGrounded;
    
    [Header("Detección del suelo")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float checkRadius = 0.2f;
    [SerializeField] private LayerMask whatIsGround;

    [Header("Posesión")]
    public float possessionDuration = 5f;
    private bool isPossessing = false;
    private GameObject possessedObject;
    private RigidbodyType2D originalBodyType;

    public bool IsPossessing => isPossessing; // Propiedad pública de solo lectura

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        originalBodyType = rb.bodyType;
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    void Update()
    {
        if (isPossessing) return;

        // --- Detección del suelo ---
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);

        // --- Movimiento lateral ---
        moveInput = Input.GetAxisRaw("Horizontal");
        rb.linearVelocity = new Vector2(moveInput * speed, rb.linearVelocity.y);
        
        // --- Flip ---
        if (moveInput > 0) spriteRenderer.flipX = false;
        else if (moveInput < 0) spriteRenderer.flipX = true;

        if (moveInput != 0)
        {
            playerController.GetPlayerAnimations().ToggleRunAnimation(true);
        }
        else
        {
            playerController.GetPlayerAnimations().ToggleRunAnimation(false);
        }

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
            else isJumping = false;
        }

        if (Input.GetKeyUp(KeyCode.Space)) isJumping = false;
    }

    // --- Detección del suelo ---
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground")) isGrounded = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground")) isGrounded = false;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground")) isGrounded = true;
    }

    // --- SISTEMA DE POSESIÓN ---
    public void PossessObject(GameObject target)
    {
        if (isPossessing) return;

        isPossessing = true;
        possessedObject = target;

        // Desactivar visual y movimiento del fantasma
        spriteRenderer.enabled = false;
        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Static;

        // Activar el control del objeto poseído
        possessedObject.GetComponent<PossessableObject>().StartPossession(this, possessionDuration);
    }

    public void EndPossession(Vector3 newPosition)
    {
        // Reactivar al fantasma
        isPossessing = false;
        rb.bodyType = originalBodyType;
        transform.position = newPosition;
        spriteRenderer.enabled = true;
        possessedObject = null;
    }
}
