using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TokenObjectPool : MonoBehaviour
{
    protected Queue<GameObject> objectPool = new Queue<GameObject>();
    
    protected Object[] tokenTypes;
    protected Sprite[] spriteTypes; 
    
    // Start is called before the first frame update
    void Start()
    {
        tokenTypes = (Object[])Resources.LoadAll("_Core/Tokens/"); //load all the token prefabs
        spriteTypes = Resources.LoadAll<Sprite>("_Core/Images");
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual GameObject GetToken(Vector3 position)
    {
        GameObject token;
        
        if (objectPool.Count == 0)
        {
            token =
                Instantiate(tokenTypes[Random.Range(0, tokenTypes.Length)],
                    position,
                    Quaternion.identity) as GameObject;
        }
        else
        {
            token = objectPool.Dequeue();
            Reset(token, position);
        }

        return token;
    }

    public void RemoveToken(GameObject token)
    {
        token.SetActive(false);
        
        objectPool.Enqueue(token);
    }

    public virtual void Reset(GameObject token, Vector3 position)
    {
        token.SetActive(true);
        token.transform.position = position;
        Sprite newSprite = spriteTypes[Random.Range(0, spriteTypes.Length)];
        token.GetComponent<SpriteRenderer>().sprite = newSprite;
        token.name = newSprite.name;
    }
}
