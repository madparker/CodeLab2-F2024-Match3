using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace NengkuanChen
{
    public class FixedMatchManagerScript: MatchManagerScript
    {
        
        public override bool GridHasMatch()
        {
            return GetAllMatchTokens().Count > 0;
        }
        
        /// <summary>
        /// Get all the matches in the grid.
        /// </summary>
        /// <returns>
        /// Returns a list of GameObjects that are in the matches.
        /// </returns>
        public override List<GameObject> GetAllMatchTokens(){
            HashSet<Vector2Int> allMatches = GetHorizontalMatches();
            allMatches.UnionWith(GetVerticalMatches());
            List<GameObject> tokensToRemove = new List<GameObject>();
            foreach (var position in allMatches)
            {
                tokensToRemove.Add(gameManager.gridArray[position.x, position.y]);
            }
            return tokensToRemove;
        }
        
        /// <summary>
        /// Get all the vertical matches in the grid.
        /// </summary>
        /// <returns>
        /// Returns all the positions that matches.
        /// </returns>
        private HashSet<Vector2Int> GetVerticalMatches()
        {
            HashSet<Vector2Int> matchList = new HashSet<Vector2Int>();
            List<Vector2Int> columnMatches = new List<Vector2Int>();
            for (int i = 0; i < gameManager.gridWidth; i++)
            {
                GetSingleColumnMatches(i, ref columnMatches);
                foreach (Vector2Int pairs in columnMatches)
                {
                    for (int j = pairs.x; j <= pairs.y; j++)
                    {
                        matchList.Add(new Vector2Int(i, j));
                    }
                }
            }
            return matchList;
        }
        
        /// <summary>
        /// Get all the matches in a single column.
        /// </summary>
        /// <param name="column"></param>
        /// <param name="matchList">
        /// Pass by reference to avoid GC.
        /// Each element in the list is a Vector2, representing the start and end index of the match.
        /// </param>
        private void GetSingleColumnMatches(int column, ref List<Vector2Int> matchList)
        {
            matchList.Clear();
            int gridHeight = gameManager.gridHeight;
            int count = 1;
            Sprite currentSprite = gameManager.gridArray[column, 0].GetComponent<SpriteRenderer>().sprite;
            for(int i = 1; i < gridHeight; i++)
            {
                Sprite nextSprite = gameManager.gridArray[column, i].GetComponent<SpriteRenderer>().sprite;
                if (nextSprite == currentSprite)
                {
                    count++;
                    continue;
                }
                if (count >= 3)
                {
                    matchList.Add(new Vector2Int(i - count, i - 1));
                }
                count = 1;
                currentSprite = nextSprite;
            }
            if (count >= 3)
            {
                matchList.Add(new Vector2Int(gridHeight - count - 1, gridHeight - 1));
            }
        }
        
        
        
        /// <summary>
        /// Get all the horizontal matches in the grid.
        /// </summary>
        /// <returns>Returns all the positions that matches.</returns>
        private HashSet<Vector2Int> GetHorizontalMatches()
        {
            HashSet<Vector2Int> matchList = new HashSet<Vector2Int>();
            List<Vector2Int> rowMatches = new List<Vector2Int>();
            for (int i = 0; i < gameManager.gridHeight; i++)
            {
                GetSingleRowMatches(i, ref rowMatches);
                foreach (Vector2Int pairs in rowMatches)
                {
                    for (int j = pairs.x; j <= pairs.y; j++)
                    {
                        matchList.Add(new Vector2Int(j, i));
                    }
                }
            }
            return matchList;
        }
        
        
        /// <summary>
        /// Get all the matches in a single row.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="matchList">
        /// Pass by reference to avoid GC.
        /// Each element in the list is a Vector2, representing the start and end index of the match.
        /// </param>
        private void GetSingleRowMatches(int row, ref List<Vector2Int> matchList)
        {
            matchList.Clear();
            int gridWidth = gameManager.gridWidth;
            int count = 1;
            Sprite currentSprite = gameManager.gridArray[0, row].GetComponent<SpriteRenderer>().sprite;
            for(int i = 1; i < gridWidth; i++)
            {
                Sprite nextSprite = gameManager.gridArray[i, row].GetComponent<SpriteRenderer>().sprite;
                if (nextSprite == currentSprite)
                {
                    count++;
                    continue;
                }
                if (count >= 3)
                {
                    matchList.Add(new Vector2Int(i - count, i - 1));
                }
                count = 1;
                currentSprite = nextSprite;
            }
            if (count >= 3)
            {
                matchList.Add(new Vector2Int(gridWidth - count - 1, gridWidth - 1));
            }
        }
        
    }
}
