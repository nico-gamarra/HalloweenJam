using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static event Action OnPlayerDeath;
    
    [SerializeField] private PlayerAnimations playerAnimations;
    [SerializeField] private GameObject cat;
    
    private void Awake()
    {
        PossessEvent.OnPossess += DeactivatePlayer;
        DeathEvent.OnPlayerDeath += DeactivatePlayer;
        FinalEvent.OnFinal += InstantiateCat;
        
        GameManager.instance.FadeInAnimation();
    }

    private void OnDestroy()
    {
        PossessEvent.OnPossess -= DeactivatePlayer;
        DeathEvent.OnPlayerDeath -= DeactivatePlayer;
        FinalEvent.OnFinal -= InstantiateCat;
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

    private void InstantiateCat()
    {
        Instantiate(cat, transform.position, Quaternion.identity);
    }

    public PlayerAnimations GetPlayerAnimations() => playerAnimations;
}
