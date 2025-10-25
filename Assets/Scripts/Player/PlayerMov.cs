using UnityEngine;

public class PlayerMov : MonoBehaviour
{
    [Header("Movimiento")]
    [SerializeField] private float speed = 5f; // Velocidad de movimiento lateral
    private float moveInput;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    [Header("Salto")]
    [SerializeField] private float jumpForce = 12f; // Fuerza inicial del salto
    [SerializeField] private float maxJumpTime = 0.4f; // Tiempo máximo que puede mantener el salto
    [SerializeField] private float jumpMultiplier = 1.5f; // Aumenta la fuerza mientras se mantiene
    private float jumpTimeCounter;
    private bool isJumping;
    private bool isGrounded;
    
    [Header("Posesión")]
    public float possessionDuration = 5f;
    private bool isPossessing = false;
    private GameObject possessedObject;

    private RigidbodyType2D originalbodyType;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalbodyType = rb.bodyType; 
    }

    void Update()
    {
        // --- Movimiento lateral ---
        moveInput = Input.GetAxisRaw("Horizontal");
        rb.linearVelocity = new Vector2(moveInput * speed, rb.linearVelocity.y);

        if (moveInput > 0)
        {
            spriteRenderer.flipX = false;
        }
        else if (moveInput < 0)
        {
            spriteRenderer.flipX = true;
        }
            

        // --- Salto sensible ---
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
    }

    // --- Colisión con el suelo ---
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
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
        rb.bodyType = originalbodyType;
        transform.position = newPosition;
        spriteRenderer.enabled = true;
        possessedObject = null;
    }
}
