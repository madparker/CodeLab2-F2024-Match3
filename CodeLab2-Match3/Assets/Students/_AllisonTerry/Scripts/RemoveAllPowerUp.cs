using System.Collections;
using System.Collections.Generic;
using AllisonTerry;
using UnityEngine;

public class RemoveAllPowerUp : InputManagerScript
{
    protected FixedMatchManagerScript myMatchManagerScript;
    protected GameManagerScript gameManager;
    protected InputManagerScript inputManager;
    public GameObject removeAllPowerUpButton;
    public GameObject currentToken;
    
    
    // Start is called before the first frame update
    //initialize the match manager script to be used in this one
    void Start()
    {
        myMatchManagerScript = GetComponent<FixedMatchManagerScript>();
        gameManager = GetComponent<GameManagerScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (myMatchManagerScript.matchNumber >= 5)
        {
            //remove all button active
            removeAllPowerUpButton.gameObject.SetActive(true);
        }
    }


    public void RemoveAllTokensOfType()
    {
        
        //take the sprite render of the sprite chosen
        // go through the entire array
        for (int x = 0; x < gameManager.gridWidth; x++)
        {
            for (int y = 0; y < gameManager.gridHeight; y++)
            {
                // add all the tokens of that type to the to be removed list
                
            }
        }
        
        //remove them
    }

    public void getCurrentToken()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //sets mousePos variable to the current location of the mouse on screen
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            //assigns the tokenCollider variable to the specific token that you are mousing over
            Collider2D tokenCollider = Physics2D.OverlapPoint(mousePos);

            currentToken = tokenCollider.gameObject;
            print(currentToken.name);
        }

    }
}
