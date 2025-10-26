using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static event Action OnPlayerDeath;
    
    [SerializeField] private PlayerAnimations playerAnimations;
    
    private void Awake()
    {
        PossessEvent.OnPossess += DeactivatePlayer;
        DeathEvent.OnPlayerDeath += DeactivatePlayer;
    }

    private void OnDestroy()
    {
        PossessEvent.OnPossess -= DeactivatePlayer;
        DeathEvent.OnPlayerDeath -= DeactivatePlayer;
    }
    
    public void IsDead()
    {
        playerAnimations.DeathAnimation();
        GameManager.instance.GetAudioManager().PlayAudio(AudioManager.AudioList.Fire);
        GameManager.instance.GetAudioManager().PlayAudio(AudioManager.AudioList.Death);
        OnPlayerDeath?.Invoke();
    }

    private void DeactivatePlayer()
    {
        gameObject.SetActive(false);
    }

    public PlayerAnimations GetPlayerAnimations() => playerAnimations;
}
