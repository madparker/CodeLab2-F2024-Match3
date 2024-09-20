using System.Collections;
using System.Collections.Generic;
using HermanF;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public float timeRemaining = 10f;
    private bool timerIsRunning = false;
    public FixedMatchManagerScript matchManagerScript;
    
    // Start is called before the first frame update
    void Start()
    {
        //start the timer
        timerIsRunning = true;
    }

    // Update is called once per frame
    void Update()
    {
        //if the timer is running
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                //subtract the timer counting
                timeRemaining -= Time.deltaTime;
                UpdateTimer(timeRemaining);
            }
            else
            {
                timeRemaining = 0;
                timerIsRunning = false;
                UpdateTimer(timeRemaining);
            }
        }
        else
        {
            SceneManager.LoadScene(0);
        }
    }

    void UpdateTimer(float timeDisplay)
    {
        //timer should not be negative
        timeDisplay = Mathf.Max(timeDisplay, 0);
        
        //calculate seconds and milliseconds
        int second = Mathf.FloorToInt(timeDisplay);
        int milsec = Mathf.FloorToInt((timeDisplay - second) * 100);
        
        //update the timer to SS:MSS format
        timerText.text = string.Format("{0:00}:{1:00}", second, milsec);
    }

    public void AddTime()
    {
        timeRemaining += 1f;
    }
}
