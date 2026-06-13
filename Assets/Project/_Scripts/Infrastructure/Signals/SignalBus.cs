using System;
using System.Collections.Generic;

namespace ArenaShooter.Infrastructure.Signals
{
    public class SignalBus
    {
        private readonly Dictionary<Type, Delegate> _subscribers = new Dictionary<Type, Delegate>(16);

        public void Subscribe<TSignal>(Action<TSignal> callback) where TSignal : struct
        {
            Type type = typeof(TSignal);
            if (_subscribers.TryGetValue(type, out Delegate del))
            {
                _subscribers[type] = Delegate.Combine(del, callback);
            }
            else
            {
                _subscribers[type] = callback;
            }
        }

        public void Unsubscribe<TSignal>(Action<TSignal> callback) where TSignal : struct
        {
            Type type = typeof(TSignal);
            if (_subscribers.TryGetValue(type, out Delegate del))
            {
                Delegate currentDel = Delegate.Remove(del, callback);
                if (currentDel == null)
                {
                    _subscribers.Remove(type);
                }
                else
                {
                    _subscribers[type] = currentDel;
                }
            }
        }

        public void Fire<TSignal>(TSignal signal) where TSignal : struct
        {
            Type type = typeof(TSignal);
            if (_subscribers.TryGetValue(type, out Delegate del))
            {
                (del as Action<TSignal>)?.Invoke(signal);
            }
        }
    }
}