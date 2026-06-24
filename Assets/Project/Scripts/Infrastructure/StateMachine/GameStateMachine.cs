using System;
using System.Collections.Generic;
using ArenaShooter.Infrastructure.StateMachine.States;
using Zenject;

namespace ArenaShooter.Infrastructure.StateMachine
{
    public class GameStateMachine : IInitializable
    {
        private readonly StateFactory _factory;
        private readonly Dictionary<Type, IState> _activeStatesCache = new(8);
        
        public IState CurrentState { get; private set; }
        public event Action<IState> OnStateChanged;

        public GameStateMachine(StateFactory factory)
        {
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));
        }
        
        public void Initialize()
        {
            TransitionTo<BootstrapState>();
        }

        public void TransitionTo<TState>() where TState : class, IState
        {
            CurrentState?.Exit();

            if (!_activeStatesCache.TryGetValue(typeof(TState), out var state))
            {
                state = _factory.Create<TState>();
                _activeStatesCache[typeof(TState)] = state;
            }

            CurrentState = state;
            CurrentState.Enter();
            
            OnStateChanged?.Invoke(CurrentState);
        }
    }
}