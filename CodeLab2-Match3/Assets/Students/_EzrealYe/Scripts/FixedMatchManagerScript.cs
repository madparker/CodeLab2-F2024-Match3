using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EzrealYe // add my namespace to clarify it's my script
{
    public class FixedMatchManagerScript : MatchManagerScript
    {
    //get reference to score manager
    public ScoreManager scoreManager;

    public override bool GridHasMatch()
    {
        bool match = base.GridHasMatch();

        // check horizontal match
        for (int x = 0; x < gameManager.gridWidth; x++)
        {
            for (int y = 0; y < gameManager.gridHeight; y++)
            { 
                if (x < gameManager.gridWidth - 2 )
                {
                    if (GridHasHorizontalMatch(x, y))
                    {
                        match = true;
                    }
                }
            }
        }

        // check vertical match, same logic
        for (int x = 0; x < gameManager.gridWidth; x++)
        {
            for (int y = 0; y < gameManager.gridHeight - 2; y++)
            {
                if (GridHasVerticalMatch(x, y))
                {
                    match = true;
                }
            }
        }


        // Check diagonal match, from left to right
        for (int x = 0; x < gameManager.gridWidth - 2; x++)
        {
            for (int y = 0; y < gameManager.gridHeight - 2; y++)
            {
                if (GridHasDiagonalMatchLeftToRight(x, y))
                {
                    match = true;
                }
            }
        }

        // Check diagonal match, from right left 
        for (int x = 2; x < gameManager.gridWidth; x++)
        {
            for (int y = 0; y < gameManager.gridHeight - 2; y++)
            {
                if (GridHasDiagonalMatchRightToLeft(x, y))
                {
                    match = true;
                }
            }
        }

        // if there is a match, add score
        if(match)
        {
            AddScoreForMatch();
        }

        return match;
}


    private void AddScoreForMatch()
    {
        int baseScore = 100;

        // Initialize multiplier
        float multiplier = 1.0f;  // Default multiplier is 1

        // Adjust multiplier based on the current score
        if (scoreManager.score > 20000)
        {
            multiplier = 16f;  // Multiplier is 16x if score is greater than 20,000
        }
        else if (scoreManager.score > 10000)
        {
            multiplier = 8f;  // Multiplier is 8x if score is greater than 10,000
        }
        else if (scoreManager.score > 3000)
        {
            multiplier = 4f;  // Multiplier is 4x if score is greater than 3,000
        }
        else if (scoreManager.score > 1000)
        {
            multiplier = 2f;  // Multiplier is 2x if score is greater than 1,000
        }

        // Calculate final score based on the multiplier
        int finalScore = Mathf.RoundToInt(baseScore * multiplier);
        
        // Add the final score to the total score
        scoreManager.AddScore(finalScore);
    }


    private new bool GridHasHorizontalMatch(int x, int y)
        {
            // call the original function
            return base.GridHasHorizontalMatch(x, y);
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

        // check diagonal match from top left to bottom right
        for (int x = 0; x < gameManager.gridWidth - 2; x++)
        {
            for (int y = 0; y < gameManager.gridHeight - 2; y++)
            {
                int matchLength = GetDiagonalMatchLengthLeftToRight(x, y);

                if (matchLength >= 3)
                {
                    for (int i = 0; i < matchLength; i++)
                    {
                        GameObject token = gameManager.gridArray[x + i, y + i];
                        tokensToRemove.Add(token);
                    }
                }
            }
        }

        // check diagonal match from top right to bottom left
        for (int x = 2; x < gameManager.gridWidth; x++)
        {
            for (int y = 0; y < gameManager.gridHeight - 2; y++)
            {
                int matchLength = GetDiagonalMatchLengthRightToLeft(x, y);

                if (matchLength >= 3)
                {
                    for (int i = 0; i < matchLength; i++)
                    {
                        GameObject token = gameManager.gridArray[x - i, y + i];
                        tokensToRemove.Add(token);
                    }
                }
            }
        }        
            //return tokensToRemove list
            return tokensToRemove;
        }

    private bool GridHasDiagonalMatchLeftToRight(int x, int y)
    {
        // check diagonal match from bottom left to top right
        GameObject token1 = gameManager.gridArray[x, y];
        GameObject token2 = gameManager.gridArray[x + 1, y + 1];
        GameObject token3 = gameManager.gridArray[x + 2, y + 2];

        if (token1 != null && token2 != null && token3 != null)
        {
            SpriteRenderer sr1 = token1.GetComponent<SpriteRenderer>();
            SpriteRenderer sr2 = token2.GetComponent<SpriteRenderer>();
            SpriteRenderer sr3 = token3.GetComponent<SpriteRenderer>();

            return sr1.sprite == sr2.sprite && sr2.sprite == sr3.sprite;
        }

        return false;
    }

    private bool GridHasDiagonalMatchRightToLeft(int x, int y)
    {
        // check diagonal match from bottom right to top left
        GameObject token1 = gameManager.gridArray[x, y];
        GameObject token2 = gameManager.gridArray[x - 1, y + 1];
        GameObject token3 = gameManager.gridArray[x - 2, y + 2];

        if (token1 != null && token2 != null && token3 != null)
        {
            SpriteRenderer sr1 = token1.GetComponent<SpriteRenderer>();
            SpriteRenderer sr2 = token2.GetComponent<SpriteRenderer>();
            SpriteRenderer sr3 = token3.GetComponent<SpriteRenderer>();

            return sr1.sprite == sr2.sprite && sr2.sprite == sr3.sprite;
        }

        return false;
    }

    private int GetDiagonalMatchLengthLeftToRight(int x, int y)
    {
        //same logic as before, check the length from bottom left to top right
        int matchLength = 1;
        GameObject first = gameManager.gridArray[x, y];

        if (first != null)
        {
            SpriteRenderer sr1 = first.GetComponent<SpriteRenderer>();

            for (int i = 1; i < gameManager.gridWidth - x && i < gameManager.gridHeight - y; i++)
            {
                GameObject other = gameManager.gridArray[x + i, y + i];
                if (other != null)
                {
                    SpriteRenderer sr2 = other.GetComponent<SpriteRenderer>();

                    if (sr1.sprite == sr2.sprite)
                    {
                        matchLength++;
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    break;
                }
            }
        }

        return matchLength;
    }

    private int GetDiagonalMatchLengthRightToLeft(int x, int y)
    {
        // same logic as before, check the length from bottom right to top left
        int matchLength = 1;
        GameObject first = gameManager.gridArray[x, y];

        if (first != null)
        {
            SpriteRenderer sr1 = first.GetComponent<SpriteRenderer>();

            for (int i = 1; i <= x && i < gameManager.gridHeight - y; i++)
            {
                GameObject other = gameManager.gridArray[x - i, y + i];
                if (other != null)
                {
                    SpriteRenderer sr2 = other.GetComponent<SpriteRenderer>();

                    if (sr1.sprite == sr2.sprite)
                    {
                        matchLength++;
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    break;
                }
            }
        }

        return matchLength;
    }
}
}
