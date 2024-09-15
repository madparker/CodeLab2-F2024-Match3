using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DabuLyu;

namespace DabuLyu
{
    public class PlayerSpawner : MonoBehaviour
{
    public float spawnRate = 1f; 
    public float moveTime = 1f;
    public float waitTime = 1f;
    public float moveSpeed = 1f;
    public FixedMatchManagerScript matchManager; 

    
    private Dictionary<string, GameObject> tokenPrefabs = new Dictionary<string, GameObject>();

    void Start()
    {
       
        
        Object[] tokenObjects = Resources.LoadAll("_Core/Tokens", typeof(GameObject));
        foreach (var obj in tokenObjects)
        {
            GameObject tokenPrefab = obj as GameObject;
            if (tokenPrefab != null)
            {
                string tokenType = tokenPrefab.GetComponent<SpriteRenderer>().sprite.name;
                tokenPrefabs[tokenType] = tokenPrefab;
                
                
                Debug.Log($"Loaded token prefab: tokenType = {tokenType}, prefab name = {tokenPrefab.name}");
            }
            else
            {
                
            }
        }

        
        StartCoroutine(Spawn());
    }

    IEnumerator Spawn()
    {
        int iteration = 0;
        while (true)
        {
            iteration++; 

            FixedMatchManagerScript.MatchGroup matchGroup = matchManager.DequeueMatchGroup();
            if (matchGroup != null)
            {
                Debug.Log($"matchGroup.tokenType: {matchGroup.tokenType} at iteration {iteration}");
                
                
                
                if (tokenPrefabs.TryGetValue(matchGroup.tokenType, out GameObject tokenPrefab))
                {
                    GameObject token = Instantiate(tokenPrefab);
                    token.transform.position = transform.position;
                    
                    token.tag = "Player";
                    Rigidbody2D rb = token.AddComponent<Rigidbody2D>();
                    rb.bodyType = RigidbodyType2D.Kinematic;
                    TokenCollision tokenCollision = token.AddComponent<TokenCollision>();
                    tokenCollision.targetTag = "Enemy";
                    
                    

                    TokenMovement tokenMovement = token.AddComponent<TokenMovement>();
                    tokenMovement.speed = moveSpeed;
                    tokenMovement.moveTime = moveTime;
                    tokenMovement.waitTime = waitTime;
                    //Debug.Log($"spawn {matchGroup.tokenType} at iteration {iteration}");
                    matchManager.DebugMatchGroups();
                }
                else
                {
                    Debug.LogWarning($"no token spawn for {matchGroup.tokenType} at iteration {iteration}");
                }
            }
            else
            {
                
                Debug.Log($"matchGroups is empty at iteration {iteration}");
            }

            yield return new WaitForSeconds(spawnRate);
        }
    }

}

}
