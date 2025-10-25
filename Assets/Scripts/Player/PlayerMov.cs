using UnityEngine;

public class PlayerMov : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;

    [Header("Movimiento del fantasma")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 12f;

    [Header("Detección de suelo")]
    [SerializeField] private Transform footPoint;       // Punto en los pies del personaje
    [SerializeField] private float rayDistance = 0.7f;  // Distancia del raycast

    private float _moveInput;
    private Rigidbody2D _rb;
    private SpriteRenderer _spriteRenderer;
    private bool _isGrounded;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    void Update()
    {
        HandleMovement();
        HandleFlip();
        if (CheckGround())
            HandleJump();
    }

    private void HandleMovement()
    {
        _moveInput = Input.GetAxisRaw("Horizontal");
        _rb.linearVelocity = new Vector2(_moveInput * speed, _rb.linearVelocity.y);
    }

    private void HandleFlip()
    {
        if (_moveInput > 0)
        {
            _spriteRenderer.flipX = false;
            playerController.GetPlayerAnimations().ToggleRunAnimation(true);
        }
        else if (_moveInput < 0)
        {
            _spriteRenderer.flipX = true;
            playerController.GetPlayerAnimations().ToggleRunAnimation(true);
        }
        else
        {
            playerController.GetPlayerAnimations().ToggleRunAnimation(false);
        }
    }

    private bool CheckGround()
    {
        // Lanzamos un raycast desde los pies hacia abajo
        ContactFilter2D filter = new ContactFilter2D();
        filter.useTriggers = false; // ignorar triggers
        int ignoreLayer = LayerMask.NameToLayer("Player"); // la capa que quiero ignorar
        int mask = ~(1 << ignoreLayer); // todas las capas menos la ignorada
        filter.SetLayerMask(mask);

        RaycastHit2D[] results = new RaycastHit2D[1]; // Solo necesitamos el primer impacto
        int hitCount = Physics2D.Raycast(footPoint.position, Vector2.down, filter, results, rayDistance);

        if (hitCount > 0 && results[0].collider)
        {
            Debug.DrawLine(footPoint.position, results[0].point, Color.green);

            // Solo devolvemos true si el primer collider impactado es el del jugador
            return true;
        }
        return false;
    }

    private void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, jumpForce);
        }
    }
}
