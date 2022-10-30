using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PacState
{
    Moving,
    Waiting,
    Dead,
}

public class PacStudentController : MonoBehaviour
{
    public Tweener tweener;
    public LevelGenerator levelGenerator;
    public AudioManager audioManager;
    public GameManager gameManager;
    public ParticleSystem dirt;
    public ParticleSystem hitParticle;

    public Vector3 spawnPosition;

    private AnimationStateController animStateController;
    private BoxCollider2D boxCollider;

    [SerializeField]
    public float MoveTime = 0.3f;
    private float moveTime = 0.3f;

    private float respawnInterval = 3f;
    private float respawnTime = 0f;

    Movement lastInput = Movement.MoveRight;
    Movement currentInput = Movement.MoveRight;

    PacState state = PacState.Dead;

    private void Start()
    {
        transform.position = spawnPosition;
        animStateController = GetComponent<AnimationStateController>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.currentRoundState != RoundState.Round)
        {
            animStateController.StopAnimation();
            return;
        }

        if (state == PacState.Dead)
        {
            WaitForRespawn();
        }

        if (state == PacState.Moving || state == PacState.Waiting)
        {
            ReadPlayerInput();

            if (currentInput == Movement.None)
            {
                currentInput = lastInput;
            }

            if (lastInput != Movement.None || currentInput != Movement.None)
            {
                animStateController.PlayAnimation();
                state = PacState.Moving;
            }
        }

        if (state == PacState.Moving)
        {
            MovePlayer();
        }
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
                SpawnDirt(lastInput);
                AdjustBox(lastInput);
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
                    SpawnDirt(currentInput);
                    AdjustBox(currentInput);
                    dirt.Play();
                    TweenPlayer(nextPosition);
                    animStateController.ChangeAnimationState(currentInput.ToString());
                    PlayMovementAudioClip(nextPosition);
                }
                //else
                //{
                //    if (animStateController.IsPlaying()) PlayMovementAudioClip(nextPosition);
                //    animStateController.StopAnimation();
                //}
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
        //// Cross Reference with Grid
        //int[,] levelMap = levelGenerator.GetLeveLMap();

        //if (nextPosition.x < 0 || -nextPosition.y < 0 ||
        //    nextPosition.x >= levelMap.GetLength(1) || -nextPosition.y >= levelMap.GetLength(0))
        //{
        //    return;
        //}


        //if (levelMap[-nextPosition.y, nextPosition.x] == 0)
        //{
        audioManager.PlayAudioClip("Movement");
        //}
        //else if (levelMap[-nextPosition.y, nextPosition.x] == 5)
        //{
        //    audioManager.PlayAudioClip("Pickup_Coin");
        //}
        //else if (levelMap[-nextPosition.y, nextPosition.x] == 6)
        //{
        //    audioManager.PlayAudioClip("Pickup_Powerup");
        //}
        //else if (levelMap[-nextPosition.y, nextPosition.x] == 1 ||
        //    levelMap[-nextPosition.y, nextPosition.x] == 2 ||
        //    levelMap[-nextPosition.y, nextPosition.x] == 3 ||
        //    levelMap[-nextPosition.y, nextPosition.x] == 4)
        //{
        //    audioManager.PlayAudioClip("Hit_Wall");
        //}
    }

    private void SpawnDirt(Movement input)
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

    private void SpawnHit(Movement input, Vector2 hitPosition)
    {
        switch (input)
        {
            case Movement.MoveLeft:
                Instantiate(hitParticle, hitPosition, Quaternion.Euler(0, 90, 0));
                break;
            case Movement.MoveRight:
                Instantiate(hitParticle, hitPosition, Quaternion.Euler(180, 90, 0));
                break;
            case Movement.MoveUp:
                Instantiate(hitParticle, hitPosition, Quaternion.Euler(90, 90, 0));
                break;
            case Movement.MoveDown:
                Instantiate(hitParticle, hitPosition, Quaternion.Euler(270, 90, 0));
                break;
        }
    }


    private void AdjustBox(Movement input)
    {
        switch (input)
        {
            case Movement.MoveLeft:
                boxCollider.offset = new Vector2(-0.1f, 0f);
                boxCollider.size = new Vector2(1f, 0.8f);
                break;
            case Movement.MoveRight:
                boxCollider.offset = new Vector2(0.1f, 0f);
                boxCollider.size = new Vector2(1f, 0.8f);
                break;
            case Movement.MoveUp:
                boxCollider.offset = new Vector2(0f, 0.1f);
                boxCollider.size = new Vector2(0.8f, 1f);
                break;
            case Movement.MoveDown:
                boxCollider.offset = new Vector2(0f, -0.1f);
                boxCollider.size = new Vector2(0.8f, 1f);
                break;
            default:
                boxCollider.offset = new Vector2(0f, 0f);
                boxCollider.size = new Vector2(0.1f, 0.1f);
                break;
        }
    }

    private void WaitForRespawn()
    {
        if (respawnTime > 0f)
        {
            respawnTime -= Time.deltaTime;
        }
        else
        {
            transform.position = spawnPosition;
            animStateController.ChangeAnimationState(Movement.MoveRight.ToString());
            lastInput = Movement.None;
            currentInput = Movement.None;
            state = PacState.Waiting;
            AdjustBox(lastInput);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (state != PacState.Moving) return;

        if (collision.tag == "Wall")
        {
            if (lastInput == currentInput)
            {
                Instantiate(hitParticle, collision.ClosestPoint(transform.position), Quaternion.Euler(0, 0, 0));
                audioManager.PlayAudioClip("Hit_Wall");
            }
        }

        if (collision.tag == "Teleporter")
        {
            if (state != PacState.Moving) return;

            Transform teleportPoint = collision.transform.GetComponent<TeleporterController>().TeleportPoint;
            transform.position = teleportPoint.position;
            tweener.RemoveTween(transform);
        }

        if (collision.tag == "Pellet")
        {
            if (state != PacState.Moving) return;

            gameManager.AddScore(10);
            audioManager.PlayAudioClip("Pickup_Coin");
            Destroy(collision.gameObject);
        }

        if (collision.tag == "BonusPellet")
        {
            if (state != PacState.Moving) return;

            gameManager.AddScore(100);
            audioManager.PlayAudioClip("Pickup_Coin");
            tweener.RemoveTween(collision.transform);
            Destroy(collision.gameObject);
        }

        if (collision.tag == "PowerPellet")
        {
            if (state != PacState.Moving) return;

            audioManager.PlayAudioClip("Pickup_Powerup");
            gameManager.ScareGhosts();
            Destroy(collision.gameObject);
        }

        if (collision.tag == "Walking")
        {
            if (state != PacState.Moving) return;

            audioManager.PlayAudioClip("Death_Sound");
            animStateController.ChangeAnimationState("Death");
            tweener.RemoveTween(transform);
            state = PacState.Dead;
            respawnTime = respawnInterval;
            gameManager.ReduceLife();
        }

        if (collision.tag == "Scared")
        {
            if (state != PacState.Moving) return;

            audioManager.PlayAudioClip("Death_Sound");
            tweener.RemoveTween(collision.transform);
            gameManager.AddScore(300);
            collision.transform.GetComponent<GhostController>().RunDeadTimer();

            AudioManager.currentBGMState = BGMState.Eaten;
        }
    }
}
