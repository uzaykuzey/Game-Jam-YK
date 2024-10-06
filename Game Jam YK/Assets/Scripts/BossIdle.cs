using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossIdle : StateMachineBehaviour
{
    float waitTime = 50;
    BossActions behavior;
    float phase;
    //OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        behavior = animator.GetComponent<BossActions>();
        phase = behavior.phase;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (phase == 1)
        {
            waitTime = 50;
        }
        else if (phase == 2)
        {
            waitTime = 30;
        }
        else
        {
            waitTime = 25;
        }

        int temp = Random.Range(0, 2);
        if (temp == 0 || true)
        {
            animator.SetTrigger("Run");
        }
        else
        {
            animator.SetTrigger("Cast");
        }
    }

    //OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Run");
        animator.ResetTrigger("Cast");
    }
}
