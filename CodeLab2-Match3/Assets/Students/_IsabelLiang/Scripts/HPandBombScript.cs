using System.Collections;
using System.Collections.Generic;
using IsabelLiang;
using UnityEngine;
using UnityEngine.UI;

public class HPandBombScript : MonoBehaviour
{

    public Slider health;
    public Slider bomb;

    public List<GameObject> bomblist;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        health.value = FixedMatchManagerScript.Instance.healthSlider;
        bomb.value = FixedMatchManagerScript.Instance.bombSlider;
        bomblist = FixedMatchManagerScript.Instance.bombTokens;

    }
}
