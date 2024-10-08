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


        if (phase == 1)
        {
            waitTime = 40;
        }
        else
        {
            waitTime = 20;
        }
        IEnumerator StallExecution()
        {
            yield return new WaitForSeconds(waitTime / 100);
        }
        int temp = Random.Range(0 + (phase == 1 ? 0: 10), 100);
        Controller.instance.bossOverlayRenderer.enabled = false;
        if (temp < 10)
        { 
            animator.SetTrigger("Run");
        }
        else
        {
            animator.SetTrigger("Cast");
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
    


    }

    //OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Run");
        animator.ResetTrigger("Cast");
    }
}
