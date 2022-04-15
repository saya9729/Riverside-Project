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
        protected float _sol;
        [SerializeField] protected float maxSol = 100;

        private void Start()
        {
            InitializeVariable();
        }

        protected void InitializeVariable()
        {
            _health = maxHealth;
            _movementSpeed = _normalMovementSpeed;
            _sol = maxSol;
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

        public bool CanPullFromSol(float p_amount)
        {
            _sol -= p_amount;
            if (_sol < 0)
            {
                _sol += p_amount;
                return false;
            }
            return true;
        }
    }
}
