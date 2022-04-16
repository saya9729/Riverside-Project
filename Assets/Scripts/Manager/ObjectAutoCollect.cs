using UnityEngine;

namespace Player
{
    public class ObjectAutoCollect : MonoBehaviour
    {
        private void OnTriggerEnter(Collider collider)
        {
            if (collider.tag == "Player")
            {
                //HP Potion collected
                this.PostEvent(EventID.onHPPotCollected);
                //destroy money object
                Destroy(gameObject);
            }
        }
    }
}