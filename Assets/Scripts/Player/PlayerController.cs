using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static event Action OnPlayerDeath;

    [SerializeField] private PlayerAnimations playerAnimations;
    
    public void IsDead()
    {
        OnPlayerDeath?.Invoke();
    }
    
    public PlayerAnimations GetPlayerAnimations() => playerAnimations;
}
