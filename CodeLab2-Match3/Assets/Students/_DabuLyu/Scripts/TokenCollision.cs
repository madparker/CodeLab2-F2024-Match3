using UnityEngine;

namespace DabuLyu
{
    public class TokenCollision : MonoBehaviour
    {
        public string targetTag;

        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag(targetTag))
            {
                
                Destroy(other.gameObject);
                
                Destroy(gameObject);
            }
        }
    }
}
