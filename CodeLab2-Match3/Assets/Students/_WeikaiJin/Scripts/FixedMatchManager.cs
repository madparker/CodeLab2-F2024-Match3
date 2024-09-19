using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeikaiJin
{
    public class FixedMatchManagerScript : MatchManagerScript
    {
        public override bool GridHasMatch()
        {
            
            //Initialize the match bool to false
            bool match = false;
            
            //loop through the entire array EXCEPT the last 2 colomns
            for(int x = 0; x < gameManager.gridWidth - 2; x++){
                for(int y = 0; y < gameManager.gridHeight ; y++)
                {
                    match = match || GridHasHorizontalMatch(x, y);
                }
            }

            //loop through the entire array EXCEPT the last 2 rows
            for(int x = 0; x < gameManager.gridWidth; x++){
                for(int y = 0; y < gameManager.gridHeight - 2 ; y++)
                { 
                    match = match || GridHasVerticalMatch(x, y);
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
        List<GameObject> tokensToRemove = base.GetAllMatchTokens();

        //using for loop to check each component
        for (int x = 0; x < gameManager.gridWidth; x++)
        {
            for (int y = 0; y < gameManager.gridHeight; y++)
            {
                //as long as x is more than 2 spaces away from the right edge of the grid,
                if (y < gameManager.gridHeight - 2)
                {
                    // run the GetVerticalMatchLength method to get result
                    int verticalMatchLength = GetVerticalMatchLength(x, y);

                    //if the verticalMatchLength variable is larger than 2
                    //(meaning there are 3 of the same token in a column vertically) 
                    if (verticalMatchLength > 2)
                    {
                        
                        // loop through all matched tokens and add them to the tokensToRemove list
                        for (int i = y; i < y + verticalMatchLength; i++)
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
