using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public InputManager input;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        ReadInput();
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
}
