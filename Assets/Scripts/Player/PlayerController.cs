using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static event Action OnPlayerDeath;
    
    [SerializeField] private PlayerAnimations playerAnimations;
    
    private void OnEnable()
    {
        PossessEvent.OnPossess += DeactivatePlayer;
    }

    private void OnDisable()
    {
        PossessEvent.OnPossess -= DeactivatePlayer;
    }
    
    public void IsDead()
    {
        playerAnimations.DeathAnimation();
        OnPlayerDeath?.Invoke();
    }

    private void DeactivatePlayer()
    {
        gameObject.SetActive(false);
    }

    public PlayerAnimations GetPlayerAnimations() => playerAnimations;
}
