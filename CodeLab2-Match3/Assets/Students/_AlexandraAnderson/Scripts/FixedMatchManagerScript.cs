using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

namespace AlexandraAnderson 
{

    public class FixedMatchManagerScript : MatchManagerScript
    {

        public override bool GridHasMatch(){ //when a function is virtual, any subclass can override and change it.
            /*
             GridHasMatch loops through the grid with all of the potential matches, on the x and the y and checks to see if there are matches.
            Since the x has already been checked with the function GridHasHorizontalMatch in the original MatchManager Script, the same must be done with y using GridHasVerticalMatch.
            This function will check for vertical matches the same way the previous function checks for horizontal matches.
            */
            //AudioSource aud; //initializing audio source for match sound
            bool match = base.GridHasMatch();
            
            for (int x = 0; x < gameManager.gridWidth; x++) //iterates through x-axis
            {
                for (int y = 0; y < gameManager.gridHeight; y++) //iterates through y-axis
                {
                    //same for the x, if the y is less than 2 spaces away it's in the correct range for a match, if it's closer you can't have a match of 3 anymore
                    if (y < gameManager.gridHeight - 2)
                    {
                        match = match || GridHasVerticalMatch(x, y); 
                    }
                }
            }
		      //this is the match bool variable which is returned true if there is a match and false if there isn't
            return match;
            /*  
            //if ( match == true) { //If a match is true, then play nice audio sound
                 // aud = GetComponent<AudioSource>();
                //  aud.Play(); 
             // }
             */
        }
        
        
        public override List<GameObject> GetAllMatchTokens(){ //needs to be virtual so you can override it and change it
            List<GameObject> tokensToRemove = base.GetAllMatchTokens(); //getting all of the tokens in the base fucniton, getting all of the horizontal matches

            
		
            return tokensToRemove;
        }

        public bool GridHasVerticalMatch(int x, int y)
        {
            //creating the function that is for checking the y-axis for vertical matches
            //first the tokens in a column vertically must be given their own variable so you can check if they empty or if there's a match.
            GameObject Token1 = gameManager.gridArray[x, y + 0];
            GameObject Token2 = gameManager.gridArray[x, y + 1];
            GameObject Token3 = gameManager.gridArray[x, y + 2];
            //Tokens as Game Object from the grid array
            
            //then need to ensure the tokens are not empty
            if (Token1 != null && Token2 != null && Token3 != null)
            {
                //then assign their sprites to variables
                SpriteRenderer Sprite1 = Token1.GetComponent<SpriteRenderer>();
                SpriteRenderer Sprite2 = Token2.GetComponent<SpriteRenderer>();
                SpriteRenderer Sprite3 = Token3.GetComponent<SpriteRenderer>();
                
                //return true if there is a match and all the sprites are the same
                //print("checking for vertical match...YES you got one.");
                return (Sprite1.sprite == Sprite2.sprite && Sprite2.sprite == Sprite3.sprite);
            }
            //if they are not a match return false
            else
            {
                return false;
            }
        }

        public bool NewGridCheck()
        {
            bool match = false; //setting match equal to false
            
            //loop through the grid
            for (int x = 0; x < gameManager.gridWidth; x++) //iterates through x-axis
            {
                for (int y = 0; y < gameManager.gridHeight; y++) //iterates through y-axis
                {
                    //same for the x, if the y is less than 2 spaces away it's in the correct range for a match
                    if (y < gameManager.gridHeight - 2)
                    {
                        //Check for a vertical match!
                        match = match || GridHasVerticalMatch(x, y) || GridHasVerticalMatch(x, y);
                    }
                }
            }
            //returns match variable which will be false if there isn't a match and true if there is
            return match;
        }
        
        //need to get the length of the match in order to get all of the vertical matches and loop through them then remove them
        public int VerticalMatchLength(int x, int y)
        {
            int matchLength = 1; //initializing the match length variable

            GameObject firstPos = gameManager.gridArray[x, y];

            //if the first position is not empty
            if (firstPos != null)
            {
                //then set the first sprite to the sprite renderer of first position
                SpriteRenderer Sprite1 = firstPos.GetComponent<SpriteRenderer>();

                //then loop through the height of the grid
                for (int t = y; t < gameManager.gridHeight; t++)
                {
                    //assign the game objects (gobjs) to t's current position as it goes through the length of the column
                    GameObject gobjs = gameManager.gridArray[x, t];

                    //the game object is not empty, then set the second sprite to the sprite renderer of the gobjs
                    if (gobjs != null)
                    {
                        SpriteRenderer Sprite2 = gobjs.GetComponent<SpriteRenderer>();

                        //if the sprites are the same
                        if (Sprite1 == Sprite2)
                        {
                            matchLength++; //continue
                        }
                        else
                        {
                            break; //stop, if they are not the same
                        }
                    }
                    //if the game objects (gobjs) empty then stop because there isn't anything next to it so it cant be a match
                    else
                    {
                        break;
                    }
                }
            }

            return matchLength; //this returns the match length as an integer, the length of how many of the same sprite are next to eachother vertically to then be removed
        }

        public List<GameObject> AllVerticalMatchTokens() //making a list of all the vertical match tokens yay
            {
                //empty list
                List<GameObject> tokensRemoved = new List<GameObject>();
                
                //loop through the grid!
                for (int x = 0; x < gameManager.gridWidth; x++) //iterates through x-axis
                {
                    for (int y = 0; y < gameManager.gridHeight; y++) //iterates through y-axis
                    {
                        //if the y is less than 2 spaces away it's in the correct range for a match
                        if (y < gameManager.gridHeight - 2)
                        {
                            //time to use VerticalMatchLength
                            int VML = VerticalMatchLength(x, y);
                            
                            //if the vertical match length is greater than 2
                            if (VML >2){
                                //then loop through the matching tokens
                                for (int t = y; t < y + VML; t++)
                                {
                                    //set the token that is currently being checked
                                    GameObject token = gameManager.gridArray[x, y];
                                    
                                    //add the token to the list of tokens to be removed
                                    tokensRemoved.Add(token);
                                }
                            }
                        }
                    }
                }
                // return the list of tokens to be removed
                return tokensRemoved;
            }
    }
}