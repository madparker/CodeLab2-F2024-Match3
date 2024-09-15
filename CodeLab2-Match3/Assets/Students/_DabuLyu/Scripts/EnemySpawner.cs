using System.Collections;
using System.Collections.Generic;
using DabuLyu;
using UnityEngine;

namespace DabuLyu
{
    public class EnemySpawner : MonoBehaviour
    {
        public float spawnRate = 1f;
        public float spawnPossibility = 1f;
        public float moveTime = 1f;
        public float waitTime = 1f;
        public float moveSpeed = 1f;
        Object[] tokenTypes;
    
        void Start()
        {
            tokenTypes = (Object[])Resources.LoadAll("_Core/Tokens/"); //load all the token prefabs
            StartCoroutine(Spawn());
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    
        public IEnumerator Spawn()
        {
            while (true)
            {
            
                if (Random.value < spawnPossibility)
                {
                    //randomly select a token type
                    GameObject token = Instantiate(tokenTypes[Random.Range(0, tokenTypes.Length)]) as GameObject;
                    token.transform.position = transform.position;
                    
                    token.tag = "Enemy";
                    Rigidbody2D rb = token.AddComponent<Rigidbody2D>();
                    rb.bodyType = RigidbodyType2D.Kinematic;
                    TokenCollision tokenCollision = token.AddComponent<TokenCollision>();
                    tokenCollision.targetTag = "Player";
                    
                    
                    TokenMovement tokenMovement = token.AddComponent <TokenMovement>();
                    tokenMovement.speed = - moveSpeed;
                    tokenMovement.moveTime = moveTime;
                    tokenMovement.waitTime = waitTime;
                }
                yield return new WaitForSeconds(spawnRate);
            }
        }
    }

}
