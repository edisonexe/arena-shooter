using System;
using ArenaShooter.Infrastructure.Signals;
using ArenaShooter.Services.Gameplay;
using ArenaShooter.Services.Input;
using UnityEngine;

namespace ArenaShooter.Infrastructure.StateMachine.States
{
    public class GameplayState : IState
    {
        private readonly GameStateMachine _stateMachine;
        private readonly SignalBus _signalBus;
        private readonly MatchDurationSystem _matchDurationSystem;
        private readonly IInputService _inputService;
        
        private readonly Action<PlayerDiedSignal> _onPlayerDiedCache;
        private readonly Action<LevelUpSignal> _onLevelUpCache;
        private readonly Action _onTimeoutCache;
        
        public GameplayState(
            GameStateMachine stateMachine, 
            SignalBus signalBus, 
            MatchDurationSystem matchDurationSystem,
            IInputService inputService)
        {
            _stateMachine = stateMachine ?? throw new ArgumentNullException(nameof(stateMachine));
            _signalBus = signalBus ?? throw new ArgumentNullException(nameof(signalBus));
            _matchDurationSystem = matchDurationSystem ?? throw new ArgumentNullException(nameof(matchDurationSystem));
            _inputService = inputService ?? throw new ArgumentNullException(nameof(inputService));
            
            _onPlayerDiedCache = OnPlayerDied;
            _onLevelUpCache = OnLevelUp;
            _onTimeoutCache = OnMatchTimeout;
        }

        public void Enter()
        {
            Time.timeScale = 1f;
            
            _inputService.Enable(); 
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            
            _matchDurationSystem.StartTimer();
            
            _matchDurationSystem.OnTimeout += _onTimeoutCache;
            _signalBus.Subscribe(_onPlayerDiedCache);
            _signalBus.Subscribe(_onLevelUpCache);
        }

        public void Exit()
        {
            _inputService.Disable();
            
            _matchDurationSystem.OnTimeout -= _onTimeoutCache;
            _matchDurationSystem.StopTimer();
            
            _signalBus.Unsubscribe(_onPlayerDiedCache);
            _signalBus.Unsubscribe(_onLevelUpCache);
        }

        private void OnPlayerDied(PlayerDiedSignal signal) => _stateMachine.TransitionTo<GameOverState>();
        private void OnLevelUp(LevelUpSignal signal) => _stateMachine.TransitionTo<UpgradeSelectionState>();
        private void OnMatchTimeout() => _stateMachine.TransitionTo<GameVictoryState>();
    }
}