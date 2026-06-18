using System;
using ArenaShooter.Infrastructure.Signals;
using ArenaShooter.Services.Gameplay;
using UnityEngine;

namespace ArenaShooter.Infrastructure.StateMachine.States
{
    public class GameplayState : IState
    {
        private readonly GameStateMachine _stateMachine;
        private readonly SignalBus _signalBus;
        private readonly MatchDurationSystem _matchDurationSystem;
        
        private readonly Action<PlayerDiedSignal> _onPlayerDiedCache;
        private readonly Action<LevelUpSignal> _onLevelUpCache;
        private readonly Action _onTimeoutCache;
        
        public GameplayState(GameStateMachine stateMachine, SignalBus signalBus, MatchDurationSystem matchDurationSystem)
        {
            _stateMachine = stateMachine ?? throw new ArgumentNullException(nameof(stateMachine));
            _signalBus = signalBus ?? throw new ArgumentNullException(nameof(signalBus));
            _matchDurationSystem = matchDurationSystem ?? throw new ArgumentNullException(nameof(matchDurationSystem));
            
            _onPlayerDiedCache = OnPlayerDied;
            _onLevelUpCache = OnLevelUp;
            _onTimeoutCache = OnMatchTimeout;
        }

        public void Enter()
        {
            Time.timeScale = 1f;
            
            _matchDurationSystem.ResetTimer();
            _matchDurationSystem.StartTimer();
            
            _matchDurationSystem.OnTimeout += _onTimeoutCache;
            _signalBus.Subscribe(_onPlayerDiedCache);
            _signalBus.Subscribe(_onLevelUpCache);
        }

        public void Exit()
        {
            _matchDurationSystem.OnTimeout -= _onTimeoutCache;
            _matchDurationSystem.StopTimer();
            
            _signalBus.Unsubscribe(_onPlayerDiedCache);
            _signalBus.Unsubscribe(_onLevelUpCache);
        }

        private void OnPlayerDied(PlayerDiedSignal signal)
        {
            _stateMachine.TransitionTo<GameOverState>();
        }

        private void OnLevelUp(LevelUpSignal signal)
        {
            _stateMachine.TransitionTo<UpgradeSelectionState>();
        }
        
        private void OnMatchTimeout()
        {
            _stateMachine.TransitionTo<GameVictoryState>();
        }
    }
}