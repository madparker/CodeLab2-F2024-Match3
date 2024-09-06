using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MatchManagerScript : MonoBehaviour {

	protected GameManagerScript gameManager;

	//On start, get the game manager script so it can be used by this script
	public virtual void Start () {
		gameManager = GetComponent<GameManagerScript>();
	}

	//loops through the entire grid to check if there is a match or not
	public virtual bool GridHasMatch(){
		
		//Initialize the match bool to false
		bool match = false;
		
		//loop through the entire array 
		for(int x = 0; x < gameManager.gridWidth; x++){
			for(int y = 0; y < gameManager.gridHeight ; y++){
				
				//if the x position currently being checked is more than 2 spaces away from the right side
				//of the grid (because you cannot match 3 to the right if you're not at least 3 spaces away)
				if(x < gameManager.gridWidth - 2){
					
					//call the GridHasHorizontalMatch function to check if there is a match
					//and set the match variable to false or true depending on the outcome of GridHasHorizontalMatch
					match = match || GridHasHorizontalMatch(x, y);
				}
			}
		}
		
		//return the match variable (which can be false or true)
		return match;
	}

	//Checks the sprites of three tokens that are next to each other to see if they are the same
	//if they are, returns true, else returns false
	public bool GridHasHorizontalMatch(int x, int y){
		//initialize 3 tokens, next to each other horizontally to the right
		GameObject token1 = gameManager.gridArray[x + 0, y];
		GameObject token2 = gameManager.gridArray[x + 1, y];
		GameObject token3 = gameManager.gridArray[x + 2, y];
		
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

	//Returns how many of the same sprite(token) are next to each other in a continuous row horizontally to the right
	public int GetHorizontalMatchLength(int x, int y){
		
		//initialize the matchLength integer to 1
		int matchLength = 1;
		
		//initializes the first GameObject to the GameObject at the position that is fed into this function
		GameObject first = gameManager.gridArray[x, y];

		//if first has been initialized,
		if(first != null){
			
			//set the sr1 SpriteRenderer to the SpriteRenderer of the first GameObject
			SpriteRenderer sr1 = first.GetComponent<SpriteRenderer>();
			
			//loop through the array's width
			for(int i = x + 1; i < gameManager.gridWidth; i++){
				
				//assign the other GameObject to the i'th GameObject in the same row
				GameObject other = gameManager.gridArray[i, y];

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
		//to the right
		return matchLength;
	}

	//Returns a list of token GameObjects to be removed in the GameManagerScript's "RemoveAllMatchTokens" Function
	public virtual List<GameObject> GetAllMatchTokens(){
		
		//initialize the tokensToRemove list (of GameObjects)
		List<GameObject> tokensToRemove = new List<GameObject>();

		//loop through the entire grid 
		for(int x = 0; x < gameManager.gridWidth; x++){
			for(int y = 0; y < gameManager.gridHeight ; y++){
				
				//as long as x is more than 2 spaces away from the right edge of the grid,
				if(x < gameManager.gridWidth - 2){

					//Run the GetHorizontalMatchLength function on the currently checked space
					//and set the horizonMatchLength integer to what is returned
					int horizonMatchLength = GetHorizontalMatchLength(x, y);

					//if the horizonMatchLength variable is larger than 2
					//(meaning there are 3 of the same token in a row horizontally)
					if(horizonMatchLength > 2){

						//loop through the three matching tokens
						for(int i = x; i < x + horizonMatchLength; i++){
							
							//Assign the token in the space currently being checked to the token variable
							GameObject token = gameManager.gridArray[i, y]; 
							
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
