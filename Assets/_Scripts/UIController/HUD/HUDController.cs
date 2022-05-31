using UnityEngine;
using UnityEngine.UI;

namespace GameUI
{
    public class HUDController : MonoBehaviour
    {
        [SerializeField] private TMPro.TextMeshProUGUI solCount;
        [SerializeField] private Slider healthBar;
        [SerializeField] private Slider energyBar;

        private void Start()
        {
            this.RegisterListener(EventID.onHPChanged, (param) => SetHealth((float)param));
            this.RegisterListener(EventID.onEnergyChange, (param) => SetEnergy((float)param));
        }

        public void SetMaxHealth(float p_health)
        {
            healthBar.maxValue = p_health;
            healthBar.value = p_health;
        }

        public void SetHealth(float p_health)
        {
            healthBar.value = p_health;
        }

        public void SetMaxEnergy(float p_energy)
        {
            energyBar.maxValue = p_energy;
            energyBar.value = p_energy;
        }

        public void SetEnergy(float p_energy)
        {
            energyBar.value = p_energy;
        }

        public void SetSol(float p_sol)
        {
            solCount.text = p_sol.ToString();
        }

    }
}


