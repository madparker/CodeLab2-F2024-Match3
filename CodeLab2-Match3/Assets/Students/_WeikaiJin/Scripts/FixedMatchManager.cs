using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeikaiJin
{
    public class FixedMatchManagerScript : MatchManagerScript
    {
        public Material bombMaterial;
        
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

    public bool Bomb(GameObject obj)
    {
        return obj.tag == "bomb";
    }

    public bool InGrid(int x, int y)
    {
        return x>=0 && x<gameManager.gridWidth && y>=0 && y<gameManager.gridHeight;
    }

    public List<GameObject> GetAllNeighbors(int x, int y)
    {
        List<GameObject> neighbors = new List<GameObject>();
        for (int i = x - 1; i <= x + 1; i++)
        {
            for (int j = y - 1; j <= y + 1; j++)
            {
                if(!InGrid(i,j))
                    continue;
                GameObject neighbor = gameManager.gridArray[i, j];
                neighbors.Add(neighbor);
            }
        }

        return neighbors;
    }
    
    List<GameObject> tokensToRemove = new List<GameObject>();

    // Remove Tokens, and blow up neighbors if the removed one is a bomb.
    public void RemoveToken(int x, int y)
    {
        GameObject token = gameManager.gridArray[x, y];
        if(tokensToRemove.Contains(token)) return;
        tokensToRemove.Add(token);
        Debug.Log("Remove Token!");
        if (Bomb(token))
        {
            for (int i = x - 1; i <= x + 1; i++)
            {
                for (int j = y - 1; j <= y + 1; j++)
                {
                    if(!InGrid(i,j))
                        continue;
                    if(i == x && j == y)
                        continue;
                    RemoveToken(i,j);
                }
            }
        }
        
    }
    public override List<GameObject> GetAllMatchTokens()
    {
        tokensToRemove.Clear();
        //using for loop to check each component
        for (int x = 0; x < gameManager.gridWidth - 2; x++)
        {
            for (int y = 0; y < gameManager.gridHeight; y++)
            {
                int horizontalMatchLength = GetHorizontalMatchLength(x, y);

                if (horizontalMatchLength > 2)
                {
                    // loop through all matched tokens and add them to the tokensToRemove list
                    for (int i = x; i < x + horizontalMatchLength; i++)
                    {
                        RemoveToken(i, y);        
                    }
                }
            }
        }
        
        for (int x = 0; x < gameManager.gridWidth; x++)
        {
            for (int y = 0; y < gameManager.gridHeight - 2; y++)
            {
                int verticalMatchLength = GetVerticalMatchLength(x, y);

                if (verticalMatchLength > 2)
                {
                    // loop through all matched tokens and add them to the tokensToRemove list
                    for (int i = y; i < y + verticalMatchLength; i++)
                    {
                        RemoveToken(x, i);             
                    }
                }
            }
        }
        //
        // // Process all the bombs
        // List<GameObject> curBombs = new List<GameObject>();
        // List<GameObject> processedBombs = new List<GameObject>();
        // List<GameObject> blownTokens = new List<GameObject>();
        //
        //
        // do
        // {
        //     curBombs.Clear();
        //     blownTokens.Clear();
        //     foreach (var i in tokensToRemove)
        //     {
        //         if (TokenIsBomb(i) && !processedBombs.Contains(i))
        //         {
        //             curBombs.Add(i);
        //         }
        //     }
        //
        //     foreach (var i in curBombs)
        //     {
        //         var l = GetAllNeighbors(i);
        //         foreach (var j in l)
        //         {
        //             if(!blownTokens.Contains(j) && !tokensToRemove.Contains(j))
        //                 blownTokens.Add(j);
        //         }
        //         
        //     }
        //     
        //     
        // } while (curBombs.Count > 0);
        //
        //return tokensToRemove list
        return tokensToRemove;
    }
    }

}
