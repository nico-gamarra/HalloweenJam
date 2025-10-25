using System;
using UnityEngine;

public class PlayerPossessing : MonoBehaviour
{
    public static event Action<float> OnPossessEnd;
    public static event Action OnPossessStart;

    [Header("Config")]
    [SerializeField] private PlayerController playerController;
    [SerializeField] private float invincibilityDuration;
    
    private bool _isPossessing;

    private void Awake()
    {
        PossessableObject.OnPossess += PossessObject;
    }

    private void OnDestroy()
    {
        PossessableObject.OnPossess -= PossessObject;
    }

    private void PossessObject()
    {
        if (_isPossessing) return;

        _isPossessing = true;

        playerController.GetPlayerAnimations().PossessAnimation();
        OnPossessStart?.Invoke();
    }

    public void EndPossession(Vector3 newPosition)
    {
        // Reactivar al fantasma
        _isPossessing = false;
        transform.position = newPosition;
        OnPossessEnd?.Invoke(invincibilityDuration);
    }
}
