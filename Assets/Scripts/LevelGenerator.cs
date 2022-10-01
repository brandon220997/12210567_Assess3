using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    // Walls
    public GameObject OutsideWall;

    public GameObject OutsideCorner;

    public GameObject InsideWall;

    public GameObject InsideCorner;

    public GameObject TJunction;

    public GameObject Pellet;

    public GameObject PowerPellet;

    private GameObject map;

    private int[,] quadrant;
    private int[,] levelMap;

    // Start is called before the first frame update
    void Start()
    {
        LoadMapQuadrant();
        GenerateMap();
    }

    private void LoadMapQuadrant()
    {
        quadrant = new int[,]
        {
         {1,2,2,2,2,2,2,2,2,2,2,2,2,7},
         {2,5,5,5,5,5,5,5,5,5,5,5,5,4},
         {2,5,3,4,4,3,5,3,4,4,4,3,5,4},
         {2,6,4,0,0,4,5,4,0,0,0,4,5,4},
         {2,5,3,4,4,3,5,3,4,4,4,3,5,3},
         {2,5,5,5,5,5,5,5,5,5,5,5,5,5},
         {2,5,3,4,4,3,5,3,3,5,3,4,4,4},
         {2,5,3,4,4,3,5,4,4,5,3,4,4,3},
         {2,5,5,5,5,5,5,4,4,5,5,5,5,4},
         {1,2,2,2,2,1,5,4,3,4,4,3,0,4},
         {0,0,0,0,0,2,5,4,3,4,4,3,0,3},
         {0,0,0,0,0,2,5,4,4,0,0,0,0,0},
         {0,0,0,0,0,2,5,4,4,0,3,4,4,0},
         {2,2,2,2,2,1,5,3,3,0,4,0,0,0},
         {0,0,0,0,0,0,5,0,0,0,4,0,0,0},
         };
    }

    public void GenerateMap()
    {
        Debug.Log("Generate Map");

        map = GameObject.Find("AutoMap");
        if (map != null) DestroyImmediate(map);

        map = new GameObject("AutoMap");
        map.transform.position = new Vector2(30, -2);

        // TODO: Quadrant Black Magic
        LoadMapQuadrant();
        levelMap = new int[quadrant.GetLength(0) * 2 - 1, quadrant.GetLength(1) * 2];

        LoadTopLeft();
        LoadTopRight();
        LoadBottomLeft();
        LoadBottomRight();

        LoadSprites();
    }

    private void LoadSprites()
    {
        for (int r = 0; r < levelMap.GetLength(0); r++)
        {
            for (int c = 0; c < levelMap.GetLength(1); c++)
            {
                GameObject mapObject;

                switch (levelMap[r, c])
                {
                    case 1:
                        mapObject = Instantiate(OutsideCorner, map.transform, false);
                        mapObject.transform.localPosition = new Vector2(c, -r);

                        RotateOutsideCorner(mapObject, new Vector2Int(r, c));
                        break;
                    case 2:
                        mapObject = Instantiate(OutsideWall, map.transform, false);
                        mapObject.transform.localPosition = new Vector2(c, -r);

                        RotateOutsideWall(mapObject, new Vector2Int(r, c));

                        break;
                    case 3:
                        mapObject = Instantiate(InsideCorner, map.transform, false);
                        mapObject.transform.localPosition = new Vector2(c, -r);

                        RotateInsideCorner(mapObject, new Vector2Int(r, c));

                        break;
                    case 4:
                        mapObject = Instantiate(InsideWall, map.transform, false);
                        mapObject.transform.localPosition = new Vector2(c, -r);

                        RotateInsideWall(mapObject, new Vector2Int(r, c));

                        break;
                    case 5:
                        mapObject = Instantiate(Pellet, map.transform, false);
                        mapObject.transform.localPosition = new Vector2(c, -r);

                        break;
                    case 6:
                        mapObject = Instantiate(PowerPellet, map.transform, false);
                        mapObject.transform.localPosition = new Vector2(c, -r);

                        break;
                    case 7:
                        mapObject = Instantiate(TJunction, map.transform, false);
                        mapObject.transform.localPosition = new Vector2(c, -r);

                        RotateTJunction(mapObject, new Vector2Int(r, c));

                        break;
                }
            }
        }
    }


    private void LoadTopLeft()
    {
        for (int r = 0; r < quadrant.GetLength(0); r++)
        {
            for (int c = 0; c < quadrant.GetLength(1); c++)
            {
                levelMap[r, c] = quadrant[r, c];
            }
        }
    }

    private void LoadTopRight()
    {
        for (int r = 0; r < quadrant.GetLength(0); r++)
        {
            for (int c = 0; c < quadrant.GetLength(1); c++)
            {
                levelMap[r, c + quadrant.GetLength(1)] = quadrant[r, (quadrant.GetLength(1) - 1) - c];
            }
        }
    }

    private void LoadBottomLeft()
    {
        for (int r = 0; r < (quadrant.GetLength(0) - 1); r++)
        {
            for (int c = 0; c < quadrant.GetLength(1); c++)
            {
                levelMap[r + quadrant.GetLength(0), c] = quadrant[(quadrant.GetLength(0) - 2) - r, c];
            }
        }
    }

    private void LoadBottomRight()
    {
        for (int r = 0; r < (quadrant.GetLength(0) - 1); r++)
        {
            for (int c = 0; c < quadrant.GetLength(1); c++)
            {
                levelMap[r + quadrant.GetLength(0), c + quadrant.GetLength(1)] = quadrant[(quadrant.GetLength(0) - 2) - r, (quadrant.GetLength(1) - 1) - c];
            }
        }
    }

    private void RotateOutsideWall(GameObject gameObject, Vector2Int point)
    {
        Vector2Int left = new Vector2Int(point.x, point.y - 1);
        Vector2Int right = new Vector2Int(point.x, point.y + 1);
        Vector2Int up = new Vector2Int(point.x - 1, point.y);
        Vector2Int down = new Vector2Int(point.x + 1, point.y);

        Vector2Int leftUp = new Vector2Int(point.x - 1, point.y - 1);
        Vector2Int leftDown = new Vector2Int(point.x + 1, point.y - 1);
        Vector2Int rightUp = new Vector2Int(point.x - 1, point.y + 1);
        Vector2Int rightDown = new Vector2Int(point.x + 1, point.y + 1);

        if (up.y > -1 && down.y < levelMap.GetLength(1))
        {
            if (EmptyPelletOrPowerPellet(up.x, up.y) ||
                EmptyPelletOrPowerPellet(down.x, down.y))
            {
                gameObject.transform.localRotation = Quaternion.Euler(0, 0, 90);
            }
        }
    }

    private bool EmptyPelletOrPowerPellet(int x, int y)
    {
        if (x < 0 || x >= levelMap.GetLength(0)) return false;
        if (y < 0 || y >= levelMap.GetLength(1)) return false;

        return levelMap[x, y] == 0 || levelMap[x, y] == 5 || levelMap[x, y] == 6;
    }

    private bool OutsideWallCornerOrTJunction(int x, int y)
    {
        if (x < 0 || x >= levelMap.GetLength(0)) return false;
        if (y < 0 || y >= levelMap.GetLength(1)) return false;

        return levelMap[x, y] == 1 || levelMap[x, y] == 2 || levelMap[x, y] == 7;
    }

    private bool OutsideWallOrCorner(int x, int y)
    {
        if (x < 0 || x >= levelMap.GetLength(0)) return false;
        if (y < 0 || y >= levelMap.GetLength(1)) return false;

        return levelMap[x, y] == 1 || levelMap[x, y] == 2;
    }

    private bool InsideWallCornerOrTJunction(int x, int y)
    {
        if (x < 0 || x >= levelMap.GetLength(0)) return false;
        if (y < 0 || y >= levelMap.GetLength(1)) return false;

        return levelMap[x, y] == 3 || levelMap[x, y] == 4 || levelMap[x, y] == 7;
    }

    private bool InsideWallOrCorner(int x, int y)
    {
        if (x < 0 || x >= levelMap.GetLength(0)) return false;
        if (y < 0 || y >= levelMap.GetLength(1)) return false;

        return levelMap[x, y] == 3 || levelMap[x, y] == 4;
    }

    private void RotateOutsideCorner(GameObject gameObject, Vector2Int point)
    {
        Vector2Int left = new Vector2Int(point.x, point.y - 1);
        Vector2Int right = new Vector2Int(point.x, point.y + 1);
        Vector2Int up = new Vector2Int(point.x - 1, point.y);
        Vector2Int down = new Vector2Int(point.x + 1, point.y);

        if (OutsideWallCornerOrTJunction(left.x, left.y) && OutsideWallCornerOrTJunction(down.x, down.y))
        {
            gameObject.transform.localRotation = Quaternion.Euler(0, 0, 270);
        }
        else if (OutsideWallCornerOrTJunction(left.x, left.y) && OutsideWallCornerOrTJunction(up.x, up.y))
        {
            gameObject.transform.localRotation = Quaternion.Euler(0, 0, 180);
        }
        else if (OutsideWallCornerOrTJunction(right.x, right.y) && OutsideWallCornerOrTJunction(up.x, up.y))
        {
            gameObject.transform.localRotation = Quaternion.Euler(0, 0, 90);
        }
    }

    private void RotateInsideWall(GameObject gameObject, Vector2Int point)
    {
        Vector2Int left = new Vector2Int(point.x, point.y - 1);
        Vector2Int right = new Vector2Int(point.x, point.y + 1);
        Vector2Int up = new Vector2Int(point.x - 1, point.y);
        Vector2Int down = new Vector2Int(point.x + 1, point.y);

        Vector2Int leftUp = new Vector2Int(point.x + 1, point.y - 1);
        Vector2Int leftDown = new Vector2Int(point.x - 1, point.y - 1);
        Vector2Int rightUp = new Vector2Int(point.x - 1, point.y + 1);
        Vector2Int rightDown = new Vector2Int(point.x + 1, point.y + 1);

        if (up.y > -1 && down.y < levelMap.GetLength(1))
        {
            if (EmptyPelletOrPowerPellet(up.x, up.y) ||
                EmptyPelletOrPowerPellet(down.x, down.y))
            {
                gameObject.transform.localRotation = Quaternion.Euler(0, 0, 90);
            }
        }
    }

    private void RotateInsideCorner(GameObject gameObject, Vector2Int point)
    {
        Vector2Int left = new Vector2Int(point.x, point.y - 1);
        Vector2Int right = new Vector2Int(point.x, point.y + 1);
        Vector2Int up = new Vector2Int(point.x - 1, point.y);
        Vector2Int down = new Vector2Int(point.x + 1, point.y);

        Vector2Int leftUp = new Vector2Int(point.x - 1, point.y - 1);
        Vector2Int leftDown = new Vector2Int(point.x + 1, point.y - 1);
        Vector2Int rightUp = new Vector2Int(point.x - 1, point.y + 1);
        Vector2Int rightDown = new Vector2Int(point.x + 1, point.y + 1);

        if (InsideWallCornerOrTJunction(left.x, left.y) && InsideWallCornerOrTJunction(down.x, down.y))
        {
            // Check Corner for Path
            if ((EmptyPelletOrPowerPellet(up.x, up.y) &&
                EmptyPelletOrPowerPellet(right.x, right.y) &&
                EmptyPelletOrPowerPellet(rightUp.x, rightUp.y)))
            {
                gameObject.transform.localRotation = Quaternion.Euler(0, 0, 270);
                return;
            }
            else if (EmptyPelletOrPowerPellet(leftDown.x, leftDown.y))
            {
                gameObject.transform.localRotation = Quaternion.Euler(0, 0, 270);
                return;
            }

        }

        if (InsideWallCornerOrTJunction(left.x, left.y) && InsideWallCornerOrTJunction(up.x, up.y))
        {
            // Check Corner for Path
            if (EmptyPelletOrPowerPellet(down.x, down.y) &&
                EmptyPelletOrPowerPellet(right.x, right.y) &&
                EmptyPelletOrPowerPellet(rightDown.x, rightDown.y))
            {
                gameObject.transform.localRotation = Quaternion.Euler(0, 0, 180);
                return;
            }
            else if (EmptyPelletOrPowerPellet(leftUp.x, leftUp.y))
            {
                gameObject.transform.localRotation = Quaternion.Euler(0, 0, 180);
                return;
            }
        }

        if (InsideWallCornerOrTJunction(right.x, right.y) && InsideWallCornerOrTJunction(up.x, up.y))
        {
            // Check Corner for Path
            if (EmptyPelletOrPowerPellet(down.x, down.y) &&
                EmptyPelletOrPowerPellet(left.x, left.y) &&
                EmptyPelletOrPowerPellet(leftDown.x, leftDown.y))
            {
                gameObject.transform.localRotation = Quaternion.Euler(0, 0, 90);
                return;
            }
            else if (EmptyPelletOrPowerPellet(rightUp.x, rightUp.y))
            {
                gameObject.transform.localRotation = Quaternion.Euler(0, 0, 90);
                return;
            }
        }

        if (InsideWallCornerOrTJunction(right.x, right.y) && InsideWallCornerOrTJunction(down.x, down.y))
        {
            // Check Corner for Path
            if (EmptyPelletOrPowerPellet(up.x, up.y) &&
                EmptyPelletOrPowerPellet(left.x, left.y) &&
                EmptyPelletOrPowerPellet(leftUp.x, leftUp.y))
            {
                gameObject.transform.localRotation = Quaternion.Euler(0, 0, 0);
                return;
            }
            else if (EmptyPelletOrPowerPellet(rightDown.x, rightDown.y))
            {
                gameObject.transform.localRotation = Quaternion.Euler(0, 0, 0);
                return;
            }
        }
    }

    private void RotateTJunction(GameObject gameObject, Vector2Int point)
    {
        Vector2Int left = new Vector2Int(point.x, point.y - 1);
        Vector2Int right = new Vector2Int(point.x, point.y + 1);
        Vector2Int up = new Vector2Int(point.x - 1, point.y);
        Vector2Int down = new Vector2Int(point.x + 1, point.y);

        Vector2Int leftUp = new Vector2Int(point.x - 1, point.y - 1);
        Vector2Int leftDown = new Vector2Int(point.x + 1, point.y - 1);
        Vector2Int rightUp = new Vector2Int(point.x - 1, point.y + 1);
        Vector2Int rightDown = new Vector2Int(point.x + 1, point.y + 1);

        // Vertical T Junction
        if (OutsideWallOrCorner(right.x, right.y) && InsideWallOrCorner(down.x, down.y))
        {
            gameObject.transform.localRotation = Quaternion.Euler(0, 180, 0);
        }
        else if (OutsideWallOrCorner(right.x, right.y) && InsideWallOrCorner(up.x, up.y))
        {
            gameObject.transform.localRotation = Quaternion.Euler(0, 0, 180);
        }
        else if (OutsideWallOrCorner(left.x, left.y) && InsideWallOrCorner(up.x, up.y))
        {
            gameObject.transform.localRotation = Quaternion.Euler(0, 180, 180);
        }

        // Horizontal T Junction
        else if (InsideWallOrCorner(right.x, right.y) && OutsideWallOrCorner(down.x, down.y))
        {
            gameObject.transform.localRotation = Quaternion.Euler(0, 0, 90);
        }
        else if (InsideWallOrCorner(right.x, right.y) && OutsideWallOrCorner(up.x, up.y))
        {
            gameObject.transform.localRotation = Quaternion.Euler(180, 0, 90);
        }
        else if (InsideWallOrCorner(left.x, left.y) && OutsideWallOrCorner(up.x, up.y))
        {
            gameObject.transform.localRotation = Quaternion.Euler(0, 0, 270);
        }
        else if (InsideWallOrCorner(left.x, left.y) && OutsideWallOrCorner(down.x, down.y))
        {
            gameObject.transform.localRotation = Quaternion.Euler(180, 0, 270);
        }
    }
}
