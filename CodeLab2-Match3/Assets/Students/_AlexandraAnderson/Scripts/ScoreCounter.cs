using System.Collections;
using System.Collections.Generic;
using AlexandraAnderson;
using TMPro;
using UnityEngine;

/*
 This sealed class prevents other classes from inheriting from it, but specifically ScoreCounter is visible outside this environment
 due to how is is an exposed API(Application Programming Interface). Thus, ScoreCounter cannot be inherited or extended since 
 this class has fixed behavior that should not change. I declared this class sealed as it is defined in a library and didn't 
 want other users of the library (library is a collection of classes and namespaces in C# without any entry point method) 
 to inherit this class.
*/
 
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
