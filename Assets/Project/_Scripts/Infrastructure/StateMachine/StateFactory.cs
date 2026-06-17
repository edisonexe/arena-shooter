using System;
using Zenject;

namespace ArenaShooter.Infrastructure.StateMachine
{
    public class StateFactory
    {
        private readonly IInstantiator _instantiator;

        public StateFactory(IInstantiator instantiator)
        {
            _instantiator = instantiator ?? throw new ArgumentNullException(nameof(instantiator));
        }

        public TState Create<TState>() where TState : class, IState
        {
            return _instantiator.Instantiate<TState>();
        }
    }
}