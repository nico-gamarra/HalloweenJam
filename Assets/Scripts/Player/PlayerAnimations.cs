using System;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    public static event Action OnFinal;
    
    [SerializeField] private Animator animator;

    public void ToggleRunAnimation(bool value)
    {
        animator.SetBool("isRunning", value);
    }

    public void DeathAnimation()
    {
        animator.SetTrigger("Death");
    }

    public void PossessAnimation()
    {
        animator.SetTrigger("Possess");
    }

    public void FinalAnimation()
    {
        animator.SetTrigger("Final");
        OnFinal?.Invoke();
    }
}
