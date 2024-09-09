using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DabuLyu
{
    public class FixedMatchManagerScript : MatchManagerScript
    {

        public override bool GridHasMatch(){
            bool match = base.GridHasMatch();
            
            

            
            
            for (int x = 0; x < gameManager.gridWidth; x++)
            {
                for (int y = 0; y < gameManager.gridHeight; y++)
                {
                    // if (x < gameManager.gridWidth - 2)
                    // {
                    //     match = match || GridHasHorizontalMatch(x, y);
                    // }
                    if (y < gameManager.gridHeight - 2)
                    {
                        match = match || GridHasVerticalMatch(x, y);
                    }
                }
            }
            
            return match;
        }

        public bool GridHasVerticalMatch(int x, int y)
        {
            
            
            
            
            GameObject token1 = gameManager.gridArray[x, y + 0];
            GameObject token2 = gameManager.gridArray[x, y + 1];
            GameObject token3 = gameManager.gridArray[x, y + 2];
            
            

            if (token1 != null && token2 != null && token3 != null)
            {
                SpriteRenderer sr1 = token1.GetComponent<SpriteRenderer>();
                SpriteRenderer sr2 = token2.GetComponent<SpriteRenderer>();
                SpriteRenderer sr3 = token3.GetComponent<SpriteRenderer>();

                return (sr1.sprite == sr2.sprite && sr2.sprite == sr3.sprite);
                
                
            }
            else
            {
                return false;
            }
        }
        
        public int GetVerticalMatchLength(int x, int y)
        {
            
            
            
            int matchLength = 1;
            GameObject first = gameManager.gridArray[x, y];
            
            
            if (first != null)
            {
                SpriteRenderer sr1 = first.GetComponent<SpriteRenderer>();
                for (int i = y + 1; i < gameManager.gridHeight; i++)
                {
                    GameObject other = gameManager.gridArray[x, i];
                    if (other != null)
                    {
                        SpriteRenderer sr2 = other.GetComponent<SpriteRenderer>();
                        if (sr1.sprite == sr2.sprite)
                        {
                            matchLength++;
                        }
                        else
                        {
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }
            
            return matchLength;
            
            
        }


        public override List<GameObject> GetAllMatchTokens(){
            
            List<GameObject> tokensToRemove = base.GetAllMatchTokens();

            
            
            for (int x = 0; x < gameManager.gridWidth; x++)
            {
                for (int y = 0; y < gameManager.gridHeight; y++)
                {
                    
                    
                    
                    if (y < gameManager.gridHeight - 2)
                    {
                        int verticalMatchLength = GetVerticalMatchLength(x, y);
                        
                        if (verticalMatchLength > 2)
                        {
                            for (int i = y; i < y+ verticalMatchLength; i++)
                            {
                                GameObject token = gameManager.gridArray[x, i];
                                if (!tokensToRemove.Contains(token))
                                {
                                    tokensToRemove.Add(token);
                                }
                            }
                        }
                    }
                }
            }
            
            return tokensToRemove;
        }
    }
}

