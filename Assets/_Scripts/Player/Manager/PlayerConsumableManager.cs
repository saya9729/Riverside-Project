using UnityEngine;

namespace Player
{
    public class PlayerConsumableManager : MonoBehaviour
    {
        private PlayerActionStateManager _playerStateManager;
        [SerializeField] private float healthPoint;
        [SerializeField] private int countHPPot = 3;

        // Start is called before the first frame update
        void Start()
        {
            _playerStateManager = GetComponent<PlayerActionStateManager>();
            this.RegisterListener(EventID.onHPPotCollected, (param) => OnCollectHPPot());
        }

        void UseConsumable()
        {
            _playerStateManager.playerStatisticManager.IncreaseHealth(healthPoint);
            _playerStateManager.inputManager.useHealthPot = false;
            countHPPot -= 1;
        }

        void OnCollectHPPot()
        {
            if (countHPPot < 3)
                countHPPot++;
        }


        // Update is called once per frame
        void Update()
        {
            if (_playerStateManager.inputManager.useHealthPot && countHPPot != 0)
            {
                UseConsumable();
            }

        }
    }
}
