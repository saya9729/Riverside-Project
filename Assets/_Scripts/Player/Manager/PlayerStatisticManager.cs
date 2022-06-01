using UnityEngine;

namespace Player
{
    public class PlayerStatisticManager : AbstractClass.StatisticManager
    {
        [SerializeField] protected float sol = 1000f;
        // after finished death statebmove all of PlayerDeathSequence to that
        private PlayerLoseSequence _playerLoseSequence;
        private PlayerSkillManager _playerSkill;
        private PlayerMovementController _playerMovementController;
        [SerializeField] private GameUI.HUDController hudController;


        private void Start()
        {
            health = 100f;
            _playerLoseSequence = GetComponent<PlayerLoseSequence>();
            _playerSkill = GetComponent<PlayerSkillManager>();
            _playerMovementController = GetComponentInParent<PlayerMovementController>();
            hudController.SetMaxHealth(health);
            hudController.SetSol(sol);
            hudController.SetMaxEnergy(_playerSkill.slowdownAmountMax);
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

        protected override void InitializeVariable()
        {
            health = maxHealth;
        }
    }
}