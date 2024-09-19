using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeikaiJin
{
    public class TokenObjectPool_Kai : TokenObjectPool
    {
        public float bombRatio = 0.05f;
        public Material bombMat;
        public Material defaultMat;
        void SetBomb(GameObject obj)
        {
            var sr = obj.GetComponent<SpriteRenderer>();
            if (Random.Range(0f, 1f) < bombRatio)
            {
                sr.material = bombMat;
                obj.tag = "bomb";
            }
            else
            {
                sr.material = defaultMat;;
                obj.tag = "Untagged";
            }
        }
        
        public override GameObject GetToken(Vector3 position)
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

            SetBomb(token);
            return token;
        }
    }
}