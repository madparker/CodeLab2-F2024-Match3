using UnityEngine;
using System.Collections;

public class VivianFixedRepopulateScript : MonoBehaviour {
	
	protected VivianFixedGameManagerScript gameManager;

	public virtual void Start () {
		gameManager = GetComponent<VivianFixedGameManagerScript>();
	}

	//fill all empty spaces with new tokens in the grid
	public virtual void AddNewTokensToRepopulateGrid(){
		for(int x = 0; x < gameManager.gridWidth; x++){ //going through all the x positions
			GameObject token = gameManager.gridArray[x, gameManager.gridHeight - 1]; //only getting the top row
			if(token == null){ //if the top row has a hole
				//make a new token at the top
				gameManager.AddTokenToPosInGrid(x, gameManager.gridHeight - 1, gameManager.grid); 
			}
		}
	}
}
