using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RoundState
{
    Start,
    Intro,
    Round
}

public class GameManager : MonoBehaviour
{
    public Transform Cherry;
    public Transform mainCamera;

    private float cherryInterval = 1f;
    private float cherryTime = 0;

    public AudioManager audioManager;
    public InputManager inputManager;

    public static RoundState currentRoundState;

    private void Awake()
    {
        currentRoundState = RoundState.Start;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (currentRoundState == RoundState.Start)
        {
                RunIntro();
                currentRoundState=RoundState.Intro;
        }
        else if(currentRoundState == RoundState.Intro)
        {
            if (audioManager.IntroPlayed())
            {
                AudioManager.currentBGMState = BGMState.Normal;
                currentRoundState = RoundState.Round;
            }
        }
        else if(currentRoundState == RoundState.Round)
        {
            RunRound();
        }
    }

    private void RunIntro()
    {
        audioManager.PlayIntro();
    }

    private void RunRound()
    {
        cherryTime += Time.deltaTime;
        if(cherryTime > cherryInterval)
        {
            SpawnCherry();
            cherryTime = 0;
        }

        if (inputManager.SpacePressed())
        {
            // audioManager.ChangeBGM();
        }

        audioManager.PlayBGM();
    }

    private void SpawnCherry()
    {
        Transform instance = Instantiate(Cherry);
        instance.GetComponent<CherryController>().tweener = GetComponent<Tweener>();
        instance.GetComponent<CherryController>().levelGenerator = GetComponent<LevelGenerator>();

        if(Random.Range(0, 2) == 1)
        {
            if(Random.Range(0,2) == 1)
            {
                instance.position = Camera.main.ViewportToWorldPoint(new Vector3(-0.1f, Random.Range(0.48f, 0.52f), 1f));
                instance.GetComponent<CherryController>().SetMovement = Movement.MoveRight;
            }
            else
            {
                instance.position = Camera.main.ViewportToWorldPoint(new Vector3(1.1f, Random.Range(0.48f, 0.52f), 1f));
                instance.GetComponent<CherryController>().SetMovement = Movement.MoveLeft;
            }
        }
        else
        {
            if (Random.Range(0, 2) == 1)
            {
                instance.position = Camera.main.ViewportToWorldPoint(new Vector3(Random.Range(0.48f, 0.52f), -0.1f, 1f));
                instance.GetComponent<CherryController>().SetMovement = Movement.MoveUp;
            }
            else
            {
                instance.position = Camera.main.ViewportToWorldPoint(new Vector3(Random.Range(0.48f, 0.52f), 1.1f, 1f));
                instance.GetComponent<CherryController>().SetMovement = Movement.MoveDown;
            }
        }
    }
}
