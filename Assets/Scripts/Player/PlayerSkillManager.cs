using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Player
{
    public class PlayerSkillManager : MonoBehaviour
    {
        private PlayerStateManager _playerStateManager;
        [SerializeField] private float timeCoefficient = 0.1f;
        [SerializeField] private float timeDelay = 5;

        void Start()
        {
            _playerStateManager = GameObject.Find("PlayerStateManager").GetComponent<PlayerStateManager>();
        }

        IEnumerator Coroutine(float p_timeDelay)
        {
            yield return new WaitForSeconds(p_timeDelay);
            UnSlowTime();
        }
        public void SlowTime()
        {
            Time.timeScale = timeCoefficient;
            StartCoroutine(Coroutine(timeDelay * timeCoefficient));
        }

        public void UnSlowTime()
        {
            Time.timeScale = 1;
        }
    }
}