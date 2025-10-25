using System;
using UnityEngine;
using System.Collections;

public class PossessableObject : MonoBehaviour
{
    public static event Action OnPossess;
    
    [Header("Config")]
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float possessDuration;
    
    private bool _isPossessed;
    private float _possessionTimer;
    
    private bool _playerInRange;
    private GameObject _player;
    
    private Rigidbody2D _rb;
    private SpriteRenderer _sr;

    private void Awake()
    {
        _rb = GetComponentInParent<Rigidbody2D>();
        _sr = GetComponentInParent<SpriteRenderer>();
    }

    void Update()
    {
        if (_playerInRange && !_isPossessed && Input.GetKeyDown(KeyCode.E))
        {
            Possess();
        }

        if (_isPossessed)
        {
            if (PossessTimer() <= 0)
            {
                StartCoroutine(ReturnControl());
            }
            else
                HandleObjectMovement();
        }
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
        _rb.bodyType = RigidbodyType2D.Dynamic;//Cambiar por lol de la masa
        StartPossession(possessDuration);
        OnPossess?.Invoke();
    }

    private void StartPossession(float duration)
    {
        _isPossessed = true;
        _possessionTimer = duration;
    }

    private IEnumerator ReturnControl()
    {
        yield return null;
        EndPossession();
    }

    private void EndPossession()
    {
        if (!_isPossessed) return;

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