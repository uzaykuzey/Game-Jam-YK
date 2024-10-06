using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastingBehavior : StateMachineBehaviour
{

    public float looking;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float cast = Random.Range(0.0f, 1.0f);
        looking = cast;
        /*if (cast < 0.4f)
        {
            animator.SetTrigger("Wave");
        }
        else */if (cast < 0.5f)
        {
            animator.SetTrigger("MagicBall");
        }
        else
        {
            animator.SetTrigger("Teleport");
        }


    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {


    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Teleport");
        animator.ResetTrigger("MagicBall");
        animator.ResetTrigger("Wave");
    }
}
