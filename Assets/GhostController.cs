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
    public GameManager gameManager;
    public Transform Player;
    public Transform Teleporter1;
    public Transform Teleporter2;

    public List<Vector2> Center;
    public Vector3 spawnLocation;

    [SerializeField]
    public float MoveTime = 0.3f;
    private float moveTime = 0.3f;

    public Behaviour ghostBehaviour;
    private Behaviour defaultBehaviour;

    private Movement lastInput = Movement.MoveLeft;
    private Movement currentInput = Movement.MoveLeft;

    private Vector2Int previousPosition;
    private Vector2Int currentMovement;

    private GhostState state = GhostState.Moving;

    private float DeadInterval = 5f;
    private float DeadTimer = 0f;

    private AnimationStateController animStateController;

    private Vector2Int Behaviour4FocusPoint;



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

        if (!tweener.TweenExists(transform))
        {
            ComputeInput();

            if (state == GhostState.Moving)
            {
                animStateController.PlayAnimation();
                TweenGhost(currentMovement);

                if (gameManager.scaredTime > 0f)
                {
                    ghostBehaviour = Behaviour.One;
                }
                else
                {
                    ghostBehaviour = defaultBehaviour;
                }
            }

            if (state == GhostState.Dead)
            {
                if (spawnLocation != transform.position)
                {
                    TweenDeadGhost(new Vector2(spawnLocation.x, spawnLocation.y));
                }
                else
                {
                    if (gameManager.scaredTime > 0f)
                    {
                        tag = "Scared";
                        ghostBehaviour = Behaviour.One;
                    }
                    else
                    {
                        tag = "Walking";
                        ghostBehaviour = defaultBehaviour;
                    }

                    state = GhostState.Moving;
                }
            }
        }
    }

    private void ComputeInput()
    {
        if (EscapeFromCenter()) return;

        if (ghostBehaviour == Behaviour.One)
        {
            Behaviour1();
        }
        else if (ghostBehaviour == Behaviour.Two)
        {
            Behaviour2();
        }
        else if (ghostBehaviour == Behaviour.Three)
        {
            Behaviour3();
        }
        else if (ghostBehaviour == Behaviour.Four)
        {
            Behaviour4();
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

    private bool EscapeFromCenter()
    {
        // Move Further Away From Spawn
        Vector2Int position = new Vector2Int((int)transform.localPosition.x, (int)transform.localPosition.y);
        List<Vector2Int> vectors = new List<Vector2Int>();

        if (!IsCenter(position)) return false;

        List<Vector2Int> further = new List<Vector2Int>();
        List<Vector2Int> closer = new List<Vector2Int>();

        vectors.Add(CalculateNextPosition(Movement.MoveLeft, position));
        vectors.Add(CalculateNextPosition(Movement.MoveRight, position));
        vectors.Add(CalculateNextPosition(Movement.MoveUp, position));
        vectors.Add(CalculateNextPosition(Movement.MoveDown, position));

        for (int i = vectors.Count - 1; i > -1; i--)
        {
            if (!CanMovewithCenter(vectors[i]))
            {
                continue;
            }

            if (Mathf.Abs(Vector2.Distance(spawnLocation, vectors[i])) < Mathf.Abs(Vector2.Distance(spawnLocation, transform.position)))
            {
                if (previousPosition != vectors[i]) closer.Add(vectors[i]);
            }
            else
            {
                if (previousPosition != vectors[i]) further.Add(vectors[i]);
            }
        }

        if (further.Count == 0)
        {
            if (closer.Count == 0)
            {
                currentMovement = previousPosition;
            }
            else
            {
                currentMovement = closer[Random.Range(0, closer.Count)];
            }
        }
        else
        {
            currentMovement = further[Random.Range(0, further.Count)];
        }

        previousPosition = position;
        return IsCenter(position);
    }

    private void Behaviour1()
    {
        // Move Further Away
        Vector2Int position = new Vector2Int((int)transform.localPosition.x, (int)transform.localPosition.y);
        List<Vector2Int> vectors = new List<Vector2Int>();

        List<Vector2Int> further = new List<Vector2Int>();
        List<Vector2Int> closer = new List<Vector2Int>();

        vectors.Add(CalculateNextPosition(Movement.MoveLeft, position));
        vectors.Add(CalculateNextPosition(Movement.MoveRight, position));
        vectors.Add(CalculateNextPosition(Movement.MoveUp, position));
        vectors.Add(CalculateNextPosition(Movement.MoveDown, position));

        for (int i = vectors.Count - 1; i > -1; i--)
        {
            if (!CanMove(vectors[i]))
            {
                continue;
            }

            if (Mathf.Abs(Vector2.Distance(Player.transform.position, vectors[i])) < Mathf.Abs(Vector2.Distance(Player.transform.position, transform.position)))
            {
                if (previousPosition != vectors[i]) closer.Add(vectors[i]);
            }
            else
            {
                if (previousPosition != vectors[i]) further.Add(vectors[i]);
            }
        }

        if (further.Count == 0)
        {
            if (closer.Count == 0)
            {
                currentMovement = previousPosition;
            }
            else
            {
                currentMovement = closer[Random.Range(0, closer.Count)];
            }
        }
        else
        {
            currentMovement = further[Random.Range(0, further.Count)];
        }

        previousPosition = position;
    }

    private void Behaviour2()
    {
        // Move Closer to Player
        Vector2Int position = new Vector2Int((int)transform.localPosition.x, (int)transform.localPosition.y);
        List<Vector2Int> vectors = new List<Vector2Int>();

        List<Vector2Int> further = new List<Vector2Int>();
        List<Vector2Int> closer = new List<Vector2Int>();

        vectors.Add(CalculateNextPosition(Movement.MoveLeft, position));
        vectors.Add(CalculateNextPosition(Movement.MoveRight, position));
        vectors.Add(CalculateNextPosition(Movement.MoveUp, position));
        vectors.Add(CalculateNextPosition(Movement.MoveDown, position));

        for (int i = vectors.Count - 1; i > -1; i--)
        {
            if (!CanMove(vectors[i]))
            {
                continue;
            }

            if (Mathf.Abs(Vector2.Distance(Player.transform.position, vectors[i])) < Mathf.Abs(Vector2.Distance(Player.transform.position, transform.position)))
            {
                if (previousPosition != vectors[i]) closer.Add(vectors[i]);
            }
            else
            {
                if (previousPosition != vectors[i]) further.Add(vectors[i]);
            }
        }

        if (closer.Count == 0)
        {
            if (further.Count == 0)
            {
                currentMovement = previousPosition;
            }
            else
            {
                currentMovement = further[Random.Range(0, further.Count)];
            }
        }
        else
        {
            currentMovement = closer[Random.Range(0, closer.Count)];
        }

        previousPosition = position;
    }

    private void Behaviour3()
    {
        // Random Valid Direction
        Vector2Int position = new Vector2Int((int)transform.localPosition.x, (int)transform.localPosition.y);
        List<Vector2Int> vectors = new List<Vector2Int>();

        List<Vector2Int> total = new List<Vector2Int>();

        vectors.Add(CalculateNextPosition(Movement.MoveLeft, position));
        vectors.Add(CalculateNextPosition(Movement.MoveRight, position));
        vectors.Add(CalculateNextPosition(Movement.MoveUp, position));
        vectors.Add(CalculateNextPosition(Movement.MoveDown, position));

        for (int i = vectors.Count - 1; i > -1; i--)
        {
            if (!CanMove(vectors[i]))
            {
                continue;
            }

            if (previousPosition != vectors[i]) total.Add(vectors[i]);
        }

        if (total.Count == 0)
        {
            currentMovement = previousPosition;
        }
        else
        {
            currentMovement = total[Random.Range(0, total.Count)];
        }

        previousPosition = position;
    }

    private void Behaviour4()
    {
        // Move Clockwise
        Vector2Int position = new Vector2Int((int)transform.localPosition.x, (int)transform.localPosition.y);
        List<Vector2Int> vectors = new List<Vector2Int>();

        // Cross Reference with Grid
        int[,] levelMap = levelGenerator.GetLeveLMap();

        Vector2Int topleft = new Vector2Int(0, 0);
        Vector2Int topright = new Vector2Int(levelMap.GetLength(1) - 1, 0);
        Vector2Int bottomright = new Vector2Int(levelMap.GetLength(1) -1 , -(levelMap.GetLength(0) - 1));
        Vector2Int bottomleft = new Vector2Int(0, -(levelMap.GetLength(0) - 1));

        if(Vector2.Distance(position, Behaviour4FocusPoint) <= 2)
        {
            if(Behaviour4FocusPoint == topleft)
            {
                Behaviour4FocusPoint = topright;
            }
            else if (Behaviour4FocusPoint == topright)
            {
                Behaviour4FocusPoint = bottomright;
            }
            else if (Behaviour4FocusPoint == bottomright)
            {
                Behaviour4FocusPoint = bottomleft;
            }
            else if (Behaviour4FocusPoint == bottomleft)
            {
                Behaviour4FocusPoint = topleft;
            }
        }

        Vector2Int left = CalculateNextPosition(Movement.MoveLeft, position);
        Vector2Int right = CalculateNextPosition(Movement.MoveRight, position);
        Vector2Int up = CalculateNextPosition(Movement.MoveUp, position);
        Vector2Int down = CalculateNextPosition(Movement.MoveDown, position);

        if (Behaviour4FocusPoint == topleft)
        {
            //left
            //Up

            if (CanMove(left) && previousPosition != left)
            {
                currentMovement = left;
            }
            else if(CanMove(up) && previousPosition != up)
            {
                currentMovement = up;
            }
            else if (CanMove(down) && previousPosition != down)
            {
                currentMovement = down;
            }
            else
            {
                currentMovement = right;
            }
        }

        if (Behaviour4FocusPoint == topright)
        {
            //Up
            //Right

            if (CanMove(up) && previousPosition != up)
            {
                currentMovement = up;
            }
            else if(CanMove(right) && previousPosition != right)
            {
                currentMovement = right;
            }
            else if (CanMove(down) && previousPosition != down)
            {
                currentMovement = down;
            }
            else
            {
                currentMovement = left;
            }
        }

        if (Behaviour4FocusPoint == bottomright)
        {
            // Right
            // Down

            if (CanMove(right) && previousPosition != right)
            {
                currentMovement = right;
            }
            else if (CanMove(down) && previousPosition != down)
            {
                currentMovement = down;
            }
            else if (CanMove(up) && previousPosition != up)
            {
                currentMovement = up;
            }
            else
            {
                currentMovement = left;
            }
        }

        if (Behaviour4FocusPoint == bottomleft)
        {
            // Down
            // Left

            if (CanMove(down) && previousPosition != down)
            {
                currentMovement = down;
            }
            else if (CanMove(left) && previousPosition != left)
            {
                currentMovement = left;
            }
            else if (CanMove(up) && previousPosition != up)
            {
                currentMovement = up;
            }
            else
            {
                currentMovement = right;
            }
        }

        previousPosition = position;
    }

    private void TweenGhost(Vector2Int nextPosition)
    {
        // Movement Magic
        tweener.AddTween(transform, transform.localPosition, new Vector2(nextPosition.x, nextPosition.y), moveTime);
    }

    private void TweenDeadGhost(Vector2 nextPosition)
    {
        // Movement Magic
        tweener.AddTween(transform, transform.localPosition, new Vector2(nextPosition.x, nextPosition.y), DeadInterval);
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

        if (Vector2.Distance(nextPosition, Teleporter1.position) < 6)
        {
            return false;
        }

        if (Vector2.Distance(nextPosition, Teleporter2.position) < 6)
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

    private bool CanMovewithCenter(Vector2Int nextPosition)
    {
        // Cross Reference with Grid
        int[,] levelMap = levelGenerator.GetLeveLMap();

        if (nextPosition.x < 0 || -nextPosition.y < 0 ||
            nextPosition.x >= levelMap.GetLength(1) || -nextPosition.y >= levelMap.GetLength(0))
        {
            return false;
        }

        if (Vector2.Distance(Player.transform.position, Teleporter1.position) < 5)
        {
            return false;
        }

        if (Vector2.Distance(Player.transform.position, Teleporter2.position) < 5)
        {
            return false;
        }

        if (levelMap[-nextPosition.y, nextPosition.x] == 0 ||
            levelMap[-nextPosition.y, nextPosition.x] == 5 ||
            levelMap[-nextPosition.y, nextPosition.x] == 6 ||
            levelMap[-nextPosition.y, nextPosition.x] == 8)
        {
            return true;
        }

        return false;
    }

    private bool IsCenter(Vector2Int nextPosition)
    {
        // Cross Reference with Grid
        int[,] levelMap = levelGenerator.GetLeveLMap();

        return levelMap[-nextPosition.y, nextPosition.x] == 8;
    }

    public void RunDeadTimer()
    {
        tag = "Dead";
        state = GhostState.Dead;
        animStateController.ChangeAnimationState("Dead Shield");
    }

    public void SetToWalk()
    {
        tag = "Walking";
        animStateController.ChangeAnimationState("Shield Down");
    }

    public void SetToScared()
    {
        if (tag != "Dead")
        {
            tag = "Scared";
            animStateController.ChangeAnimationState("Damaged Shield");
        }
    }
}
