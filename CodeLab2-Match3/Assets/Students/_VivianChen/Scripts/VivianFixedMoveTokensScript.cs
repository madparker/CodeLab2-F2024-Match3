using UnityEngine;
using System.Collections;

public class VivianFixedMoveTokensScript : MonoBehaviour {

	protected VivianFixedGameManagerScript gameManager;
	protected VivianFixedMatchManagerScript matchManager;

	//Show these variables in the inspector
	public bool move = false;
	
	[SerializeField] float lerpPercent;
	[SerializeField] float lerpSpeed;
	//allow you to see and manipulate variables in the inspector even if it's a private variable
	//setting a userSwap boolean
	bool userSwap;
	//Object Oriented Programming, if this variable is protected, can only be seen by the script or any script that extends this script. (extending from this script like DailyGenerator extends form seed generator)
	protected GameObject exchangeToken1;
	GameObject exchangeToken2;

	Vector2 exchangeGridPos1;
	Vector2 exchangeGridPos2;

	//this runs at the start
	public virtual void Start () { 
		//getting the component of the scripts on the game object
		//and assigning them into the variables
		gameManager = GetComponent<VivianFixedGameManagerScript>();
		matchManager = GetComponent<VivianFixedMatchManagerScript>();
		lerpPercent = 0; //starting lerp percent at 0
	}

	//this runs every frame
	//function controlling the specific ways the icons move
	public virtual void Update () {
		
		if(move){
			lerpPercent += lerpSpeed * Time.deltaTime; //ensuring lerp changes using deltaTime

			if(lerpPercent >= 1){ //making sure it does not go above one, if it does set to 1
				lerpPercent = 1;
			}

			if(exchangeToken1 != null){ 
				ExchangeTokens(); //calling Exchange Tokens function if the token exists
			}
		}
	}

	public void SetupTokenMove(){
		//setting move bool to true to be called in the cases the tokens move
		move = true;
		lerpPercent = 0;
	}

	public void SetupTokenExchange(GameObject token1, Vector2 pos1,
	                               GameObject token2, Vector2 pos2, bool reversable){
		//getting the parameters of the first two tokens and their relative position and creating a bool parameter
		SetupTokenMove();

		exchangeToken1 = token1;
		exchangeToken2 = token2;
		//tokens being swapped

		exchangeGridPos1 = pos1;
		exchangeGridPos2 = pos2;
		//the two positions being exchanged

		this.userSwap = reversable;
		//a match was not made, must reverse the movement
	}

	//this function is used to change the positions of the tokens being exchanged
	public virtual void ExchangeTokens(){
		
		Vector3 startPos = gameManager.GetWorldPositionFromGridPosition((int)exchangeGridPos1.x, (int)exchangeGridPos1.y);
		Vector3 endPos = gameManager.GetWorldPositionFromGridPosition((int)exchangeGridPos2.x, (int)exchangeGridPos2.y);
		//setting the start position and end position vectors
		
		Vector3 movePos1 = Vector3.Lerp(startPos, endPos, lerpPercent);
		Vector3 movePos2 = Vector3.Lerp(endPos, startPos, lerpPercent);
		//the way the tokens move as vectors
		
		exchangeToken1.transform.position = movePos1;
		exchangeToken2.transform.position = movePos2;
		//giving the tokens their new positions

		if(lerpPercent >= 1){
			//if the lerp value is greater or equal to one then place the token in the exchanged grid position
			gameManager.gridArray[(int)exchangeGridPos2.x, (int)exchangeGridPos2.y] = exchangeToken1;
			gameManager.gridArray[(int)exchangeGridPos1.x, (int)exchangeGridPos1.y] = exchangeToken2;	
			
			//if there has been a swap but it's not a match, then reset the grid positions set moving to false with no exchanged tokens
			if(!matchManager.GridHasMatch() && userSwap){
				SetupTokenExchange(exchangeToken1, exchangeGridPos2, exchangeToken2, exchangeGridPos1, false);
			} else {
				exchangeToken1 = null;
				exchangeToken2 = null;
				move = false;
			}
		}
	}

	public virtual void MoveTokenToEmptyPos(int startGridX, int startGridY,
	                                int endGridX, int endGridY,
	                                GameObject token){
	//when a token is moved to an empty positions without another token present
		Vector3 startPos = gameManager.GetWorldPositionFromGridPosition(startGridX, startGridY);
		Vector3 endPos = gameManager.GetWorldPositionFromGridPosition(endGridX, endGridY);
		//vectors of the start and end position of the token being moved into an empty positions

		Vector3 pos = Vector3.Lerp(startPos, endPos, lerpPercent);
		//the way the token moves to the positions using lerp
		
		token.transform.position =	pos;
		//setting the token's position as a variable
		if(lerpPercent >= 1){ 
			//if lerp is greater or equal to one, setting the token's end positions in the grid and setting the start positions in the grid as null
			gameManager.gridArray[endGridX, endGridY] = token;
			gameManager.gridArray[startGridX, startGridY] = null;
		}
	}

	public virtual bool MoveTokensToFillEmptySpaces(){ //boolen for true or false if a token is filling an empty space
		bool movedToken = false;
		//checking to see if the positions around token to see if they're empty, up and down only, making sure tokens when there's empty space below it
		for(int x = 0; x < gameManager.gridWidth; x++){
			for(int y = 0; y < gameManager.gridHeight; y++){
				if(gameManager.gridArray[x, y] == null){ //lopping through the grid array and checking if they are empty
					for(int pos = y; pos < gameManager.gridHeight; pos++){ //if the position is inside the height of the grid then continue
						GameObject token = gameManager.gridArray[x, pos]; //setting the token's x value and y posiiton as the pos variable
						if(token != null){ //if the token is there, then call move the token to empty position function
							MoveTokenToEmptyPos(x, pos, x, pos - 1, token);
							movedToken = true;
						}
					}
					//break; 
				}
			}
		}

		if(lerpPercent >= 1){
			move = false;
			lerpPercent = 0;
		} //resets lerp movement

		return movedToken; //returns the moved token
	}
}
