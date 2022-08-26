namespace Player
{
    public class PlayerDashWhileGroundedState : AbstractClass.State
    {
        private PlayerMovementStateManager _playerMovementController;
        public override void EnterState()
        {
            _playerMovementController.SetDashDirection();
        }

        public override void ExitState()
        {

        }

        protected override void PhysicsUpdateThisState()
        {
            _playerMovementController.MoveWhileDash();
        }

        protected override void CheckSwitchState()
        {
            if (!_playerMovementController.isInDashState)
            {
                currentSuperState.currentSuperState.SwitchToState("Run");
            }
            else if (!_playerMovementController.isGrounded)
            {
                _playerMovementController.EnableDoubleJump();
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