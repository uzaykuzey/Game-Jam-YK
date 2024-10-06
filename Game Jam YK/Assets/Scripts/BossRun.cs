using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRun : StateMachineBehaviour
{
    Transform animatorTransform;
    [SerializeField] private float cycleLengthX = 2;
    [SerializeField] private float HorizontalPos = 10;
    //OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animatorTransform = animator.GetComponent<Transform>();


        animatorTransform.DOMoveX(10, cycleLengthX).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animatorTransform.DOKill();
    }

}
