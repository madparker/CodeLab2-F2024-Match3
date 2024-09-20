using System.Collections;
using System.Collections.Generic;
using AlexandraAnderson;
using TMPro;
using UnityEngine;

//this sealed class prevents other classes from inheriting from it, but specifically ScoreCounter is visible outside this environment
public sealed class ScoreCounter : MonoBehaviour
{
    
    //using the singleton pattern for score counter
    public static ScoreCounter Instance { get; private set; }

    private int _score;

    public int Score //score as a property, can be 
    {
        get => _score;

        set
        {
            if (_score == value) return;
            _score = value;
            
            scoreText.SetText($"Score = {_score}");
        }
    }

    [SerializeField] private TextMeshProUGUI scoreText;
    private void Awake() => Instance = this; //using expression body
}
