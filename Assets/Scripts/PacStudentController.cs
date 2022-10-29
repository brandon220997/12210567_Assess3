using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacStudentController : MonoBehaviour
{
    public Tweener tweener;
    public LevelGenerator levelGenerator;
    public AudioManager audioManager;
    public ParticleSystem dirt;

    private AnimationStateController animStateController;

    [SerializeField]
    public float MoveTime = 0.3f;
    private float moveTime = 0.3f;

    Movement lastInput;
    Movement currentInput;

    private void Start()
    {
        animStateController = GetComponent<AnimationStateController>();
    }

    // Update is called once per frame
    void Update()
    {
        ReadPlayerInput();
        MovePlayer();
    }

    private void ReadPlayerInput()
    {
        if (Input.GetKey(KeyCode.W))
        {
            lastInput = Movement.MoveUp;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            lastInput = Movement.MoveLeft;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            lastInput = Movement.MoveDown;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            lastInput = Movement.MoveRight;
        }
    }

    private void MovePlayer()
    {
        if (!tweener.TweenExists(transform))
        {
            Vector2Int position = new Vector2Int(int.Parse(transform.localPosition.x.ToString()), int.Parse(transform.localPosition.y.ToString()));
            Vector2Int nextPosition = CalculateNextPosition(lastInput, position);

            // if can move, use lastinput

            if (CanMove(nextPosition))
            {
                animStateController.PlayAnimation();
                RotateDirt(lastInput);
                dirt.Play();
                TweenPlayer(nextPosition);
                animStateController.ChangeAnimationState(lastInput.ToString());
                currentInput = lastInput;
                PlayMovementAudioClip(nextPosition);
            }
            else
            {
                nextPosition = CalculateNextPosition(currentInput, position);
                if (CanMove(nextPosition))
                {
                    animStateController.PlayAnimation();
                    RotateDirt(currentInput);
                    dirt.Play();
                    TweenPlayer(nextPosition);
                    animStateController.ChangeAnimationState(currentInput.ToString());
                    PlayMovementAudioClip(nextPosition);
                }
                else
                {
                    if (animStateController.IsPlaying()) PlayMovementAudioClip(nextPosition);
                    animStateController.StopAnimation();
                }
            }
        }
    }

    private Vector2Int CalculateNextPosition(Movement input, Vector2Int currentPosition)
    {
        switch (input)
        {
            case Movement.MoveLeft:
                return new Vector2Int(currentPosition.x - 1, currentPosition.y);
            case Movement.MoveRight:
                return new Vector2Int(currentPosition.x + 1, currentPosition.y);
            case Movement.MoveUp:
                return new Vector2Int(currentPosition.x, currentPosition.y + 1);
            case Movement.MoveDown:
                return new Vector2Int(currentPosition.x, currentPosition.y - 1);
            default:
                return currentPosition;
        }
    }

    private bool CanMove(Vector2Int nextPosition)
    {
        // Cross Reference with Grid
        int[,] levelMap = levelGenerator.GetLeveLMap();

        if (nextPosition.x < 0 || -nextPosition.y < 0 ||
            nextPosition.x >= levelMap.GetLength(1) || -nextPosition.y >= levelMap.GetLength(0))
        {
            return false;
        }

        if (levelMap[-nextPosition.y, nextPosition.x] == 0 ||
            levelMap[-nextPosition.y, nextPosition.x] == 5 ||
            levelMap[-nextPosition.y, nextPosition.x] == 6)
        {
            return true;
        }

        return false;
    }

    private void TweenPlayer(Vector2Int nextPosition)
    {
        // Movement Magic
        tweener.AddTween(transform, transform.localPosition, new Vector2(nextPosition.x, nextPosition.y), moveTime);
    }

    private void PlayMovementAudioClip(Vector2Int nextPosition)
    {
        // Cross Reference with Grid
        int[,] levelMap = levelGenerator.GetLeveLMap();

        if (nextPosition.x < 0 || -nextPosition.y < 0 ||
            nextPosition.x >= levelMap.GetLength(1) || -nextPosition.y >= levelMap.GetLength(0))
        {
            return;
        }


        if (levelMap[-nextPosition.y, nextPosition.x] == 0)
        {
            audioManager.PlayAudioClip("Movement");
        }
        else if (levelMap[-nextPosition.y, nextPosition.x] == 5)
        {
            audioManager.PlayAudioClip("Pickup_Coin");
        }
        else if (levelMap[-nextPosition.y, nextPosition.x] == 6)
        {
            audioManager.PlayAudioClip("Pickup_Powerup");
        }
        else if (levelMap[-nextPosition.y, nextPosition.x] == 1 ||
            levelMap[-nextPosition.y, nextPosition.x] == 2 ||
            levelMap[-nextPosition.y, nextPosition.x] == 3 ||
            levelMap[-nextPosition.y, nextPosition.x] == 4)
        {
            audioManager.PlayAudioClip("Hit_Wall");
        }
    }

    private void RotateDirt(Movement input)
    {
        switch (input)
        {
            case Movement.MoveLeft:
                Instantiate(dirt, transform.position, Quaternion.Euler(0, 90, 0));
                break;
            case Movement.MoveRight:
                Instantiate(dirt, transform.position, Quaternion.Euler(180, 90, 0));
                break;
            case Movement.MoveUp:
                Instantiate(dirt, transform.position, Quaternion.Euler(90, 90, 0));
                break;
            case Movement.MoveDown:
                Instantiate(dirt, transform.position, Quaternion.Euler(270, 90, 0));
                break;
        }
    }
}
