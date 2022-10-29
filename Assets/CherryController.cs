using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CherryController : MonoBehaviour
{
    public Movement SetMovement = Movement.MoveLeft;
    public Tweener tweener;
    public LevelGenerator levelGenerator;

    private bool beenInMap = false;

    private float tweenTime = 0.2f;

    private void Start()
    {
        tweenTime = Random.Range(0.2f, 0.3f);
    }

    // Update is called once per frame
    void Update()
    {
        MoveCherry();
    }

    private void MoveCherry()
    {
        if (!tweener.TweenExists(transform))
        {
            Vector3 screenPoint = Camera.main.WorldToViewportPoint(transform.position);
            bool onScreen = screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;

            int[,] levelMap = levelGenerator.GetLeveLMap();
            Vector3 position = transform.position;
            bool onMap = position.x > -1 && position.x < levelMap.GetLength(1) + 1 && position.y < 1 && position.y > -(levelMap.GetLength(1) + 1);

            if (onMap)
            {
                beenInMap = true;
            }

            if (!onScreen && !onMap && beenInMap)
            {
                Destroy(gameObject);
                return;
            }

            Vector2 nextPosition = CalculateNextPosition(SetMovement, transform.position);
            tweener.AddTween(transform, transform.localPosition, new Vector2(nextPosition.x, nextPosition.y), tweenTime);
        }
    }

    private Vector2 CalculateNextPosition(Movement input, Vector2 currentPosition)
    {
        switch (input)
        {
            case Movement.MoveLeft:
                return new Vector2(currentPosition.x - 1, currentPosition.y);
            case Movement.MoveRight:
                return new Vector2(currentPosition.x + 1, currentPosition.y);
            case Movement.MoveUp:
                return new Vector2(currentPosition.x, currentPosition.y + 1);
            case Movement.MoveDown:
                return new Vector2(currentPosition.x, currentPosition.y - 1);
            default:
                return currentPosition;
        }
    }
}
