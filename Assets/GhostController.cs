using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Behaviour
{
    One, Two, Three, Four
}

public enum GhostState
{
    Moving,
    Dead,
}

public class GhostController : MonoBehaviour
{
    public Tweener tweener;
    public LevelGenerator levelGenerator;
    public List<Vector2> Center;

    public Behaviour ghostBehaviour;
    private Behaviour defaultBehaviour;

    private Movement lastInput = Movement.MoveLeft;
    private Movement currentInput = Movement.MoveLeft;

    private float DeadInterval = 5f;
    private float DeadTimer = 0f;

    private AnimationStateController animStateController;



    private void Start()
    {
        animStateController = GetComponent<AnimationStateController>();
        defaultBehaviour = ghostBehaviour;
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

    private void ComputeInput()
    {
        if (ghostBehaviour == Behaviour.One)
        {
            lastInput = Movement.MoveUp;
        }
        else if (ghostBehaviour == Behaviour.Two)
        {
            lastInput = Movement.MoveLeft;
        }
        else if (ghostBehaviour == Behaviour.Three)
        {
            lastInput = Movement.MoveDown;
        }
        else if (ghostBehaviour == Behaviour.Four)
        {
            lastInput = Movement.MoveRight;
        }
    }

    private void Behaviour1()
    {

    }

    private void Behaviour2()
    {

    }

    private void Behaviour3()
    {

    }

    private void Behaviour4()
    {

    }

    private void TweenToCenter()
    {

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
