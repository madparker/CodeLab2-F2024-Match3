using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CunxiGao
{
    public class ScoreAndTimerManager : MonoBehaviour
    {
        public TextMeshProUGUI scoreText;
        public TextMeshProUGUI timerText;
        
        private int score;
        private float timer;
        private bool isTimerEnded;
        
        // Start is called before the first frame update
        void Start()
        {
            score = 0;
            timer = 15;
            isTimerEnded = false;
        }

        // Update is called once per frame
        void Update()
        {
            // Decrease the timer over time
            if (timer > 0)
            {
                timer -= Time.deltaTime;
            }
            else
            {
                timer = 0;
                OnTimerEnd();
            }
            timerText.text = "Timer: " + timer.ToString("F1");

            if (!isTimerEnded)
            {
                scoreText.text = "Score: " + score;
            }
            else
            {
                scoreText.text = "Your final score is: " + score;
            }
        }
        
        public void AddScore(int amount)
        {
            if (!isTimerEnded)
            {
                score += amount;
            }
        }
        
        public void AddTimer(float amount)
        {
            if (!isTimerEnded)
            {
                timer += amount;
            }
        }
        
        private void OnTimerEnd()
        {
            isTimerEnded = true;
        }
    }
}
