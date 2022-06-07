using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Enemy
{
    public class EnemyStatisticManager : MonoBehaviour
    {
        private EnemyStateManager _enemyStateManager;

        protected void InitializeVariable()
        {
            _enemyStateManager = GetComponent<EnemyStateManager>();
            health = maxHealth;
        }

        private void Update()
        {
            
        }
        [SerializeField] protected float health = 100f;
        [SerializeField] protected float maxHealth = 100;

        private void Start()
        {
            InitializeVariable();
        }


        public void DecreaseHealth(float p_decreaseAmount)
        {
            health -= p_decreaseAmount;
        }

        public void IncreaseHealth(float p_increaseAmount)
        {
            health += p_increaseAmount;
            if (health > maxHealth)
            {
                health = maxHealth;
            }            
        }

        public float HealthPercentage()
        {
            return health / maxHealth * 100;
        }
    }
}
