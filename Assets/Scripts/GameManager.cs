using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RoundState
{
    Intro,
    Round,
    Over,
}

public class GameManager : MonoBehaviour
{
    public Transform Cherry;
    public Transform mainCamera;

    private float cherryInterval = 10f;
    private float cherryTime = 0;

    private float scaredInterval = 10f;
    private float scaredTime = 0f;

    private float startInterval = 3f;
    private float startTimer = 0f;

    private float highScoreTimer = 0f;

    public AudioManager audioManager;
    public InputManager inputManager;
    public HUDManager hudManager;

    public List<Transform> Ghosts;

    public static RoundState currentRoundState;

    private int totalScore = 0;
    private int lifeCount = 3;

    private void Awake()
    {
        currentRoundState = RoundState.Intro;
        startTimer = startInterval;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (currentRoundState == RoundState.Intro)
        {
            RunIntro();
        }

        if (currentRoundState == RoundState.Round)
        {
            RunRound();
        }

        if (currentRoundState == RoundState.Over)
        {
            // RunRound();
        }
    }

    private void RunIntro()
    {
        if (Mathf.Ceil(startTimer) >= 0f)
        {
            if(!audioManager.introStarted) audioManager.PlayIntro();
            hudManager.SetIntro(Mathf.Ceil(startTimer));
            startTimer -= Time.deltaTime;
        }
        else
        {
            hudManager.HideIntro();
            currentRoundState = RoundState.Round;
        }
    }

    private void RunRound()
    {
        if(lifeCount > 0f)
        {
            highScoreTimer += Time.deltaTime;
            hudManager.DisplayTime(highScoreTimer);
        }

        hudManager.SetScareTime(Mathf.Ceil(scaredTime));

        if (scaredTime <= 3 && scaredTime > 0)
        {
            Ghosts.ForEach(g =>
            {
                if (g.tag == "Scared")
                {
                    g.GetComponent<AnimationStateController>().ChangeAnimationState("Shield Recovery");
                }
            });
        }

        if (scaredTime > 0)
        {
            scaredTime -= Time.deltaTime;
        }
        else
        {
            Ghosts.ForEach(g =>
            {
                if (g.tag != "Dead")
                {
                    g.GetComponent<GhostController>().SetToWalk();
                }
            });

            AudioManager.currentBGMState = BGMState.Normal;
        }

        cherryTime += Time.deltaTime;
        if (cherryTime > cherryInterval)
        {
            SpawnCherry();
            cherryTime = 0;
        }

        audioManager.PlayBGM();
    }

    private void SpawnCherry()
    {
        Transform instance = Instantiate(Cherry);
        instance.GetComponent<CherryController>().tweener = GetComponent<Tweener>();
        instance.GetComponent<CherryController>().levelGenerator = GetComponent<LevelGenerator>();

        if (Random.Range(0, 2) == 1)
        {
            if (Random.Range(0, 2) == 1)
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

    public void AddScore(int score)
    {
        totalScore += score;
        hudManager.SetScore(totalScore);
    }

    public void ScareGhosts()
    {
        scaredTime = scaredInterval;

        Ghosts.ForEach(g =>
        {
            g.GetComponent<GhostController>().SetToScared();
        });

        AudioManager.currentBGMState = BGMState.PoweredUp;
    }

    public void ReduceLife()
    {
        lifeCount--;
        hudManager.SetLife(lifeCount);
    }
}
