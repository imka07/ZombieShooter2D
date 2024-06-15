using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoosFalls : StateMachineBehaviour
{
    public ParticleSystem groundedEffect;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        var boss = FindObjectOfType<ZombieAI>();
        if (groundedEffect != null)
        {
            groundedEffect.Play();
            boss.PlayClips(2);
        }
        else
        {
            Debug.LogWarning("Grounded effect is not assigned.");
        }
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
