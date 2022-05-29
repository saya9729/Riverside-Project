using UnityEngine;

namespace Player
{
    public class PlayerStatisticManager : AbstractClass.StatisticManager
    {
        [SerializeField] protected float sol = 1000f;
        // after finished death statebmove all of PlayerDeathSequence to that
        private PlayerDeathSequence _playerDeathSequence;
        private PlayerSkillManager playerSkill;
        [SerializeField] private GameUI.HUDController hudController;


        private void Start()
        {
            //sol = PlayerPrefs.GetFloat("Sol", 50f);
            health = 100f;
            _playerDeathSequence = GetComponent<PlayerDeathSequence>();
            playerSkill = GetComponent<PlayerSkillManager>();
            hudController.SetMaxHealth(health);
            hudController.SetSol(sol);
            hudController.SetMaxEnergy(playerSkill.slowdownAmountMax);
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
            PlayerPrefs.SetFloat("Sol", sol);
            PlayerPrefs.Save();
            return true;
        }

        private void Update()
        {
            if (health <= 0)
            {
                _playerDeathSequence.PlayPlayerDeathSequence();
            }
        }

        protected override void InitializeVariable()
        {
            health = maxHealth;
        }
    }
}