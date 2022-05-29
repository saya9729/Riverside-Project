using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Enemy
{
    public class EnemyStatisticManager : AbstractClass.StatisticManager
    {
        private EnemyStateManager _enemyStateManager;

        protected override void InitializeVariable()
        {
            _enemyStateManager = GetComponent<EnemyStateManager>();
            health = maxHealth;
        }

        private void Update()
        {
            if (health <= 0)
            {
                _enemyStateManager.SwitchToState("DeadState");
            }
        }
    }
}
