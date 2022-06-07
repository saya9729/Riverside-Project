using UnityEngine;

namespace Player
{
    public class PlayerStatisticManager : MonoBehaviour
    {

        [SerializeField] protected float health = 100f;
        [SerializeField] protected float maxHealth = 100f;
        [SerializeField] protected float sol = 1000f;
        // after finished death statebmove all of PlayerDeathSequence to that
        private PlayerLoseSequence _playerLoseSequence;
        private PlayerSkillManager _playerSkill;
        private PlayerMovementController _playerMovementController;
        [SerializeField] private GameUI.HUDController hudController;

        private void Start()
        {
            InitializeVariable();
        }

        public bool CanPullFromSol(float p_amount)
        {
            sol -= p_amount;
            hudController.SetSol(sol);
            if (sol < 0)
            {
                sol += p_amount;
                hudController.SetSol(sol);
                return false;
            }
            return true;
        }

        private void Update()
        {
            if (!_playerMovementController.isGrounded)
            {
                if (Mathf.Abs(_playerMovementController.verticalVelocity) >= Mathf.Abs(_playerMovementController.terminalVelocity))
                {
                    health = 0f;
                }
            }
            if (health <= 0)
            {
                _playerLoseSequence.PlayPlayerLoseSequence();
            }
        }

        protected void InitializeVariable()
        {
            health = maxHealth;
            _playerLoseSequence = GetComponent<PlayerLoseSequence>();
            _playerSkill = GetComponent<PlayerSkillManager>();
            _playerMovementController = GetComponent<PlayerMovementController>();
            hudController.SetMaxHealth(health);
            hudController.SetSol(sol);
            hudController.SetMaxEnergy(_playerSkill.slowdownAmountMax);
        }

        public void IncreaseSol(float p_amount)
        {
            sol += p_amount;
            hudController.SetSol(sol);
        }

        public void DecreaseHealth(float p_decreaseAmount)
        {
            health -= p_decreaseAmount;
            this.PostEvent(EventID.onHPChanged, health);
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