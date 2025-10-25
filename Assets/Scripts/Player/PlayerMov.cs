using UnityEngine;

public class PlayerMov : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    
    [Header("Movimiento del fantasma")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 12f;
    [SerializeField] private float maxJumpTime = 0.4f;
    [SerializeField] private float jumpMultiplier = 1.5f;
    
    private float _moveInput;
    private Rigidbody2D _rb;
    private SpriteRenderer _spriteRenderer;

    private float _jumpTimeCounter;
    private bool _isJumping;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    void Update()
    {
        HandleFlip();
        HandleMovement();
        HandleJump();
    }

    private void HandleFlip()
    {
        // --- Flip ---
        if (_moveInput > 0)
        {
            _spriteRenderer.flipX = false;
            playerController.GetPlayerAnimations().ToggleRunAnimation(true);
        }
        else
        {
            if (_moveInput < 0){
                _spriteRenderer.flipX = true;
                playerController.GetPlayerAnimations().ToggleRunAnimation(true);
            }
            else
                playerController.GetPlayerAnimations().ToggleRunAnimation(false);
            
        }
    }

    private void HandleMovement()
    {
        // --- Movimiento lateral ---
        _moveInput = Input.GetAxisRaw("Horizontal");
        _rb.linearVelocity = new Vector2(_moveInput * speed, _rb.linearVelocity.y);
    }

    private void HandleJump()
    {
        // Si estamos en el suelo y presionamos espacio, saltamos
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, jumpForce);
        }
    }
}
