using System.Collections;
using System.Collections.Generic;
using AlexandraAnderson;
using TMPro;
using UnityEngine;

public class ScoreCounter : MonoBehaviour
{
    //using the singleton pattern for score counter
    public static ScoreCounter Instance { get; private set; }

    private int _score;

    public int Score //score as a property
    {
        get => _score;

        set
        {
            if (_score == value) return;
            _score = value;
            
            scoreText.SetText("Score = {_score}");
        }
    }

    [SerializeField] private TextMeshProUGUI scoreText;
    private void Awake() => Instance = this; //using expression body
}
