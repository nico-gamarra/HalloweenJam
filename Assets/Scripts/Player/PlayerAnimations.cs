using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
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
}
