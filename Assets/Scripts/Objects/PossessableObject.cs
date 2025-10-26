using System;
using UnityEngine;
using System.Collections;

public class PossessableObject : MonoBehaviour
{
    public static event Action OnPossess;
    
    [Header("Config")]
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float possessDuration;
    [SerializeField] private float onEndPossessingCooldown;
    [SerializeField] private float onStartPossessingCooldown;
    
    private bool _isPossessed;
    private float _possessionTimer;
    private float _cooldownTimer;
    
    private bool _playerInRange;
    private GameObject _player;
    
    private Rigidbody2D _rb;
    private SpriteRenderer _sr;

    private float possestimer2;
    private float cooldowntimer2;

    private void Awake()
    {
        _rb = GetComponentInParent<Rigidbody2D>();
        _sr = GetComponentInParent<SpriteRenderer>();
    }

    void Update()
    {
        
        if (_isPossessed)
        {
            HandleObjectMovement();
            
            possestimer2 = PossessTimer();
            
            if (possestimer2 <= 0f)
            {
                StartCoroutine(ReturnControl());
            }
            else if (possestimer2 <= possessDuration - onStartPossessingCooldown && Input.GetKeyDown(KeyCode.E))
            {
                StartCoroutine(ReturnControl());
            }
        }

        if (!_isPossessed)
        {
            if (_playerInRange && cooldowntimer2 <= 0 && Input.GetKeyDown(KeyCode.E))
            {
                Possess();
            }
            
            cooldowntimer2 = UpdateCooldownTimer();
        }
    }

    private float UpdateCooldownTimer()
    {
        _cooldownTimer -= Time.deltaTime;
        return _cooldownTimer;
    }

    private void HandleObjectMovement()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        _rb.linearVelocity = new Vector2(moveX * moveSpeed, _rb.linearVelocity.y);

        if (moveX > 0) _sr.flipX = false;
        else if (moveX < 0) _sr.flipX = true;
    }

    private float PossessTimer()
    {
        _possessionTimer -= Time.deltaTime;
        return _possessionTimer;
    }
    private void Possess()
    {
        StartPossession(possessDuration);
        OnPossess?.Invoke();
    }

    private void StartPossession(float duration)
    {
        _possessionTimer = duration;
        _isPossessed = true;
    }

    private IEnumerator ReturnControl()
    {
        yield return null;
        EndPossession();
    }

    private void EndPossession()
    {
        if (!_isPossessed) return;
        
        _cooldownTimer = onEndPossessingCooldown;
        _isPossessed = false;
        _rb.linearVelocity = Vector2.zero;
        
        _player.gameObject.SetActive(true);
        _player.GetComponent<PlayerPossessing>().EndPossession(transform.position);
    }

    // --- Detección del jugador ---
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _playerInRange = true;
            _player = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _playerInRange = false;
        }
    }
}