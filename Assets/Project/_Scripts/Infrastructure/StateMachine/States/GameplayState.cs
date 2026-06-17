using System;
using ArenaShooter.Infrastructure.Signals;
using UnityEngine;

namespace ArenaShooter.Infrastructure.StateMachine.States
{
    public class GameplayState : IState
    {
        private readonly GameStateMachine _stateMachine;
        private readonly SignalBus _signalBus;

        private readonly Action<PlayerDiedSignal> _onPlayerDiedCache;
        private readonly Action<LevelUpSignal> _onLevelUpCache;
        
        public GameplayState(GameStateMachine stateMachine, SignalBus signalBus)
        {
            _stateMachine = stateMachine;
            _signalBus = signalBus;
            
            _onPlayerDiedCache = OnPlayerDied;
            _onLevelUpCache = OnLevelUp;
        }

        public void Enter()
        {
            Time.timeScale = 1f;
            _signalBus.Subscribe(_onPlayerDiedCache);
            _signalBus.Subscribe(_onLevelUpCache);
        }

        public void Exit()
        {
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
    }
}