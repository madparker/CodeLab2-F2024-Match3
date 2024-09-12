using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpyriPotam
{
    
    public class SpyriFixedMatchManagerScript : MatchManagerScript
    {

        //additional code added on top of the initial GridHasMatch function to check for vertical matches
        public override bool GridHasMatch()
        {
            bool match = base.GridHasMatch();

            //loop through the entire array 
            for(int y = 0; y < gameManager.gridHeight; y++){
                for(int x = 0; x < gameManager.gridWidth ; x++){
				
                    //if the y position currently being checked is more than 2 spaces away from the bottom
                    //of the grid (because you cannot match 3 down otherwise)
                    if(y < gameManager.gridHeight - 2){
					
                        //then, call the GridHasVerticalMatch function to check if there is a match
                        //and set the match variable to false or true depending on the outcome of GridHasVerticalMatch
                        match = match || GridHasVerticalMatch(x, y);
                    }
                }
            }

            return match;
        }
        
        //Checks the sprites of three tokens that are next to each other vertically to see if they are the same
        //if they are, returns true, else returns false
        public bool GridHasVerticalMatch(int x, int y){
            //initialize 3 tokens that are next to each other vertically, top to bottom
            GameObject token1 = gameManager.gridArray[x, y + 0];
            GameObject token2 = gameManager.gridArray[x, y + 1];
            GameObject token3 = gameManager.gridArray[x, y + 2];
		
            //if all three tokens are not null,
            if(token1 != null && token2 != null && token3 != null){
			
                //Assign all of their sprite renderers to variables
                SpriteRenderer sr1 = token1.GetComponent<SpriteRenderer>();
                SpriteRenderer sr2 = token2.GetComponent<SpriteRenderer>();
                SpriteRenderer sr3 = token3.GetComponent<SpriteRenderer>();
			
                //return true (assuming all of their sprites are the same)
                return (sr1.sprite == sr2.sprite && sr2.sprite == sr3.sprite);
            } else {
			
                //else, return false
                return false;
            }
        }
        
        //Returns how many of the same sprite(token) are next to each other
        //in a continuous column top to bottom
        public int GetVerticalMatchLength(int x, int y){
		
            //initialize the matchLength integer to 1
            int matchLength = 1;
		
            //initializes the first GameObject to the GameObject at the position that is fed into this function
            GameObject first = gameManager.gridArray[x, y];

            //if first has been initialized,
            if(first != null){
			
                //set the sr1 SpriteRenderer to the SpriteRenderer of the first GameObject
                SpriteRenderer sr1 = first.GetComponent<SpriteRenderer>();
			
                //loop through the array's height
                for(int i = y + 1; i < gameManager.gridHeight; i++){
				
                    //assign the other GameObject to the i'th GameObject in the current column being checked
                    GameObject other = gameManager.gridArray[x, i];

                    //if the other GameObject is not null/has been initialized
                    if(other != null){
					
                        //set the sr2 SpriteRenderer to the SpriteRenderer of the other GameObject
                        SpriteRenderer sr2 = other.GetComponent<SpriteRenderer>();

                        //if sr1 and sr2 are the same sprite
                        if(sr1.sprite == sr2.sprite){
                            //increment the matchLength integer
                            matchLength++;
						
                            //if the sprites are not the same, break
                        } else {
                            break;
                        }
					
                        //if the "other" GameObject is null, break
                    } else {
                        break;
                    }
                }
            }
		
            //return the matchLength variable (which lists how many of the same sprite are next to each other
            //vertically top to bottom
            return matchLength;
        }

        //additional code added on top of the initial GetAllMatchTokens function which accounts for vertical matches
        public override List<GameObject> GetAllMatchTokens()
        {
            List<GameObject> tokensToRemove = base.GetAllMatchTokens();

            //loop through the entire grid 
            for(int y = 0; y < gameManager.gridHeight; y++){
                for(int x = 0; x < gameManager.gridWidth ; x++){
				
                    //as long as y is more than 2 spaces away from the bottom of the grid,
                    if(y < gameManager.gridHeight - 2){

                        //Run the GetVerticalMatchLength function on the currently checked space
                        //and set the verticalMatchLength integer to what is returned
                        int verticalMatchLength = GetVerticalMatchLength(x, y);

                        //if the verticalMatchLength variable is larger than 2
                        //(meaning there are 3 of the same token in a row vertically)
                        if(verticalMatchLength > 2){

                            //loop through the three matching tokens
                            for(int i = y; i < y + verticalMatchLength; i++){
							
                                //Assign the token in the space currently being checked to the token variable
                                GameObject token = gameManager.gridArray[x, i]; 
							
                                //and add that token to the tokensToRemove list
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
