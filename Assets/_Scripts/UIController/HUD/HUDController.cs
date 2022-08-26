using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

namespace GameUI
{
    public class HUDController : MonoBehaviour
    {
        [SerializeField] private GameObject gameUICanvas;
        [SerializeField] private TMPro.TextMeshProUGUI solCount;
        [SerializeField] private Image slowTimeIcon;
        //[SerializeField] private Slider healthBar;
        [SerializeField] private Slider slowTimeBar;
        [SerializeField] private Slider keyCountSlider;
        [SerializeField] private Slider dashChargeSlider;
        [SerializeField] private Slider dashIconSlider;
        //[SerializeField] private Slider[] dashChargeCooldownBar;

        private int _dashCurrentCount;

        private void Start()
        {
            //this.RegisterListener(EventID.onHPChanged, (param) => OnHealthChange((float)param));
            //this.RegisterListener(EventID.onHPMaxChanged, (param) => OnMaxHealthChange((float)param));
            this.RegisterListener(EventID.onSolChange, (param) => OnSolChange((float)param));
            this.RegisterListener(EventID.onSlowTime, (param) => OnSlowTime((float)param));
            this.RegisterListener(EventID.onSlowTimeCoolDown, (param) => OnSlowTimeCoolDown((float)param));
            this.RegisterListener(EventID.onKeyCollected, (param) => OnKeyCollectedDisplay((int)param));
            this.RegisterListener(EventID.onDashChargeCooldown, (param) => OnDashChargeCooldown((float)param));
            this.RegisterListener(EventID.onDash, (param) => OnDash());
            this.RegisterListener(EventID.onToggleUI, (param) => OnToggleUI((bool)param));

            slowTimeBar.maxValue = 1;
            slowTimeBar.minValue = 0;
            keyCountSlider.value = 0;

            gameUICanvas.SetActive(Convert.ToBoolean(PlayerPrefs.GetInt(PlayerPrefEnum.HUD.ToString(), 1)));

            bool refresh = Convert.ToBoolean(PlayerPrefs.GetInt(PlayerPrefEnum.Refresh.ToString(), 0));
            if (refresh)
            {
                return;
            }
            EnvironmentData environmentData = SaveManager.LoadEnvironment(PlayerPrefs.GetInt(PlayerPrefEnum.CurrentScene.ToString(), SceneManager.GetActiveScene().buildIndex));
            for (int i = 0; i < environmentData.isKeyInteractableArray.Length; i++)
            {
                if (!environmentData.isKeyInteractableArray[i])
                {
                    OnKeyCollectedDisplay(1);
                }
            }
        }

        //public void OnMaxHealthChange(float p_health)
        //{
        //    healthBar.maxValue = p_health;
        //}

        //public void OnHealthChange(float p_health)
        //{
        //    healthBar.value = p_health;
        //}

        public void OnSolChange(float p_sol)
        {
            solCount.text = p_sol.ToString();
        }

        public void OnSlowTime(float p_slowTimeDuration)
        {
            slowTimeIcon.enabled = true;
            StartCoroutine(HandleSlowTimeBar(p_slowTimeDuration));
        }

        private IEnumerator HandleSlowTimeBar(float p_slowTimeDuration)
        {
            float timeElap = 0;
            while (timeElap < p_slowTimeDuration)
            {
                slowTimeBar.value = Universal.Smoothing.LinearSmoothFixedTime(slowTimeBar.value, slowTimeBar.maxValue, slowTimeBar.minValue, Time.unscaledDeltaTime, p_slowTimeDuration);
                timeElap += Time.unscaledDeltaTime;
                yield return null;
            }
        }

        public void OnSlowTimeCoolDown(float p_slowTimeCoolDownDuration)
        {
            slowTimeIcon.enabled = false;
            StartCoroutine(HandleSlowTimeBarCoolDown(p_slowTimeCoolDownDuration));
        }

        private IEnumerator HandleSlowTimeBarCoolDown(float p_slowTimeCoolDownDuration)
        {
            float timeElap = 0;
            while (timeElap < p_slowTimeCoolDownDuration)
            {
                slowTimeBar.value = Universal.Smoothing.LinearSmoothFixedTime(slowTimeBar.value, slowTimeBar.minValue, slowTimeBar.maxValue, Time.unscaledDeltaTime, p_slowTimeCoolDownDuration);
                timeElap += Time.unscaledDeltaTime;
                yield return null;
            }
            slowTimeIcon.enabled = true;
        }

        private void OnKeyCollectedDisplay(int p_count)
        {
            if (keyCountSlider.value <= 3)
            {
                keyCountSlider.value += p_count;
            }
        }

        private void OnDash()
        {
            if (dashChargeSlider.value > 0)
            {
                dashChargeSlider.value -= 1;
            }        
        }

        private void OnToggleUI(bool p_toggle)
        {
            gameUICanvas.SetActive(p_toggle);
        }

        public void OnDashChargeCooldown(float p_dashChargeCooldownDuration)
        {
            StartCoroutine(HandleDashChargeCoolDownSlider(p_dashChargeCooldownDuration));
        }

        private IEnumerator HandleDashChargeCoolDownSlider(float p_dashChargeCooldownDuration)
        {
            float timeElap = 0;
            while (timeElap < p_dashChargeCooldownDuration)
            {
                dashIconSlider.value = Universal.Smoothing.LinearSmoothFixedTime(dashIconSlider.value, 0, 1, Time.unscaledDeltaTime, p_dashChargeCooldownDuration);
                timeElap += Time.unscaledDeltaTime;
                yield return null;
            }
            dashChargeSlider.value += 1;
        }


    }
}


