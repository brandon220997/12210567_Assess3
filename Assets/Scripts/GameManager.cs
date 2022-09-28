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
        if (inputManager.SpacePressed())
        {
            audioManager.ChangeBGM();
        }

        audioManager.PlayBGM();
    }
}
