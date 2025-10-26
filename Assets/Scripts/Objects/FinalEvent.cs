using System;
using UnityEngine;

public class FinalEvent : StateMachineBehaviour
{
    public static event Action OnFinal;

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        OnFinal?.Invoke();
    }
}