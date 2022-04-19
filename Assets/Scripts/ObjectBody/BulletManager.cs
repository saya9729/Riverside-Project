using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class BulletManager : MonoBehaviour
    {
        private PlayerStatisticManager _playerStatisticManager;
        private InputManager _inputManager;

        private const int maxBullet = 6;
        private int curBullet;
        private float reloadTime = 2;
        private bool isReloading = false;

        void Start()
        {
            _playerStatisticManager = FindObjectOfType<PlayerStatisticManager>();
            _inputManager = FindObjectOfType<InputManager>();
            curBullet = maxBullet;
        }

        IEnumerator ReloadOnSol(float time, int bulletAmount)
        {
            isReloading = true;

            yield return new WaitForSecondsRealtime(time);

            if (_playerStatisticManager.CanPullFromSol(bulletAmount))
            {
                curBullet = maxBullet;
                Debug.Log("Fully Reloaded.");
            }
            else Debug.Log("Not enough Sols to reload.");

            isReloading = false;
        }

        void Update()
        {
            if (curBullet <= 0 && !isReloading)
            {
                StartCoroutine(ReloadOnSol(reloadTime, maxBullet));
            }
            if(_inputManager.pullFromSol  && !isReloading) 
            {
                StartCoroutine(ReloadOnSol(reloadTime, maxBullet - curBullet));
            }
        }
    }
}
