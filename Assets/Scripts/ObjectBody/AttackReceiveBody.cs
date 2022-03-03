using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class AttackReceiveBody : MonoBehaviour
    {
        private PlayerStateManager _playerStateManager;
        private PlayerAttackState _playerAttackState;

        void OnTriggerEnter(Collider other)
        {
            if (_playerAttackState.isAttacking)
            {
                if (other.tag == "attack") //come to contact with weapon or projectile have tag "attack"
                {
                    ReceiveDamage(_playerStateManager.playerAttackState.attackValue);
                }
            }
        }

        private void Start()
        {
            _playerStateManager = GameObject.Find("PlayerStateManager").GetComponent<PlayerStateManager>();
            _playerAttackState = GameObject.Find("PlayerState").GetComponent<PlayerAttackState>();
        }

        void ReceiveDamage(float dmg)
        {
            //HP -= dmg;
        }
    }
}
