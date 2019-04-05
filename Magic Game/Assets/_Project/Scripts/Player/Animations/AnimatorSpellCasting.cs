using UnityEngine;

public class AnimatorSpellCasting : StateMachineBehaviour
{
    // OnStateMachineExit is called when exiting a state machine via its Exit Node
    override public void OnStateMachineExit(Animator animator, int stateMachinePathHash)
    {
        animator.SetBool("Cast Spell", false);
    }
}
