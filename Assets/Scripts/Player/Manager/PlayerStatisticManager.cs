using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class PlayerStatisticManager : AbstractClass.StatisticManager
    {
        [SerializeField] protected float sol;
        private void Start()
        {
            sol = PlayerPrefs.GetFloat("Sol", 50f);
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
    }
}