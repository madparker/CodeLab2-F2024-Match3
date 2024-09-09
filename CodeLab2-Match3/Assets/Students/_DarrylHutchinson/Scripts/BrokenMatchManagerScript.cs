using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DarrylHutchinson
{


    public class FixedMatchManagerScript : MatchManagerScript
    {
        // Start is called before the first frame update
        void Start()
        {
            //Grab the OG GameManager script 
            gameManager = GetComponent<GameManagerScript>();
        }

        public override bool GridHasMatch()
        {
            //variable match is set to false which will be used later as a placeholder to check potential truthiness of match
            bool match = false;
            
            //loop through 2D array, all x's and y's
            for (int x = 0; x < gameManager.gridWidth; x++)
            {
                for (int y = 0; y < gameManager.gridHeight; y++)
                {
                    //grid is offset by 2 to prevent checks near the edge of the grid where sprites should not exist
                    if (x < gameManager.gridWidth - 2)
                    {
                        //calls GridHasHorizontalMatch taking parameters of x and y and sets match equal result of GridHasHorizontal() if true
                        //otherwise match is left false
                        match = match || base.GridHasHorizontalMatch(x, y);
                    }
                }
            }
            //returns true or false indicating if a match has been made
            return match;
        }

        public bool GridHasHorizontalMatch(int x, int y)
        {
            //create three placeholder tokens as GameObjects in a horizontal row, after the first incrementing by 1 in the x-axis and the same y value for position
            GameObject token1 = gameManager.gridArray[x + 0, y];
            GameObject token2 = gameManager.gridArray[x + 1, y];
            GameObject token3 = gameManager.gridArray[x + 2, y];

            //Check if all tokens currently exist, and if so, assign a SpriteRenderer to their unique respective sprite variable
            if (token1 != null && token2 != null && token3 != null)
            {
                SpriteRenderer sr1 = token1.GetComponent<SpriteRenderer>();
                SpriteRenderer sr2 = token1.GetComponent<SpriteRenderer>();
                SpriteRenderer sr3 = token1.GetComponent<SpriteRenderer>();

                
                //if values of sprite1, sprite2 and sprite3 are all the same, then return true;...
                return (sr1.sprite == sr2.sprite && sr2.sprite == sr3.sprite);
            }
            else
            {
                //...otherwise return false;
                return false;
            }
        }

        public int GetHorizontalMatchLength(int x, int y)
        {
            //create variable for checking matchLength and set to 1
            int matchLength = 1;
            
            //create placeholder GameObject called first at [x,y] for counting potential match spaces
            GameObject first = gameManager.gridArray[x, y];
            
            //if first currently exists in the scene...
            if (first != null)
            {
                
                //...assign spriteRenderer sr1 to the gameObject called first
                SpriteRenderer sr1 = first.GetComponent<SpriteRenderer>();
                
                //iterate through the grid across x's, width
                for (int i = x + 1; i < gameManager.gridWidth; i++)
                {
                    
                    //a placeholder GameObject called other is assigned the [i, y] position in the grid
                    //I DONT UNDERSTAND HOW 'other' WORKS HERE! S.O.S.! 
                    //...since the first iteration in the loop is i=x+1, and other is starting at [i,y] does this mean other is next to the first position? 🤔
                    GameObject other = gameManager.gridArray[i, y];
                    
                    //if GameObject other exists....
                    if (other != null)
                    {
                        //...then set a SpriteRenderer for other to variable SpriteRenderer sr2
                        SpriteRenderer sr2 = other.GetComponent<SpriteRenderer>();
                        
                        //if sprite values for sprite placeholder sr1 and placeholder sr2 are the same...
                        if (sr1.sprite == sr2.sprite)
                        {
                            //...increment matchLength variable by 1...
                            matchLength++;
                        }
                        
                        //...else break time
                        else
                        {
                            break;
                        }
                    }
                    
                    //....if GameObject other does not else, then break time
                    else
                    {
                        break;
                    }
                }
            }
            //return matchLength variable which represents how many sprites variables with the same sprite value are next to one another
            //checking x to x+1 (to the right)
            return matchLength;
        }
        
        //I DON'T UNDERSTAND HOW/WHY THIS FUNCTION HAS A LIST<> AS A NAME! S.O.S!
        public override List<GameObject> GetAllMatchTokens()
        {
            //create placeholder as a list of tokensToRemove later 
            List<GameObject> tokensToRemove = new List<GameObject>();
            
            //nested loop to iterate through entire grid
            for (int x = 0; x < gameManager.gridWidth; x++)
            {
                for (int y = 0; y < gameManager.gridHeight; y++)
                {

                    //confirmation check that current x is at least 2 spaces away from the edge of the grid to avoid checking spaces at the edge without any sprite
                    if (x < gameManager.gridWidth - 2)
                    {
                        
                        //call GetHorizontalMatchLength() on current space and assign that result to horizontalMatchLength variable
                        int horizontalMatchLength = GetHorizontalMatchLength(x, y);

                        //if horizontalMatchLength is greater than 2, then...
                        if (horizontalMatchLength > 2)
                        {
                            //...proceed to loop throug...IM NOT SURE WHAT IS BEING LOOPED THROUGH! S.O.S.!
                            for (int i = x; i < x + horizontalMatchLength; i++)
                            {
                                //assign current grid position to GameObject token variable
                                GameObject token = gameManager.gridArray[i, y];
                                
                                //add current token to tokesToRemove List
                                tokensToRemove.Add(token);
                            }
                        }
                    }
                }
            }
            //return tokensToRemove as a List
            return tokensToRemove;
        }
        // Update is called once per frame
        void Update()
        {

        }
    }
}

