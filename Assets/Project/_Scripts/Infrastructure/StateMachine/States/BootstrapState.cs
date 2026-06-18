using System;
using ArenaShooter.Services.Gameplay;

namespace ArenaShooter.Infrastructure.StateMachine.States
{
    public class BootstrapState : IState
    {
        private readonly GameNavigationService _navigationService;

        public BootstrapState(GameNavigationService navigationService)
        {
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
        }

        public void Enter()
        {
            _navigationService.ToMainMenu();
        }

        public void Exit()
        {
        }
    }
}