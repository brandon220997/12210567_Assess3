using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationStateController : MonoBehaviour
{
    Animator animator;
    private string currentState = "";

    private void Start()
    {
        animator = GetComponent<Animator>();        
    }

    public void ChangeAnimationState(string animState)
    {
        if (currentState == animState) return;
        animator.Play(animState);
        currentState = animState;
    }

    public bool IsPlaying()
    {
        return animator.speed != 0f;
    }
   
    public void PlayAnimation()
    {
        animator.speed = 1f;
    }

    public void StopAnimation()
    {
        animator.speed = 0f;
    }
}
