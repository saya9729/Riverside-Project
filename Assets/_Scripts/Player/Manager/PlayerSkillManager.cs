using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Player
{
    public class PlayerSkillManager : MonoBehaviour
    {
        private PlayerStateManager _playerStateManager;
        private float _fixedDeltaTimeOldValue;
        [SerializeField] private float timeCoefficient = 0.1f;
        [SerializeField] private float slowdownAmount;
        public float slowdownAmountMax = 5f;
        [SerializeField] private float timeAddToPrefixAndSuffixes = 0.5f;
        private float _timeAddToPrefixAndSuffixesCoefficient = 1f;
        private float _a1, _b1, _c1;
        private float _a2, _b2, _c2;
        [NonSerialized] public bool gameIsSlowDown = false;
        [SerializeField] private float amountPullFromSol = 1f;
        [SerializeField] private float amountPerSecond = 1f;

        void Start()
        {
            _playerStateManager = GetComponent<PlayerStateManager>();
            slowdownAmount = PlayerPrefs.GetFloat("SlowdownAmount", slowdownAmountMax);
            gameIsSlowDown = false;
            _a1 = timeCoefficient - 1f;
            _b1 = 0f - timeAddToPrefixAndSuffixes;
            _c1 = -(_a1 * 0f) - (_b1 * 1f);

            _a2 = -_a1;
            _b2 = _b1;
            _c2 = -(_a2 * 0f) - (_b2 * timeCoefficient);

        }

        IEnumerator StartOfSlowTimeCoroutine()
        {
            _playerStateManager.volume.enabled = true;
            _playerStateManager.slowTimeIcon.SetActive(true);
            int index = 1;
            while (_timeAddToPrefixAndSuffixesCoefficient != timeCoefficient && slowdownAmount != 0)
            {
                yield return new WaitForSeconds(timeAddToPrefixAndSuffixes / 10f * _timeAddToPrefixAndSuffixesCoefficient);
                StartOfSlowTime(index);
                index++;
            }
            Time.timeScale = timeCoefficient;
            _fixedDeltaTimeOldValue = Time.fixedDeltaTime;
            Time.fixedDeltaTime = Time.timeScale * 0.02f;

            _playerStateManager.volume.enabled = true;
            _playerStateManager.slowTimeIcon.SetActive(true);

            while (slowdownAmount != 0)
            {
                yield return new WaitForSecondsRealtime(1);
                slowdownAmount-= amountPerSecond;
                slowdownAmount = Mathf.Clamp(slowdownAmount, 0f, slowdownAmountMax);
                PlayerPrefs.SetFloat("SlowdownAmount", slowdownAmount);
                PlayerPrefs.Save();
                this.PostEvent(EventID.onEnergyChange, slowdownAmount);
            }
            UnSlowTime();

        }
        IEnumerator EndOfSlowTimeCoroutine()
        {
            int index = 1;
            while (_timeAddToPrefixAndSuffixesCoefficient != 1f && slowdownAmount != 0)
            {
                yield return new WaitForSeconds(timeAddToPrefixAndSuffixes / 10f * _timeAddToPrefixAndSuffixesCoefficient);
                EndOfSlowTime(index);
                index++;
            }

            Time.timeScale = 1;
            Time.fixedDeltaTime = _fixedDeltaTimeOldValue;
            _playerStateManager.volume.enabled = false;
            _playerStateManager.slowTimeIcon.SetActive(false);
            gameIsSlowDown = false;
            StopAllCoroutines();
            PullFromSol(amountPullFromSol);
        }
        private void StartOfSlowTime(int _index)
        {
            float x = 0f + (timeAddToPrefixAndSuffixes / 10f) * _index;
            Time.timeScale = (-_a1 * x - _c1)/_b1;
            Time.timeScale = Mathf.Clamp(Time.timeScale, timeCoefficient, 1f);
            _timeAddToPrefixAndSuffixesCoefficient = Time.timeScale;
        }
        private void EndOfSlowTime(int _index)
        {
            float x = 0f + (timeAddToPrefixAndSuffixes / 10f) * _index;
            Time.timeScale = (-_a2 * x - _c2) / _b2;
            Time.timeScale = Mathf.Clamp(Time.timeScale, timeCoefficient, 1f);
            _timeAddToPrefixAndSuffixesCoefficient = Time.timeScale;
        }
        public void SlowTime()
        {
            StartCoroutine(StartOfSlowTimeCoroutine());
           
        }
        public void UnSlowTime()
        {
            
            StartCoroutine(EndOfSlowTimeCoroutine());
        }

        public void ToggleSlowGame(bool p_toggle)
        {
            if (p_toggle)
            {
                SlowTime();
            }
            else if(!p_toggle) { UnSlowTime(); }
        }
        IEnumerator PullFromSolCoroutine(float p_amount)
        {
            while (slowdownAmount < slowdownAmountMax && gameIsSlowDown==false)
            {
                yield return new WaitForSecondsRealtime(1);
                if (_playerStateManager.playerStatisticManager.CanPullFromSol(p_amount))
                {
                    slowdownAmount += p_amount;
                    PlayerPrefs.SetFloat("SlowdownAmount", slowdownAmount);
                    PlayerPrefs.Save();
                    this.PostEvent(EventID.onEnergyChange, slowdownAmount);
                }
            }
        }

        public void PullFromSol(float p_amount)
        {
            StartCoroutine(PullFromSolCoroutine(p_amount));
        }
    }
}