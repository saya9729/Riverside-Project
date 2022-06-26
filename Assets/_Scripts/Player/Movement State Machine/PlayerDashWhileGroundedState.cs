namespace Player
{
    public class PlayerDashWhileGroundedState : AbstractClass.State
    {
        private PlayerMovementStateManager _playerMovementController;
        public override void EnterState()
        {
            _playerMovementController.ResetAirborneDirection();
            _playerMovementController.DisableJump();
            _playerMovementController.SetDashDirection();
            _playerMovementController.DisableInput();
            _playerMovementController.ResetInputDirection();
        }

        public override void ExitState()
        {
            _playerMovementController.EnableJump();
            _playerMovementController.EnableInput();
        }

        protected override void PhysicsUpdateThisState()
        {

        }

        protected override void CheckSwitchState()
        {
            if (!_playerMovementController.isInDashState)
            {
                _playerMovementController.SetRunSpeed();
                currentSuperState.currentSuperState.SwitchToState("Run");
            }
            else if (!_playerMovementController.isGrounded)
            {
                currentSuperState.SwitchToState("Airborne");
            }
        }

        protected override void InitializeComponent()
        {
            _playerMovementController = GetComponent<PlayerMovementStateManager>();
        }

        protected override void InitializeState()
        {

        }

        protected override void UpdateThisState()
        {
            CheckSwitchState();
        }

        public override void SwitchToState(string p_StateType)
        {
            throw new System.NotImplementedException();
        }

        protected override void InitializeVariable()
        {

        }
    }
}