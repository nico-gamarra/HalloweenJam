using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DeadZone : MonoBehaviour
{
    [Header("Detección")] 
    [SerializeField] private Light2D lightSource;
    [SerializeField] private int rays = 30;
    
    [Header("Debug")]
    [SerializeField] private bool showDebug = true;
    
    private float _range = 5f; // hasta dónde llegan los rayos
    private float _angle = 45f; // ángulo del cono de luz
    
    private bool _isInvencible;
    private float _timeLeft;

    private void OnEnable()
    {
        PlayerPossessing.OnPossessEnd += Invincibility;
    }

    private void OnDisable()
    {
        PlayerPossessing.OnPossessEnd -= Invincibility;
    }

    private void Start()
    {
        _range = lightSource.pointLightOuterRadius;
        _angle = lightSource.pointLightInnerAngle;
    }

    private void Update()
    {
        TimeLeft();
        
        if (_isInvencible) return;
        DetectPlayer();
    }

    void DetectPlayer()
    {
        bool playerDetected = false;

        // ángulo inicial para repartir los rayos
        float startAngle = -_angle * 0.5f;
        float step = _angle / rays;

        for (int i = 0; i <= rays; i++)
        {
            float currentAngle = startAngle + step * i;
            Vector2 dir = Quaternion.Euler(0, 0, currentAngle) * transform.right;

            RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, _range);

            if (showDebug)
                Debug.DrawRay(transform.position, dir * _range, Color.yellow);

            if (hit.collider && hit.collider.CompareTag("Player"))
            {
                playerDetected = true;
                hit.collider.GetComponent<PlayerController>()?.IsDead();
                gameObject.SetActive(false);
                break;
            }
        }

        if (playerDetected)
        {
            Debug.DrawRay(transform.position, transform.right * _range, Color.red);
        }
    }

    private void Invincibility(float time)
    {
        _timeLeft = time;
        _isInvencible = true;
    }

    private void TimeLeft()
    {
        _timeLeft -= Time.deltaTime;
        if (_timeLeft <= 0)
            _isInvencible = false;
    }
}
