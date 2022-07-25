using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

namespace Player
{
    public class PlayerStatisticManager : MonoBehaviour
    {

        [SerializeField] protected float health = 100f;
        [SerializeField] protected float maxHealth = 100f;
        [SerializeField] protected float defaultSol = 0f;
        [SerializeField] protected float sol = 0f;
        [SerializeField] protected float energy;
        [SerializeField] protected float maxEnergy = 5f;

        private Vector3 _initPosition = new Vector3(0.0f, 0.0f, 0.0f);
        private int _playerCurrentLevel = 0;
        // after finished death statebmove all of PlayerDeathSequence to that
        private PlayerLoseSequence _playerLoseSequence;
        private PlayerSkillStateManager _playerSkillStateManager;
        [SerializeField] private GameUI.HUDController hudController;

        private void Start()
        {
            InitializeVariable();
        }
        protected void InitializeVariable()
        {
            this.RegisterListener(EventID.onSave, (param) => SavePlayerStatistic());
            _playerLoseSequence = GetComponent<PlayerLoseSequence>();
            _playerSkillStateManager = GetComponent<PlayerSkillStateManager>();
            _initPosition = transform.position;
            _playerCurrentLevel = PlayerPrefs.GetInt("CurrentScene", SceneManager.GetActiveScene().buildIndex);

            hudController.SetMaxHealth(maxHealth);
            hudController.SetSol(defaultSol);
            hudController.SetMaxEnergy(maxEnergy);

            LoadPlayerStatistic();
        }

        public void IncreaseSol(float p_amount)
        {
            sol += p_amount;
            hudController.SetSol(sol);
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

        IEnumerator PullFromSolCoroutine(float p_amount)
        {
            while (energy < maxEnergy && !_playerSkillStateManager.IsTimeSlowed())
            {
                yield return new WaitForSecondsRealtime(1);
                if (CanPullFromSol(p_amount))
                {
                    IncreaseEnergy(p_amount);
                }
            }
        }

        public void PullFromSol(float p_amount)
        {
            StartCoroutine(PullFromSolCoroutine(p_amount));
        }

        public void DecreaseHealth(float p_decreaseAmount)
        {
            health -= p_decreaseAmount;
            if (health <= 0)
            {
                _playerLoseSequence.PlayPlayerLoseSequence();
            }
            this.PostEvent(EventID.onHPChanged, health);
        }

        public void IncreaseHealth(float p_increaseAmount)
        {
            health += p_increaseAmount;
            health = Mathf.Clamp(health, 0f, maxHealth);
            this.PostEvent(EventID.onHPChanged, health);
        }

        public void DecreaseEnergy(float p_decreaseAmount)
        {
            energy -= p_decreaseAmount;
            energy = Mathf.Clamp(energy, 0f, maxEnergy);
            this.PostEvent(EventID.onEnergyChange, energy);
        }

        public void IncreaseEnergy(float p_increaseAmount)
        {
            energy += p_increaseAmount;
            energy = Mathf.Clamp(energy, 0f, maxEnergy);
            this.PostEvent(EventID.onEnergyChange, energy);
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

        public float GetEnergy()
        {
            return energy;
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
            //energy = playerData.energy;
            //hudController.SetEnergy(energy);

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
            energy = maxEnergy;
            transform.position = _initPosition;
            Physics.SyncTransforms();
        }
    }
}