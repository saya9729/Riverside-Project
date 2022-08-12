using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Player
{
    public class PlayerAnimatorAPIHandler : MonoBehaviour
    {
        private PlayerActionStateManager _playerActionStateManager;
        // Start is called before the first frame update
        void Start()
        {
            _playerActionStateManager = GetComponentInParent<PlayerActionStateManager>();
        }

        public void EnableAttackHitbox()
        {
            _playerActionStateManager.EnableAttackHitbox();
        }

        public void DisableAttackHitbox()
        {
            _playerActionStateManager.DisableAttackHitbox();
        }
    }
}