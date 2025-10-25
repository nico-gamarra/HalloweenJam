using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerAnimations playerAnimations;
    
    public void IsDead()
    {
        playerAnimations.DeathAnimation();
        //Que no se pueda mover
    }
    
    public PlayerAnimations GetPlayerAnimations() => playerAnimations;
}
