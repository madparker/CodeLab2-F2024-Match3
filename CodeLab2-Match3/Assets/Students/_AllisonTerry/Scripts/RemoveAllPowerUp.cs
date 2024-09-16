using System.Collections;
using System.Collections.Generic;
using AllisonTerry;
using UnityEngine;

public class RemoveAllPowerUp : MonoBehaviour
{
    protected FixedMatchManagerScript myMatchManagerScript;
    protected GameManagerScript gameManager;
    protected TokenObjectPool tokenObjectPool;
    public GameObject removeAllPowerUpButton;
    GameObject[] tokenTypes;
    Sprite[] spriteTypes;
    public int powerUpThreshold = 10;
    
    
    // Start is called before the first frame update
    //initialize the match manager script to be used in this one
    void Start()
    {
        myMatchManagerScript = GetComponent<FixedMatchManagerScript>();
        gameManager = GetComponent<GameManagerScript>();
        tokenTypes = Resources.LoadAll<GameObject>("_Core/Tokens"); //load all the token prefabs
        spriteTypes = Resources.LoadAll<Sprite>("_Core/Images");
    }

    // Update is called once per frame
    void Update()
    {
        if (myMatchManagerScript.matchNumber >= powerUpThreshold)
        {
            //remove all button active
            removeAllPowerUpButton.gameObject.SetActive(true);
        }
    }


    public void RemoveAllTokensOfType()
    {
        
        //have two game objects one from the list of tokens
        GameObject tokenToBeChecked;
        //chooses a random token from the list
        tokenToBeChecked = tokenTypes[Random.Range(0, spriteTypes.Length)];
        //the other to be initalized later in the loop to then be destroyed
        GameObject tokenToBeRemoved;
        print(tokenToBeChecked);
        
        //take the sprite render of the sprite chosen
        // go through the entire array
        for (int x = 0; x < gameManager.gridWidth; x++)
        {
            for (int y = 0; y < gameManager.gridHeight; y++)
            {
                //set a place holder sprite equal to the current sprite the loop is at
                Sprite currentSprite = gameManager.gridArray[x, y].GetComponent<SpriteRenderer>().sprite;
                //if the current sprite is equal to the sprite from the list of tokens
                if (currentSprite == tokenToBeChecked.GetComponent<SpriteRenderer>().sprite)
                {
                    //set the token to be removed to the current token in the array
                    tokenToBeRemoved = gameManager.gridArray[x, y];
                    //tokenObjectPool.RemoveToken(tokenToBeRemoved); didn't work :(
                    //remove that token
                    Destroy(tokenToBeRemoved);
                }
            }
        }
        myMatchManagerScript.matchNumber = 0;
        removeAllPowerUpButton.SetActive(false);
        powerUpThreshold *= 2;
    }
}
