using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TokenObjectPool : MonoBehaviour
{
    protected Object[] tokenTypes;
    
    // Start is called before the first frame update
    void Start()
    {
        tokenTypes = (Object[])Resources.LoadAll("_Core/Tokens/"); //load all the token prefabs
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject GetToken(Vector3 position)
    {
        GameObject token = 
            Instantiate(tokenTypes[Random.Range(0, tokenTypes.Length)], 
                position, 
                Quaternion.identity) as GameObject;

        return token;
    }

    public void RemoveToken(GameObject token)
    {
        Destroy(token);
    }
}
