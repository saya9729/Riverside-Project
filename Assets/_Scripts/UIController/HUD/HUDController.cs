using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using UnityEngine.UI;

namespace GameUI
{
    public class HUDController : MonoBehaviour
    {
        [SerializeField] private TMPro.TextMeshProUGUI solCount;
        [SerializeField] private RawImage slowTimeIcon;
        [SerializeField] private Slider healthBar;
        [SerializeField] private Slider slowTimeBar;

        private void Start()
        {
            this.RegisterListener(EventID.onHPChanged, (param) => OnHealthChange((float)param));
            this.RegisterListener(EventID.onHPMaxChanged, (param) => OnMaxHealthChange((float)param));
            this.RegisterListener(EventID.onSolChange, (param) => OnSolChange((float)param));
            this.RegisterListener(EventID.onSlowTime, (param) => OnSlowTime((float)param));
            this.RegisterListener(EventID.onSlowTimeCoolDown, (param) => OnSlowTimeCoolDown((float)param));
            this.RegisterListener(EventID.onDashCooldown, (param) => OnDashCooldown((float)param));

            slowTimeBar.maxValue = 1;
            slowTimeBar.minValue = 0;
        }

        public void OnMaxHealthChange(float p_health)
        {
            healthBar.maxValue = p_health;
        }

        public void OnHealthChange(float p_health)
        {
            healthBar.value = p_health;
        }

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
            float timeElap  = 0;
            while (timeElap < p_slowTimeDuration)
            {
                slowTimeBar.value = Universal.Smoothing.LinearSmoothFixedTime(slowTimeBar.value, slowTimeBar.maxValue, slowTimeBar.minValue, Time.unscaledDeltaTime, p_slowTimeDuration);
                timeElap += Time.unscaledDeltaTime;
                yield return null;
            }
            slowTimeIcon.enabled = false;
        }

        public void OnSlowTimeCoolDown(float p_slowTimeCoolDownDuration)
        {
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
        }

        public void OnDashCooldown(float p_dashCooldownDuration)
        {
            StartCoroutine(HandleDashCoolDownBar(p_dashCooldownDuration));
        }

        private  IEnumerator HandleDashCoolDownBar(float p_dashCooldownDuration)
        {
            yield return null;
        }

    }
}


