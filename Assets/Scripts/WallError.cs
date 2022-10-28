using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WallError : MonoBehaviour
{
    Quaternion defaultRotation;
    Vector3 defaultScale;
    Image wallRenderer;
    Sprite wall1;

    [Range(1f, 5f)]
    public float WaitTime = 1f;

    [Range(1f, 2f)]
    public float RandomizeTime = 1f;

    private float actionTime = 0.2f;

    public List<Sprite> WallImages;

    private float[] rotations = { 0f, 90f, 180f, 270f };

    private float totalTime = 0f;
    private float currentTime = 0f;
    private float nextActionTime = 0f;

    private void Start()
    {
        wallRenderer = GetComponent<Image>();
        wall1 = wallRenderer.sprite;

        defaultRotation = transform.rotation;
        defaultScale = transform.localScale;
        totalTime = RandomizeTime + WaitTime;
    }

    private void Update()
    {
        RandomWallError();
    }

    private void RandomWallError()
    {
        currentTime += Time.deltaTime;
        if (currentTime > totalTime)
        {
            currentTime = 0f;
            nextActionTime = 0f;
        }

        if (currentTime > RandomizeTime)
        {
            if(currentTime >= nextActionTime)
            {
                wallRenderer.sprite = WallImages[Random.Range(0, WallImages.Count)];
                transform.rotation = Quaternion.Euler(0f, 0f, rotations[Random.Range(0, rotations.Length)]);
                transform.localScale = new Vector3(Random.Range(0.3f, 0.9f), Random.Range(0.2f, 0.7f), 1f);
                nextActionTime = nextActionTime + actionTime;
            }
        }
        else
        {
            wallRenderer.sprite = wall1;
            transform.rotation = defaultRotation;
            transform.localScale = defaultScale;
        }
    }
}
