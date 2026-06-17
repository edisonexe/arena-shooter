namespace ArenaShooter.Infrastructure.StateMachine.States
{
    public class BootstrapState : IState
    {
        private readonly GameStateMachine _stateMachine;

        public BootstrapState(GameStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public void Enter()
        {
            _stateMachine.TransitionTo<GameplayState>();
        }

        public void Exit() { }
    }
}