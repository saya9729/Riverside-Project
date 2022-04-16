using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class PlayerStatisticManager : AbstractClass.StatisticManager
    {
        [SerializeField] protected float sol = 1000f;
        // after finished death statebmove all of PlayerDeathSequence to that
        private PlayerDeathSequence _playerDeathSequence;
        private void Start()
        {
            //sol = PlayerPrefs.GetFloat("Sol", 50f);
            health = 100f;
            _playerDeathSequence = GameObject.Find("Manager").GetComponent<PlayerDeathSequence>();
        }

        public bool CanPullFromSol(float p_amount)
        {
            sol -= p_amount;
            if (sol < 0)
            {
                sol += p_amount;
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
    }
}