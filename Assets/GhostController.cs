using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostController : MonoBehaviour
{
    private float DeadInterval = 5f;
    private float DeadTimer = 0f;

    private AnimationStateController animStateController;

    private void Start()
    {
        animStateController = GetComponent<AnimationStateController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.currentRoundState != RoundState.Round)
        {
            animStateController.StopAnimation();
            return;
        }

        animStateController.PlayAnimation();

        if (tag == "Dead")
        {
            if (DeadTimer > 0f)
            {
                DeadTimer -= Time.deltaTime;
            }
            else
            {
                SetToWalk();
            }
        }
    }

    public void RunDeadTimer()
    {
        tag = "Dead";
        animStateController.ChangeAnimationState("Dead Shield");
        DeadTimer = DeadInterval;
    }

    public void SetToWalk()
    {
        tag = "Walking";
        animStateController.ChangeAnimationState("Shield Down");
    }

    public void SetToScared()
    {
        if(tag != "Dead")
        {
            tag = "Scared";
            animStateController.ChangeAnimationState("Damaged Shield");
        }
    }
}
