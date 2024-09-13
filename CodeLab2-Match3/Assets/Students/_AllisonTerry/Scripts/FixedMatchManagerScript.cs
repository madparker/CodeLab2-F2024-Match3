using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

namespace AllisonTerry
{
    //sub class /inherited etc of match manager script
    public class FixedMatchManagerScript : MatchManagerScript
    {
        //base class needs to be virtual
        public override bool GridHasMatch()
        {
            //runs the original base function
            bool match = base.GridHasMatch();
            
            
            //GridHasMatch loops through the entire array of sprites
            //and runs GridHasHorizontalMatch
            //to see if there are any matches
            //so in theory
            //we would need to do the same thing here with a new function
            //GridHasVerticalMatch
            //this new check function checks for vertical as well
            //is there a way to just call this function instead of having the whole thing written here?
            //return NewGridCheck();
            //loop through the entire array again
            for (int x = 0; x < gameManager.gridWidth; x++)
            {
                for (int y = 0; y < gameManager.gridHeight; y++)
                {
                    //if the y position currently being checked is less than 2 spaces away
                    //it is in the right range for a match
                    if (y < gameManager.gridHeight - 2)
                    {
                        //call GridHasVerticalMatxh to check for a vertical match
                        match = match || GridHasVerticalMatch(x, y);
                    }
                }
            }
            //return match variable 
            //true if there is a match
            //false if there isnt
            return match;
        }

        public override List<GameObject> GetAllMatchTokens()
        {
            List<GameObject> tokensToRemove = base.GetAllMatchTokens();
            
            //again is there a way to put all of this in its own function
            //and just call it
            //return GetAllVerticalMatchTokens();

            for (int x = 0; x < gameManager.gridWidth; x++)
            {
                for (int y = 0; y < gameManager.gridHeight; y++)
                {
                    //as long as y is in the appropriate range of 2 space for a match
                    if (y < gameManager.gridHeight - 2)
                    {
                        //run GetVerticalMatchLength 
                        //ser the VeritcalMatchLength int to the returned number
                        int verticalMatchLength = GetVerticalMatchLength(x, y);
                        
                        //if the verticalMatchLength int is larger than 2
                        if (verticalMatchLength > 2)
                        {
                            // loop through the matching tokens
                            for (int i = y; i < y + verticalMatchLength; i++)
                            {
                                //set the token in the space currently being checked
                                GameObject token = gameManager.gridArray[x, y];
                                
                                //add the token to the remove list
                                tokensToRemove.Add(token);
                            }
                        }
                    }
                }
            }
            //return the list of tokens to be removed
            return tokensToRemove;
        }

        public bool GridHasVerticalMatch(int x, int y)
        {
            //takes the 3 tokens that are next to each other in a vertical line
            //and adds them to a place holder 3 game objects
            GameObject token1 = gameManager.gridArray[x, y + 0];
            GameObject token2 = gameManager.gridArray[x, y + 1];
            GameObject token3 = gameManager.gridArray[x, y + 2];
            
            //if all tokens are not empty
            if (token1 != null && token2 != null && token3 != null)
            {
                // assign all of their sprites to these variables to be checked
                SpriteRenderer sr1 = token1.GetComponent<SpriteRenderer>();
                SpriteRenderer sr2 = token2.GetComponent<SpriteRenderer>();
                SpriteRenderer sr3 = token3.GetComponent<SpriteRenderer>();
                
                //return true if all sprites are the same
                //print("there's a vertical match! this is checking the sprites!");
                return (sr1.sprite == sr2.sprite && sr2.sprite == sr3.sprite);
            }
            else
            {
                //else they are not a match and return false
                return false;
            }
        }

        public bool NewGridCheck()
        {
            bool match = false;
            
            //loop thorugh the entire array again
            for (int x = 0; x < gameManager.gridWidth; x++)
            {
                for (int y = 0; y < gameManager.gridHeight; y++)
                {
                    //if the y position currently being checked is less than 2 spaces away
                    //it is in the right range for a match
                    if (y < gameManager.gridHeight - 2)
                    {
                        //call GridHasVerticalMatxh to check for a vertical match
                        match = match || GridHasVerticalMatch(x, y) || GridHasHorizontalMatch(x, y);
                    }
                }
            }
            //return match variable 
            //true if there is a match
            //false if there isnt
            return match;
        }

        //returns how many of the same token are next to each other
        public int GetVerticalMatchLength(int x, int y)
        {
            int matchLength = 1;

            GameObject first = gameManager.gridArray[x, y];
            
            //if first is not empty
            if (first != null)
            {
                // set the sr1 spriteRenderer to the SpriteRenderer of first
                SpriteRenderer sr1 = first.GetComponent<SpriteRenderer>();
                
                //loop through the arrays height
                for (int i = y;  i< gameManager.gridHeight; i++)
                {
                    // assign the other gameobjects to the i'th gameobject in the same column
                    GameObject other = gameManager.gridArray[x, i];
                    
                    //if the other gameobject is not empty
                    if (other != null)
                    {
                        //set the sr2 SpriteRenderer to the SpriteRenderer of the other
                        SpriteRenderer sr2 = other.GetComponent<SpriteRenderer>();
                        
                        //if sr1 and sr2 are the same sprite
                        if (sr1.sprite == sr2.sprite)
                        {
                            matchLength++;
                        }
                        //else if they are not the same stop
                        else
                        {
                            break;
                        }
                    }
                    //else if the other gameobject is empty stop because nothing is next to it
                    else
                    {
                        break;
                    }
                }
            }
            //returm match length int. thats how many of the same sprite are next to each other to then be removed
            return matchLength;
        }

        public List<GameObject> GetAllVerticalMatchTokens()
        {
            //make a new empty list
            List<GameObject> tokensToRemove = new List<GameObject>();
            
            //loop through the grid again
            for (int x = 0; x < gameManager.gridWidth; x++)
            {
                for (int y = 0; y < gameManager.gridHeight; y++)
                {
                    //as long as y is in the appropriate range of 2 space for a match
                    if (y < gameManager.gridHeight - 2)
                    {
                        //run GetVerticalMatchLength 
                        //ser the VeritcalMatchLength int to the returned number
                        int verticalMatchLength = GetVerticalMatchLength(x, y);
                        
                        //if the verticalMatchLength int is larger than 2
                        if (verticalMatchLength > 2)
                        {
                            // loop through the matching tokens
                            for (int i = y; i < y + verticalMatchLength; i++)
                            {
                                //set the token in the space currently being checked
                                GameObject token = gameManager.gridArray[x, y];
                                
                                //add the token to the remove list
                                tokensToRemove.Add(token);
                            }
                        }
                    }
                }
            }
            //return the list of tokens to be removed
            return tokensToRemove;
        }
        
    }

}
