using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    [SerializeField] private Animator animator;

    public void ToggleRunAnimation(bool value)
    {
        animator.SetBool("isRunning", value);
    }
}
