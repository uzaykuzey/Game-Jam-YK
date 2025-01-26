using UnityEngine;

public class CastingBehavior : StateMachineBehaviour
{
    public float looking;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float cast = Random.Range(0.0f, 1.0f);
        looking = cast;
        Controller.instance.bossOverlayRenderer.enabled = true;
        if(!animator.GetComponent<BossScript>().hasShield && (animator.transform.position - Controller.instance.player.transform.position).magnitude < 4)
        {
            if(Random.Range(0,3)==1)
            {
                Controller.instance.bossOverlayRenderer.color = Color.red;
                animator.SetTrigger("Teleport");
                return;
            }
        }
        if (cast < 0.2f)
        {
            Controller.instance.bossOverlayRenderer.color = Color.red;
            animator.SetTrigger("Teleport");
        }
        else if (cast < 0.6f)
        {
            Controller.instance.bossOverlayRenderer.color = Color.green;
            animator.SetTrigger("MagicBall");
        }
        else
        {
            Controller.instance.bossOverlayRenderer.color = Color.yellow;
            animator.SetTrigger("Wave");
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
