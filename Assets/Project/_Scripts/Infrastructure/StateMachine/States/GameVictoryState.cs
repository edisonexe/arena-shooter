using System;
using ArenaShooter.Infrastructure.Signals;
using UnityEngine;

namespace ArenaShooter.Infrastructure.StateMachine.States
{
    public class GameVictoryState : IState
    {
        private readonly GameStateMachine _stateMachine;
        private readonly SignalBus _signalBus;

        public GameVictoryState(GameStateMachine stateMachine, SignalBus signalBus)
        {
            _stateMachine = stateMachine ?? throw new ArgumentNullException(nameof(stateMachine));
            _signalBus = signalBus ?? throw new ArgumentNullException(nameof(signalBus));
        }

        public void Enter()
        {
            Time.timeScale = 0f;
            //_signalBus.Fire(new GameVictorySignal());
        }

        public void Exit()
        {
            Time.timeScale = 1f;
        }
    }
}