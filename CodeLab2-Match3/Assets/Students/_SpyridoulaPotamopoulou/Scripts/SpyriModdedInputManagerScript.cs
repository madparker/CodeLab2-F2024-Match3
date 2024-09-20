using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SpyriModdedInputManagerScript : InputManagerScript
{
    public bool playerMadeMove = false;
    
    //a modified version of the SelectToken script whose only change is that it sets a bool, 
    //playerMadeMove, to true, once the "selected" variable has been assigned a non-null value for the first time
    //which indicates that the player has clicked on a token
    public override void SelectToken()
    {
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
                    //and sets playerMadeMove to true
                    selected = tokenCollider.gameObject;
                    playerMadeMove = true;
                    
                    //if the player has already clicked a token before,
                } else {
                    
                    //set pos1 to the position of the token selected first
                    Vector2 pos1 = gameManager.GetPositionOfTokenInGrid(selected);
					
                    //set pos2 to the position of the token selected second
                    Vector2 pos2 = gameManager.GetPositionOfTokenInGrid(tokenCollider.gameObject);
					
                    //If the two tokens are next to each other,
					
                    if(Mathf.Approximately((pos1 - pos2).magnitude, 1))
                    {
						
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
