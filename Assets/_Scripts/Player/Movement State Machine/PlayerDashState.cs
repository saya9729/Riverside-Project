using UnityEngine;
namespace Player
{
    [RequireComponent(typeof(PlayerDashWhileGroundedState))]
    [RequireComponent(typeof(PlayerDashWhileAirborneState))]
    public class PlayerDashState : AbstractClass.State
    {
        private PlayerMovementStateManager _playerMovementController;

        private PlayerDashWhileGroundedState _playerDashWhileGroundedState;
        private PlayerDashWhileAirborneState _playerDashWhileAirborneState;
        public override void EnterState()
        {
            _playerMovementController.StartCoroutineDashState();
            _playerMovementController.StartCoroutineChangeFOVWhileDash();
            _playerMovementController.EnableAttackHitbox();
            _playerMovementController.EnablePhaseThroughEnemy();

            if (_playerMovementController.isGrounded)
            {
                SwitchToState("Grounded");
            }
            else
            {
                SwitchToState("Airborne");
            }
        }

        public override void ExitState()
        {
            _playerMovementController.StarCoroutineRevertFOVAfterDash();
            _playerMovementController.DisableAttackHitbox();
            _playerMovementController.DisablePhaseThroughEnemy();
        }

        public override void SwitchToState(string p_stateType)
        {
            switch (p_stateType)
            {
                case "Grounded":
                    SetSubState(_playerDashWhileGroundedState);
                    break;
                case "Airborne":
                    SetSubState(_playerDashWhileAirborneState);
                    break;
                default:
                    SetSubState(null);
                    break;
            }
        }

        protected override void CheckSwitchState()
        {

        }

        protected override void InitializeComponent()
        {
            _playerMovementController = GetComponent<PlayerMovementStateManager>();
        }

        protected override void InitializeState()
        {
            _playerDashWhileGroundedState = GetComponent<PlayerDashWhileGroundedState>();
            _playerDashWhileAirborneState = GetComponent<PlayerDashWhileAirborneState>();

            _playerDashWhileGroundedState.SetSuperState(this);
            _playerDashWhileAirborneState.SetSuperState(this);
        }

        protected override void InitializeVariable()
        {

        }

        protected override void PhysicsUpdateThisState()
        {

        }

        protected override void UpdateThisState()
        {
            CheckSwitchState();
        }
    }
}