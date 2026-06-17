using System;
using ArenaShooter.Infrastructure.Signals;
using UnityEngine;

namespace ArenaShooter.Infrastructure.StateMachine.States
{
    public class UpgradeSelectionState : IState
    {
        
        private readonly GameStateMachine _stateMachine;
        private readonly SignalBus _signalBus;
        private readonly Action<RequestGameplayStateSignal> _onGameplayRequestedCache;
        
        public UpgradeSelectionState(GameStateMachine stateMachine, SignalBus signalBus)
        {
            _stateMachine = stateMachine;
            _signalBus = signalBus;
            
            _onGameplayRequestedCache = OnGameplayRequested;
        }
        
        public void Enter()
        {
            Time.timeScale = 0f;
            _signalBus.Subscribe(_onGameplayRequestedCache);
            
            _signalBus.Fire(new ShowUpgradeWindowSignal());
        }

        public void Exit()
        {
            Time.timeScale = 1f;
            _signalBus.Unsubscribe(_onGameplayRequestedCache);
        }
        
        private void OnGameplayRequested(RequestGameplayStateSignal signal)
        {
            _stateMachine.TransitionTo<GameplayState>();
        }
    }
}