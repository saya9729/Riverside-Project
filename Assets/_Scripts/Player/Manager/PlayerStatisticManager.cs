using UnityEngine;
using UnityEngine.SceneManagement;
using System;

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

        private void Start()
        {
            InitializeVariable();
        }
        protected void InitializeVariable()
        {
            this.RegisterListener(EventID.onSave, (param) => SavePlayerStatistic());
            this.RegisterListener(EventID.onRefresh, (param) => RefreshPlayerStatistic());
            _initPosition = transform.position;
            _playerCurrentLevel = PlayerPrefs.GetInt(PlayerPrefEnum.CurrentScene.ToString(), SceneManager.GetActiveScene().buildIndex);

            this.PostEvent(EventID.onHPMaxChanged, maxHealth);

            LoadPlayerStatistic();
        }

        public void IncreaseSol(float p_amount)
        {
            sol += p_amount;
            this.PostEvent(EventID.onSolChange, sol);
        }

        public void DecreaseSol(float p_amount)
        {
            sol -= p_amount;
            sol = Mathf.Clamp(sol, 0f, float.MaxValue);
            this.PostEvent(EventID.onSolChange, sol);
        }

        public bool CanPullFromSol(float p_amount)
        {
            if (sol < p_amount)
            {
                return false;
            }
            DecreaseSol(p_amount);
            return true;
        }

        public void DecreaseHealth(float p_decreaseAmount)
        {
            health -= p_decreaseAmount;
            health = Mathf.Clamp(health, 0f, maxHealth);
            this.PostEvent(EventID.onHPChanged, health);

            // after finished death state move this.PostEvent(EventID.onLose) to that
            if (health == 0)
            {
                this.PostEvent(EventID.onLose);
            }
        }

        public void IncreaseHealth(float p_increaseAmount)
        {
            health += p_increaseAmount;
            health = Mathf.Clamp(health, 0f, maxHealth);
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

            bool refresh = Convert.ToBoolean(PlayerPrefs.GetInt(PlayerPrefEnum.Refresh.ToString(), 0));

            if (playerData == null || refresh)
            {
                Debug.Log("generate default value");
                RefreshPlayerStatistic();
                return;
            }

            health = playerData.health;
            sol = playerData.sol;

            Vector3 position = new Vector3(playerData.position[0], playerData.position[1], playerData.position[2]);
            transform.position = position;

            RefreshUI();
        }

        public void RefreshPlayerStatistic()
        {
            health = maxHealth;
            sol = defaultSol;
            transform.position = _initPosition;

            RefreshUI();
            SavePlayerStatistic();
        }

        private void RefreshUI()
        {
            
            this.PostEvent(EventID.onHPChanged, health);
            this.PostEvent(EventID.onSolChange, sol);
            Physics.SyncTransforms();
        }
    }
}