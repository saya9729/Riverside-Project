using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AbstractClass
{
    public abstract class StatisticManager : MonoBehaviour
    {
        [SerializeField] protected float health = 100f;
        [SerializeField] protected float maxHealth = 100;

        private void Start()
        {
            InitializeVariable();
        }

        protected abstract void InitializeVariable();        

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
            this.PostEvent(EventID.onHPChanged, health);
        }

        public float HealthPercentage()
        {
            return health / maxHealth * 100;
        }

    }
}
