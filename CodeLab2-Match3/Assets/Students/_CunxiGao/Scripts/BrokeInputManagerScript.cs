using UnityEngine;
using System.Collections;

public class BrokeInputManagerScript : MonoBehaviour {

	protected GameManagerScript gameManager;
	protected MoveTokensScript moveManager;
	protected GameObject selected = null;

	//On start, pull two other scripts to be used in this one.
	public virtual void Start () {
		moveManager = GetComponent<MoveTokensScript>();
		gameManager = GetComponent<GameManagerScript>();
	}

	//A function that allows players to select two tokens and initiate a swapping of their positions
	public virtual void SelectToken(){
		
		//On left mouse button click, 
		if(Input.GetMouseButtonDown(0)){
			
			//sets mousePos variable to the current location of the mouse on screen
			Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			
			//assigns the tokenCollider variable to the specific token that you are mousing over
			Collider2D tokenCollider = Physics2D.OverlapPoint(mousePos);

			//if there is a tokenCollider initialized,
			if(tokenCollider != null){
				
				//and if the player has not yet clicked on any tokens prior to this
				if(selected == null){
					
					//set the "selected" variable to the token that has just been clicked
					selected = tokenCollider.gameObject; 
					
					//if the player has already clicked a token before,
				} else {
					//set pos1 to the position of the token selected first
					Vector2 pos1 = gameManager.GetPositionOfTokenInGrid(selected);
					
					//set pos2 to the position of the token selected second
					Vector2 pos2 = gameManager.GetPositionOfTokenInGrid(tokenCollider.gameObject);
					
					//if the xs are the same and the ys are one off from each other
					//or the ys are the same and the xs are one off from each other
					if(Mathf.Abs((pos1.x - pos2.x) + (pos1.y - pos2.y)) == 1){ 
						
						
						
						//call the SetupTokenExchange function from the MoveTokensScript, to lerp the two selected tokens
						//to each other's position
						moveManager.SetupTokenExchange(selected, pos1, tokenCollider.gameObject, pos2, true);
					}
					//clear the selected variable
					selected = null;
				}
			}
		}

	}
}
