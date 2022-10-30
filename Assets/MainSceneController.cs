using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MainSceneController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI HighScore;
    [SerializeField] private TextMeshProUGUI HighScoreTime;

    // Start is called before the first frame update
    void Start()
    {
        if(PlayerPrefs.GetInt("HighScore") != 0) HighScore.text = PlayerPrefs.GetInt("HighScore").ToString();
        if (PlayerPrefs.GetFloat("HighScoreTimer") != 0f) DisplayTime(PlayerPrefs.GetFloat("HighScoreTimer"));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DisplayTime(float timeToDisplay)
    {
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        float milliSeconds = (timeToDisplay % 1) * 1000;
        HighScoreTime.text = string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliSeconds);
    }
}
