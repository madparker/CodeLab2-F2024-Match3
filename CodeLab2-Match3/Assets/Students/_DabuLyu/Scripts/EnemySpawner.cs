using System.Collections;
using System.Collections.Generic;
using DabuLyu;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public float spawnRate = 1f;
    public float spawnPossibility = 1f;
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
            yield return new WaitForSeconds(spawnRate);
            if (Random.value < spawnPossibility)
            {
                //randomly select a token type
                GameObject token = Instantiate(tokenTypes[Random.Range(0, tokenTypes.Length)]) as GameObject;
                token.transform.position = transform.position;
                TokenMovement tokenMovement = token.AddComponent <TokenMovement>();
                tokenMovement.speed = -1.0f;
            }
        }
    }
}
