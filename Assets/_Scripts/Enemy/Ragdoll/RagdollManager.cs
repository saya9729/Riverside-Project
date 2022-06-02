using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Enemy
{
    public class RagdollManager : MonoBehaviour
    {
        private RagdollBody[] _ragdollBody;
        // Start is called before the first frame update
        void Start()
        {
            _ragdollBody = GetComponentsInChildren<RagdollBody>();
        }

        public void EnableRagdoll()
        {
            foreach(RagdollBody body in _ragdollBody)
            {
                body.EnableRagdoll();
            }
        }
        public void DisableRagdoll()
        {
            foreach(RagdollBody body in _ragdollBody)
            {
                body.DisableRagdoll();
            }
        }

    }
}
