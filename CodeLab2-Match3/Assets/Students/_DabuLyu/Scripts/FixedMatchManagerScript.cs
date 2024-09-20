using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace DabuLyu
{
    public class FixedMatchManagerScript : MatchManagerScript
    {
        public class MatchGroup
        {
            public string tokenType;
            public int count;

            public MatchGroup(string tokenType, int count)
            {
                this.tokenType = tokenType;
                this.count = count;
            }
        }

        private Queue<MatchGroup> matchGroups = new Queue<MatchGroup>();

        public void DebugMatchGroups()
        {
            if (matchGroups.Count == 0)
            {
                Debug.Log("MatchGroups is empty.");
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("Current：");
                foreach (var matchGroup in matchGroups)
                {
                    sb.AppendFormat(" [tokenType: {0}, count: {1}]", matchGroup.tokenType, matchGroup.count);
                }

                Debug.Log(sb.ToString());
            }
        }

        public MatchGroup DequeueMatchGroup()
        {
            if (matchGroups.Count > 0)
            {
                Debug.Log("MatchGroupCount" +matchGroups.Count);
                DebugMatchGroups();
                DebugMatchGroups();
                return matchGroups.Dequeue();
            }
            else
            {
                return null;
            }
        }

        public override bool GridHasMatch()
        {
            // Combine base match check and additional vertical match check
            bool match = base.GridHasMatch();
            for (int x = 0; x < gameManager.gridWidth; x++)
            {
                for (int y = 0; y < gameManager.gridHeight; y++)
                {
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

        public override List<GameObject> GetAllMatchTokens()
        {
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
                            
                            //add to queue
                            Sprite currentSprite = gameManager.gridArray[x, y].GetComponent<SpriteRenderer>().sprite;
                            matchGroups.Enqueue(new MatchGroup(currentSprite.name, horizonMatchLength));

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

            // Check for vertical matches
            for (int x = 0; x < gameManager.gridWidth; x++)
            {
                for (int y = 0; y < gameManager.gridHeight; y++)
                {
                    if (y < gameManager.gridHeight - 2)
                    {
                        int verticalMatchLength = GetVerticalMatchLength(x, y);

                        if (verticalMatchLength > 2)
                        {
                            //add to queue
                            Sprite currentSprite = gameManager.gridArray[x, y].GetComponent<SpriteRenderer>().sprite;
                            matchGroups.Enqueue(new MatchGroup(currentSprite.name, verticalMatchLength));
                            Debug.Log("add " + currentSprite.name + " count: " + verticalMatchLength);
                            for (int i = y; i < y + verticalMatchLength; i++)
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

        // Additional methods to find horizontal and vertical matches
        private HashSet<Vector2Int> GetVerticalMatches()
        {
            HashSet<Vector2Int> matchList = new HashSet<Vector2Int>();
            for (int x = 0; x < gameManager.gridWidth; x++)
            {
                List<Vector2Int> columnMatches = new List<Vector2Int>();
                GetSingleColumnMatches(x, ref columnMatches);
                foreach (Vector2Int pairs in columnMatches)
                {
                    for (int y = pairs.x; y <= pairs.y; y++)
                    {
                        matchList.Add(new Vector2Int(x, y));
                    }
                }
            }

            return matchList;
        }

        private void GetSingleColumnMatches(int column, ref List<Vector2Int> matchList)
        {
            int gridHeight = gameManager.gridHeight;
            int count = 1;
            Sprite currentSprite = gameManager.gridArray[column, 0].GetComponent<SpriteRenderer>().sprite;

            for (int y = 1; y < gridHeight; y++)
            {
                Sprite nextSprite = gameManager.gridArray[column, y].GetComponent<SpriteRenderer>().sprite;
                if (nextSprite == currentSprite)
                {
                    count++;
                }
                else
                {
                    if (count >= 3)
                    {
                        matchList.Add(new Vector2Int(y - count, y - 1));
                        matchGroups.Enqueue(new MatchGroup(currentSprite.name, count));
                        Debug.Log("add " + currentSprite.name + " count: " + count);
                    }

                    count = 1;
                    currentSprite = nextSprite;
                }
            }

            // Handle the final match in the column
            if (count >= 3)
            {
                matchList.Add(new Vector2Int(gridHeight - count, gridHeight - 1));
                matchGroups.Enqueue(new MatchGroup(currentSprite.name, count));
                Debug.Log("add " + currentSprite.name + " count: " + count);
            }
        }

        private HashSet<Vector2Int> GetHorizontalMatches()
        {
            HashSet<Vector2Int> matchList = new HashSet<Vector2Int>();
            for (int y = 0; y < gameManager.gridHeight; y++)
            {
                List<Vector2Int> rowMatches = new List<Vector2Int>();
                GetSingleRowMatches(y, ref rowMatches);
                foreach (Vector2Int pairs in rowMatches)
                {
                    for (int x = pairs.x; x <= pairs.y; x++)
                    {
                        matchList.Add(new Vector2Int(x, y));
                    }
                }
            }

            return matchList;
        }

        private void GetSingleRowMatches(int row, ref List<Vector2Int> matchList)
        {
            int gridWidth = gameManager.gridWidth;
            int count = 1;
            Sprite currentSprite = gameManager.gridArray[0, row].GetComponent<SpriteRenderer>().sprite;

            for (int i = 1; i < gridWidth; i++)
            {
                Sprite nextSprite = gameManager.gridArray[i, row].GetComponent<SpriteRenderer>().sprite;
                if (nextSprite == currentSprite)
                {
                    count++;
                }
                else
                {
                    if (count >= 3)
                    {
                        matchList.Add(new Vector2Int(i - count, i - 1));
                        matchGroups.Enqueue(new MatchGroup(currentSprite.name, count));
                        Debug.Log("add " + currentSprite.name + " count: " + count);
                    }

                    count = 1;
                    currentSprite = nextSprite;
                }
            }

            // Handle the final match in the row
            if (count >= 3)
            {
                matchList.Add(new Vector2Int(gridWidth - count, gridWidth - 1));
                matchGroups.Enqueue(new MatchGroup(currentSprite.name, count));
                Debug.Log("add " + currentSprite.name + " count: " + count);
            }
        }


        //
        // //Learn from NengkuanChen
        // public class FixedMatchManagerScript : MatchManagerScript
        // {
        //     public class MatchGroup
        //     {
        //         public string tokenType;
        //         public int count;
        //         
        //         public MatchGroup(string tokenType, int count)
        //         {
        //             this.tokenType = tokenType;
        //             this.count = count;
        //         }
        //         
        //     }
        //     private Queue<MatchGroup> matchGroups = new Queue<MatchGroup>();
        //     
        //     public void DebugMatchGroups()
        //     {
        //         if (matchGroups.Count == 0)
        //         {
        //             Debug.Log("MatchGroups is empty.");
        //         }
        //         else
        //         {
        //             StringBuilder sb = new StringBuilder();
        //             sb.Append("Current：");
        //             foreach (var matchGroup in matchGroups)
        //             {
        //                 sb.AppendFormat(" [tokenType: {0}, count: {1}]", matchGroup.tokenType, matchGroup.count);
        //             }
        //             Debug.Log(sb.ToString());
        //         }
        //     }
        //     public MatchGroup DequeueMatchGroup()
        //     {
        //         
        //         if (matchGroups.Count > 0)
        //         {
        //             Debug.Log(matchGroups.Count);
        //             return matchGroups.Dequeue();
        //             //debug the number of matchGroups
        //             
        //         }
        //         else
        //         {
        //             return null;
        //         }
        //
        //     }
        //     
        //     public override bool GridHasMatch() => GetAllMatchTokens().Count > 0;
        //     
        //     public override List<GameObject> GetAllMatchTokens()
        //     {
        //                     
        //         
        //         
        //         // Get all the matches in the grid to a HashSet.
        //         HashSet<Vector2Int> allMatches = GetHorizontalMatches();
        //         // Union the vertical matches to the HashSet. Will not add duplicates.
        //         allMatches.UnionWith(GetVerticalMatches());
        //         List<GameObject> tokensToRemove = new List<GameObject>();
        //         
        //         
        //         
        //         foreach (var position in allMatches)
        //         {
        //             GameObject token = gameManager.gridArray[position.x, position.y];
        //             
        //             tokensToRemove.Add(token);
        //             
        //             
        //         }
        //         
        //         
        //
        //         return tokensToRemove;
        //     }
        //     
        //     // Get all the vertical matches using columnMatches.
        //     private HashSet<Vector2Int> GetVerticalMatches()
        //     {
        //         HashSet<Vector2Int> matchList = new HashSet<Vector2Int>();
        //         //List<Vector2Int> columnMatches = new List<Vector2Int>();
        //         for (int x = 0; x < gameManager.gridWidth; x++)
        //         {
        //             List<Vector2Int> columnMatches = new List<Vector2Int>();
        //             GetSingleColumnMatches(x, ref columnMatches);
        //             foreach (Vector2Int pairs in columnMatches)
        //             {
        //                 for (int y = pairs.x; y <= pairs.y; y++)
        //                 {
        //                     matchList.Add(new Vector2Int(x, y));
        //                 }
        //             }
        //         }
        //         return matchList;
        //     }
        //     private Dictionary<string, int> matchCountDict = new Dictionary<string, int>();
        //     
        //     // Get all the matches in a single column.
        //     private void GetSingleColumnMatches(int column, ref List<Vector2Int> matchList)
        //     {
        //         int gridHeight = gameManager.gridHeight;
        //         int count = 1;
        //         Sprite currentSprite = gameManager.gridArray[column, 0].GetComponent<SpriteRenderer>().sprite;
        //
        //         for (int y = 1; y < gridHeight; y++)
        //         {
        //             Sprite nextSprite = gameManager.gridArray[column, y].GetComponent<SpriteRenderer>().sprite;
        //             if (nextSprite == currentSprite)
        //             {
        //                 count++;
        //             }
        //             else
        //             {
        //                 if (count >= 3)
        //                 {
        //                     matchList.Add(new Vector2Int(y - count, y - 1));
        //                     matchGroups.Enqueue(new MatchGroup(currentSprite.name, count));
        //                     Debug.Log("add " + currentSprite.name + " count: " + count);
        //                 }
        //                 count = 1;
        //                 currentSprite = nextSprite;
        //             }
        //         }
        //         // Handle the final match in the column
        //         if (count >= 3)
        //         {
        //             matchList.Add(new Vector2Int(gridHeight - count, gridHeight - 1));
        //             matchGroups.Enqueue(new MatchGroup(currentSprite.name, count));
        //             Debug.Log("add " + currentSprite.name + " count: " + count);
        //         }
        //     }
        //
        //     
        //     private HashSet<Vector2Int> GetHorizontalMatches()
        //     {
        //         HashSet<Vector2Int> matchList = new HashSet<Vector2Int>();
        //         // List<Vector2Int> rowMatches = new List<Vector2Int>(); // Commented out in your code
        //         for (int y = 0; y < gameManager.gridHeight; y++)
        //         {
        //             List<Vector2Int> rowMatches = new List<Vector2Int>();
        //             GetSingleRowMatches(y, ref rowMatches);
        //             foreach (Vector2Int pairs in rowMatches)
        //             {
        //                 for (int x = pairs.x; x <= pairs.y; x++)
        //                 {
        //                     matchList.Add(new Vector2Int(x, y));
        //                 }
        //             }
        //         }
        //         return matchList;
        //     }
        //     
        //     private void GetSingleRowMatches(int row, ref List<Vector2Int> matchList)
        //     {
        //         int gridWidth = gameManager.gridWidth;
        //         int count = 1;
        //         Sprite currentSprite = gameManager.gridArray[0, row].GetComponent<SpriteRenderer>().sprite;
        //
        //         for (int i = 1; i < gridWidth; i++)
        //         {
        //             Sprite nextSprite = gameManager.gridArray[i, row].GetComponent<SpriteRenderer>().sprite;
        //             if (nextSprite == currentSprite)
        //             {
        //                 count++;
        //             }
        //             else
        //             {
        //                 if (count >= 3)
        //                 {
        //                     matchList.Add(new Vector2Int(i - count, i - 1));
        //                     matchGroups.Enqueue(new MatchGroup(currentSprite.name, count));
        //                     Debug.Log("add " + currentSprite.name + " count: " + count);
        //                 }
        //                 count = 1;
        //                 currentSprite = nextSprite;
        //             }
        //         }
        //         // Handle the final match in the row
        //         if (count >= 3)
        //         {
        //             matchList.Add(new Vector2Int(gridWidth - count, gridWidth - 1));
        //             matchGroups.Enqueue(new MatchGroup(currentSprite.name, count));
        //             Debug.Log("add " + currentSprite.name + " count: " + count);
        //         }
        //     }
        //
        //     
        // }


    }
}

