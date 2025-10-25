using System;
using UnityEngine;

public class PossessEvent : StateMachineBehaviour
{
    public static event Action OnPossess;
    
    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        OnPossess?.Invoke();
    }
}
