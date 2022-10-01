using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Movement
{
    MoveLeft,
    MoveRight,
    MoveUp,
    MoveDown,
}

public class CharacterManager : MonoBehaviour
{
    public InputManager input;
    public Tweener tweener;
    private Animator animator;

    public List<Movement> movementList = new List<Movement>();
    private Movement currentMovement = Movement.MoveUp;
    private int nextMoveIndex = 0;

    public float moveTime = 3f;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // For Testing Purposes Only
        // ReadInput();

        if (!tweener.TweenExists(transform))
        {
            if(nextMoveIndex >= movementList.Count) nextMoveIndex = 0;

            Vector2 position = transform.position;
            Movement nextMove = movementList[nextMoveIndex];

            switch (nextMove)
            {
                case Movement.MoveLeft:
                    tweener.AddTween(transform, transform.position, new Vector2(position.x - 1, position.y), moveTime);
                    break;
                case Movement.MoveRight:
                    tweener.AddTween(transform, transform.position, new Vector2(position.x + 1, position.y), moveTime);
                    break;
                case Movement.MoveUp:
                    tweener.AddTween(transform, transform.position, new Vector2(position.x, position.y + 1), moveTime);
                    break;
                case Movement.MoveDown:
                    tweener.AddTween(transform, transform.position, new Vector2(position.x, position.y - 1), moveTime);
                    break;
            }

            if(nextMove != currentMovement) animator.SetTrigger(nextMove.ToString());
            currentMovement = nextMove;
            nextMoveIndex++;
        }
    }

    private void ReadInput()
    {
        if (input.LeftKeyPressed())
        {
            animator.SetTrigger("MoveLeft");
        }
        else if (input.UpKeyPressed())
        {
            animator.SetTrigger("MoveUp");
        }
        else if (input.RightKeyPressed())
        {
            animator.SetTrigger("MoveRight");
        }
        else if (input.DownKeyPressed())
        {
            animator.SetTrigger("MoveDown");
        }
        else if (input.EnterKeyPressed())
        {
            animator.SetTrigger("Death");
        }
        else
        {
            animator.ResetTrigger("MoveLeft");
            animator.ResetTrigger("MoveUp");
            animator.ResetTrigger("MoveRight");
            animator.ResetTrigger("MoveDown");
            animator.ResetTrigger("Death");
        }
    }

    private void ResetTrigger()
    {

    }
}
