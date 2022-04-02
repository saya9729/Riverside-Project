using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AbstractClass
{
    public abstract class StatisticManager : MonoBehaviour
    {
        protected float _health;
        [SerializeField] protected float maxHealth = 100;
        protected float _movementSpeed;
        [SerializeField] protected float _normalMovementSpeed = 100;

        private void Start()
        {
            InitializeVariable();
        }

        protected void InitializeVariable()
        {
            _health = maxHealth;
            _movementSpeed = _normalMovementSpeed;
        }

        public void DecreaseHealth(float p_decreaseAmount)
        {
            _health -= p_decreaseAmount;
        }

        public void IncreaseHealth(float p_increaseAmount)
        {
            _health += p_increaseAmount;
            if (_health > maxHealth)
            {
                _health = maxHealth;
            }
        }

        public float HealthPercentage()
        {
            return _health / maxHealth * 100;
        }

        public void ChangeMovementSpeed(float p_changeMultiplier)
        {
            _movementSpeed = _normalMovementSpeed * p_changeMultiplier;
        }
    }
}
