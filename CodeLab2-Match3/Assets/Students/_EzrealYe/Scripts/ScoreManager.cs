using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public int score = 0;  // initialize the score
    public Text scoreText; // show the score on the UI Canvas

    // Add Scores
    public void AddScore(int points)
    {
        score += points;  // add scores
        UpdateScoreText(); // update UI
    }

    // update the score text on UI
    private void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score.ToString(); // convert int to string
        }
    }
}
