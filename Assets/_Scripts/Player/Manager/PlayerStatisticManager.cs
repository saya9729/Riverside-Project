using UnityEngine;
using UnityEngine.SceneManagement;

namespace Player
{
    public class PlayerStatisticManager : MonoBehaviour
    {

        [SerializeField] protected float health = 100f;
        [SerializeField] protected float maxHealth = 100f;
        [SerializeField] protected float defaultSol = 0f;
        [SerializeField] protected float sol = 0f;

        private Vector3 _initPosition = new Vector3(0.0f, 0.0f, 0.0f);
        private int _playerCurrentLevel = 0;
        // after finished death statebmove all of PlayerDeathSequence to that
        private PlayerLoseSequence _playerLoseSequence;
        private PlayerSkillManager _playerSkill;
        private PlayerMovementStateManager _playerMovementController;
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
            if (health <= 0)
            {
                _playerLoseSequence.PlayPlayerLoseSequence();
            }
        }

        protected void InitializeVariable()
        {
            _playerLoseSequence = GetComponent<PlayerLoseSequence>();
            _playerSkill = GetComponent<PlayerSkillManager>();
            _playerMovementController = GetComponent<PlayerMovementStateManager>();
            _initPosition = transform.position;
            _playerCurrentLevel = PlayerPrefs.GetInt("CurrentScene", SceneManager.GetActiveScene().buildIndex);

            hudController.SetMaxHealth(maxHealth);
            hudController.SetSol(defaultSol);
            hudController.SetMaxEnergy(_playerSkill.slowdownAmountMax);

            LoadPlayerStatistic();
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

        public float GetHealth()
        {
            return health;
        }

        public float GetSol()
        {
            return sol;
        }

        public void SavePlayerStatistic()
        {
            SaveManager.SavePlayer(this, _playerCurrentLevel);
        }

        public void LoadPlayerStatistic()
        {
            PlayerData playerData = SaveManager.LoadPlayer(_playerCurrentLevel);
            if (playerData == null)
            {
                Debug.Log("generate default value");
                RefreshPlayerStatistic();
                return;
            }
            health = playerData.health;
            hudController.SetHealth(health);
            sol = playerData.sol;
            hudController.SetSol(sol);
            Vector3 position;
            position.x = playerData.position[0];
            position.y = playerData.position[1];
            position.z = playerData.position[2];
            transform.position = position;
            Physics.SyncTransforms();
        }

        public void RefreshPlayerStatistic()
        {
            health = maxHealth;
            sol = defaultSol;
            transform.position = _initPosition;
            Physics.SyncTransforms();
        }
    }
}