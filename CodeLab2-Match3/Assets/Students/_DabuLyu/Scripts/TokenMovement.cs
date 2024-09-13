using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace DabuLyu
{
    enum TokenState
    {
        Idle,
        Moving,
        Waiting
    }
    
    public class TokenMovement : MonoBehaviour
    {
        TokenState state = TokenState.Idle;
        
        public float speed = 1.0f;
        
        public float moveTime = 1f;
        public float waitTime = 1f;
        
        //public float timer = 0f;
        void Start()
        {
            state = TokenState.Moving;
            StartCoroutine(Move());
        }
    
        // Update is called once per frame
        void Update()
        {
            if (state == TokenState.Moving)
            {
                transform.Translate( Vector3.right * speed * Time.deltaTime);
            }
        }
    
        public IEnumerator Move()
        {
            while (true)
            {
                if (state == TokenState.Idle)
                {
                    yield return null;
                }
                if (state == TokenState.Moving)
                {
                    yield return new WaitForSeconds(moveTime);
                    state = TokenState.Waiting;
                }
                
                if (state == TokenState.Waiting)
                {
                    yield return new WaitForSeconds(waitTime);
                    state = TokenState.Moving;
                }
            }
        }
    }

}
