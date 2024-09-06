using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManagerScript : MonoBehaviour {

	public int gridWidth = 8;
	public int gridHeight = 8;
	public float tokenSize = 1;

	protected MatchManagerScript matchManager;
	protected InputManagerScript inputManager;
	protected RepopulateScript repopulateManager;
	protected MoveTokensScript moveTokenManager;

	public GameObject grid;
	public  GameObject[,] gridArray;
	protected Object[] tokenTypes;
	GameObject selected;
	
	
	//Initialize
	public virtual void Start () {
		tokenTypes = (Object[])Resources.LoadAll("_Core/Tokens/"); //load all the token prefabs
		gridArray = new GameObject[gridWidth, gridHeight];
		MakeGrid();
		matchManager = GetComponent<MatchManagerScript>();
		inputManager = GetComponent<InputManagerScript>();
		repopulateManager = GetComponent<RepopulateScript>();
		moveTokenManager = GetComponent<MoveTokensScript>();
	}

	public virtual void Update(){
		if(!GridHasEmpty()){
			//ask the match manager if there are any matches
			if(matchManager.GridHasMatch()){
				//if there are matches, ask the match manager for a list of all matches and remove them
				RemoveAllMatchTokens(matchManager.GetAllMatchTokens());
			} 
			else {
				//if there are no matches and the grid is full, check for player input
				inputManager.SelectToken();
			}
		} 
		else 
		{
			//if grid has empty spaces, and moveTokenManager is not currently moving tokens
			if(!moveTokenManager.move){
				//then move tokens to fill empty spaces
				moveTokenManager.SetupTokenMove();
			}
			
			//if all empty spaces are filled
			if(!moveTokenManager.MoveTokensToFillEmptySpaces()){
				//repopulate new tokens
				repopulateManager.AddNewTokensToRepopulateGrid();
			}
		}
	}


	//called at the start to create the grid of tokens
	void MakeGrid() {
		grid = new GameObject("TokenGrid"); //make a gameObject to hold all the grid tokens in the scene
		//iterate through the grid and spawn a token at each position
		for(int x = 0; x < gridWidth; x++){
			for(int y = 0; y < gridHeight; y++){
				//spawn a token at the current grid position
				AddTokenToPosInGrid(x, y, grid);
			}
		}
	}
	
	//destroy all tokens in the list.
	public virtual void RemoveAllMatchTokens(List<GameObject> removeTokens){
		for(int x = 0; x < gridWidth; x++){
			for(int y = 0; y < gridHeight ; y++){
				if(removeTokens.Contains(gridArray[x, y])){
					Destroy(gridArray[x, y]);
					gridArray[x, y] = null;
				}
			}
		}
	}

//Iterate through the grid and check if there are any empty spaces
	public virtual bool GridHasEmpty(){
		for(int x = 0; x < gridWidth; x++){
			for(int y = 0; y < gridHeight ; y++){
				if(gridArray[x, y] == null){
					return true;
				}
			}
		}

		return false;
	}

	//Iterate through the grid and check if there are any matches
	//return position of the first match found, or (0,0) if no matches
	public Vector2 GetPositionOfTokenInGrid(GameObject token){
		for(int x = 0; x < gridWidth; x++){
			for(int y = 0; y < gridHeight ; y++){
				if(gridArray[x, y] == token){
					return(new Vector2(x, y));
				}
			}
		}
		return new Vector2();
	}
		
	//grid position to world position
	public Vector2 GetWorldPositionFromGridPosition(int x, int y){
		return new Vector2(
			(x - gridWidth/2) * tokenSize,
			(y - gridHeight/2) * tokenSize);
	}

	//spawn a token at a grid position
	public void AddTokenToPosInGrid(int x, int y, GameObject parent){
		Vector3 position = GetWorldPositionFromGridPosition(x, y);
		GameObject token = 
			Instantiate(tokenTypes[Random.Range(0, tokenTypes.Length)], 
			            position, 
			            Quaternion.identity) as GameObject;
		token.transform.parent = parent.transform;
		gridArray[x, y] = token;
	}
}
