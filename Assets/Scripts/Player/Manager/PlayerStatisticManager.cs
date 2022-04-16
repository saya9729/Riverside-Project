using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class PlayerStatisticManager : AbstractClass.StatisticManager
    {
        public float GetHealth()
        {
            return _health;
        }
    }
}