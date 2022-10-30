using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI Score;
    [SerializeField] private TextMeshProUGUI ScareTimer;
    [SerializeField] private TextMeshProUGUI GameTime;
    [SerializeField] private Image Life1;
    [SerializeField] private Image Life2;
    [SerializeField] private Image Life3;
    [SerializeField] private TextMeshProUGUI Intro;

    public void SetScore(int score)
    {
        Score.text = score.ToString();
    }

    public void SetScareTime(float time)
    {
        if (time <= 0f)
        {
            ScareTimer.gameObject.SetActive(false);
        }
        else
        {
            ScareTimer.gameObject.SetActive(true);
        }

        ScareTimer.text = time.ToString();
    }

    public void SetLife(int life)
    {
        life = Mathf.Clamp(life, 0, 3);

        if(life == 3)
        {
            Life1.gameObject.SetActive(true);
            Life2.gameObject.SetActive(true);
            Life3.gameObject.SetActive(true);
        }


        if (life == 2)
        {
            Life1.gameObject.SetActive(true);
            Life2.gameObject.SetActive(true);
            Life3.gameObject.SetActive(false);
        }

        if (life == 1)
        {
            Life1.gameObject.SetActive(true);
            Life2.gameObject.SetActive(false);
            Life3.gameObject.SetActive(false);
        }

        if (life == 0)
        {
            Life1.gameObject.SetActive(false);
            Life2.gameObject.SetActive(false);
            Life3.gameObject.SetActive(false);
        }
    }

    public void SetIntro(float time)
    {
        if (time >= 0f)
        {
            Intro.text = "GO";
        }

        if (time >= 1f)
        {
            Intro.text = "1";
        }

        if (time >= 2f)
        {
            Intro.text = "2";
        }

        if (time >= 3f)
        {
            Intro.text = "3";
        }
    }

    public void HideIntro()
    {
        Intro.gameObject.SetActive(false);
    }

    public void DisplayTime(float timeToDisplay)
    {
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        float milliSeconds = (timeToDisplay % 1) * 1000;
        GameTime.text = string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliSeconds);
    }
}
