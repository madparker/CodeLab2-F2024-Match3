using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VivianChen
{
    public class VivianGameManagerScript : GameManagerScript
    {
        [SerializeField] private int score = 0; // Count how many match-3 has been achieved 

        public int Score
        {
            get
            {
                return score;
            }
            set
            {
                score = value;
                //Debug.Log(score);

                // If the score is now bigger than the trigger,
                // start the bonus time
                if (score >= bonusTrigger)
                {
                    isBonusTime = true;
                    timer = bonusTime;
                }
            }
        }

        private bool isBonusTime = false;
        [SerializeField] private int bonusTrigger = 15; // Set the amount to trigger bonus time 
        private int timer = 0;
        [SerializeField] private int bonusTime = 500; // How long will the bonus time lasts

        public override void Update()
        {
            if(!GridHasEmpty()){
                //ask the match manager if there are any matches
                if(matchManager.GridHasMatch()){
                    //if there are matches, ask the match manager for a list of all matches and remove them
                    RemoveAllMatchTokens(matchManager.GetAllMatchTokens());
                } 
                else {
                    // If it's no in the bonus time, do normal switching
                    if (!isBonusTime)
                    {
                        //if there are no matches and the grid is full, check for player input
                        inputManager.SelectToken();
                    }
                    // If it's in the bonus time, do bonus
                    else
                    {
                        if (timer > 0)
                        {
                            BonusTime();
                        }
                        else
                        {
                            BonusTimeEnd();
                        }
                    }
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
        
        // Bonus Time,
        // in the bonus time, for the first click,
        // click the token to eliminate all identical tokens
        protected void BonusTime()
        {
            // Resetting the score to 0
            score = 0;
            
            timer--;
                
            // Change the background color to indicate bonus time
            Camera.main.backgroundColor = Color.yellow;

            // When the mouse button is pressed
            if (Input.GetMouseButtonDown(0))
            {
                //sets mousePos variable to the current location of the mouse on screen
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			
                //assigns the tokenCollider variable to the specific token that you are mousing over
                Collider2D tokenCollider = Physics2D.OverlapPoint(mousePos);
                Sprite tokenSprite = tokenCollider.gameObject.GetComponent<SpriteRenderer>().sprite;
                
                // Going through the entire grid, finding the same tokens
                for (int x = 0; x < gridWidth; x++)
                {
                    for (int y = 0; y < gridHeight; y++)
                    {
                        GameObject token = gridArray[x, y];
                        Sprite sprite = token.gameObject.GetComponent<SpriteRenderer>().sprite;
                        
                        // If the token is the same as the clicked one,
                        // remove it from the grid
                        if (sprite == tokenSprite)
                        {
                            // TODO: Wired bug -- if I use the Obj Pool, the empty space won't be refilled
                            //tokenObjectPool.RemoveToken(gridArray[x, y]);
                            
                            Destroy(token);
                        }
                    }
                }
                
                // After the first click, end the bonus time
                BonusTimeEnd();
            }
        }

        protected void BonusTimeEnd()
        {
            // Reset the background color
            Camera.main.backgroundColor = Color.white;
            
            // Reset the bool
            isBonusTime = false;
            
            // Reset timer
            timer = 0;
        }
    }
}
