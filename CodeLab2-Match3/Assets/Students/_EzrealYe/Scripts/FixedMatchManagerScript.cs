using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EzrealYe // add my namespace to clarify it's my script
{
    public class FixedMatchManagerScript : MatchManagerScript
    {
        public override bool GridHasMatch()
        {
            //inherit the function from original script 
            bool match = base.GridHasMatch();
            
            //using the same method to loop through the entire array
            for (int x = 0; x < gameManager.gridWidth; x++)
            {
                for (int y = 0; y < gameManager.gridHeight - 2; y++)  
                {
                    //prevent ineffective check from vertical gird, if the gridheight is 8, it's impossible to check for
                    //3 consecutive matches near the bottom of the grid
                    if(y < gameManager.gridHeight - 2){
                    
                    //if grid has vertical match, return true
                    //if match or the method return true
					match = match || GridHasVerticalMatch(x, y);
				    }
                }
            }
            return match;
            
        }
        
    private bool GridHasVerticalMatch(int x, int y)
        {
            //using the same structure as horizontal match, but this time increment vertically
            GameObject token1 = gameManager.gridArray[x, y + 0];
            GameObject token2 = gameManager.gridArray[x, y + 1];
            GameObject token3 = gameManager.gridArray[x, y + 2];
        
            //make sure all three tokens are not null,
            if(token1 != null && token2 != null && token3 != null)
            {
                //assign sprite renderers to them
                SpriteRenderer sr1 = token1.GetComponent<SpriteRenderer>();
                SpriteRenderer sr2 = token2.GetComponent<SpriteRenderer>();
                SpriteRenderer sr3 = token3.GetComponent<SpriteRenderer>();
                
                //if all the sprites are the same, return ture, there is a match
                return (sr1.sprite == sr2.sprite && sr2.sprite == sr3.sprite);
            } 
            else 
            {
                // if null, return false
                return false;
            }
    }

    private int GetVerticalMatchLength(int x, int y)
    {
        // initialize match Length
        int matchLength = 1;

        // get the current location
        GameObject first = gameManager.gridArray[x, y];

        // if the token is not null
        if (first != null)
        {
            // get the renderer component
            SpriteRenderer sr1 = first.GetComponent<SpriteRenderer>();

            // for loop to check 
            for (int i = y + 1; i < gameManager.gridHeight; i++)
            {
                // get the GameObject below the current position in the grid
                GameObject other = gameManager.gridArray[x, i];

                // the other GameObject exists and is not null
                if (other != null)
                {
                    SpriteRenderer sr2 = other.GetComponent<SpriteRenderer>();

                    // if the sprite of the other GameObject matches the first one, increase the match length
                    if (sr1.sprite == sr2.sprite)
                    {
                        matchLength++;
                    }
                    else
                    {
                        // break the loop if the sprites do not match
                        break;
                    }
                }
                else
                {
                    // break the loop if the GameObject is Null
                    break;
                }
            }
        }

    // return the vertical matchLength
    return matchLength;
}

    public override List<GameObject> GetAllMatchTokens()
    {
        //inherit the function from original script 
        List<GameObject> tokensToRemove = base.GetAllMatchTokens();

        //using for loop to check each component
        for (int x = 0; x < gameManager.gridWidth; x++)
        {
            for (int y = 0; y < gameManager.gridHeight; y++)
            {
                // check if there's more than 2 units from the bottom to ensure a valid vertical match can be detected
                if (y < gameManager.gridHeight - 2)
                {
                    // run the GetVerticalMatchLength method to get result
                    int vertMatchLength = GetVerticalMatchLength(x, y);

                    // if the vertical match length is greater than 2 && 
                    // y + vertMatchLength < gridHeight will prevent index out of range error : ) 
                    if (vertMatchLength > 2 && y + vertMatchLength <= gameManager.gridHeight)
                    {
                        // loop through all matched tokens and add them to the tokensToRemove list
                        for (int i = y; i < y + vertMatchLength; i++)
                        {
                            GameObject token = gameManager.gridArray[x, i];
                            tokensToRemove.Add(token);                   
                        }
                    }
                }
            }
        }
    
        //return tokensToRemove list
        return tokensToRemove;
    }
}
}
