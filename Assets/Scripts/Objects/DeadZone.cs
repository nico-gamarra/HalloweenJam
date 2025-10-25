using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DeadZone : MonoBehaviour
{
    [Header("Detección")] 
    [SerializeField] private Light2D lightSource;
    [SerializeField] private int rays = 30;
    
    private float _range = 5f; // hasta dónde llegan los rayos
    private float _angle = 45f; // ángulo del cono de luz

    [Header("Debug")]
    [SerializeField] private bool showDebug = true;

    private void Start()
    {
        _range = lightSource.pointLightOuterRadius;
        _angle = lightSource.pointLightInnerAngle;
    }

    private void Update()
    {
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
}
