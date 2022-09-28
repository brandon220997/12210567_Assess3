using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BGMState
{
    None,
    Intro,
    Normal,
    PoweredUp,
    Eaten
}

public class AudioManager : MonoBehaviour
{
    public AudioSource intro;
    public AudioSource normal;
    public AudioSource poweredUp;
    public AudioSource enemyEaten;
    public AudioSource sfx;

    private bool introStarted = false;

    public static BGMState currentBGMState = BGMState.None;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool IntroPlayed()
    {
        return (GetIntroRemainingTime() == 0) && introStarted;
    }

    public float GetIntroRemainingTime()
    {
        float pitch = (intro.pitch == 0) ? 0.00001f : intro.pitch;
        float remainingTime = (intro.clip.length - intro.time) / pitch;
        return pitch < 0 ?
            (intro.clip.length + remainingTime) :
            remainingTime;
    }

    public void PlayIntro()
    {
        intro.Play();
        normal.Stop();
        poweredUp.Stop();
        enemyEaten.Stop();

        introStarted = true;
    }

    public void ChangeBGM()
    {
        if (currentBGMState == BGMState.Normal)
        {
            currentBGMState = BGMState.PoweredUp;
        }
        else if (currentBGMState == BGMState.PoweredUp)
        {
            currentBGMState = BGMState.Eaten;
        }
        else if (currentBGMState == BGMState.Eaten)
        {
            currentBGMState = BGMState.Normal;
        }
    }

    public void PlayBGM()
    {
        if (currentBGMState == BGMState.Normal)
        {
            PlayNormalBGM();
        }

        if (currentBGMState == BGMState.PoweredUp)
        {
            PlayPoweredUpBGM();
        }

        if (currentBGMState == BGMState.Eaten)
        {
            PlayEatenBGM();
        }
    }

    private void PlayNormalBGM()
    {
        intro.Stop();

        if(!normal.isPlaying) normal.Play();
        normal.volume = 1;

        if (!poweredUp.isPlaying) poweredUp.Play();
        poweredUp.volume = 0;

        if (!enemyEaten.isPlaying) enemyEaten.Play();
        enemyEaten.volume = 0;
    }

    private void PlayPoweredUpBGM()
    {
        intro.Stop();

        if (!normal.isPlaying) normal.Play();
        normal.volume = 0;

        if (!poweredUp.isPlaying) poweredUp.Play();
        poweredUp.volume = 1;

        if (!enemyEaten.isPlaying) enemyEaten.Play();
        enemyEaten.volume = 0;
    }

    private void PlayEatenBGM()
    {
        if (!normal.isPlaying) normal.Play();
        normal.volume = 0;

        if (!poweredUp.isPlaying) poweredUp.Play();
        poweredUp.volume = 0;

        if (!enemyEaten.isPlaying) enemyEaten.Play();
        enemyEaten.volume = 1;
    }
}
