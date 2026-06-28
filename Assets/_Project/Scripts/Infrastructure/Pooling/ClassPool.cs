using System;
using System.Collections.Generic;

namespace ArenaShooter.Infrastructure.Pooling
{
    public class ClassPool<T> where T : class, IPoolable
    {
        private readonly Queue<T> _pool;
        private readonly Func<T> _factoryMethod;

        public ClassPool(Func<T> factoryMethod, int initialCapacity = 0)
        {
            _factoryMethod = factoryMethod ?? throw new ArgumentNullException(nameof(factoryMethod));
            _pool = new Queue<T>(initialCapacity);

            for (int i = 0; i < initialCapacity; i++)
            {
                T instance = _factoryMethod();
                instance.Despawn();
                _pool.Enqueue(instance);
            }
        }

        public T Get()
        {
            T instance = _pool.Count > 0 ? _pool.Dequeue() : _factoryMethod();
            instance.Spawn();
            return instance;
        }

        public void Return(T instance)
        {
            if (instance == null) return;
            
            instance.Despawn();
            _pool.Enqueue(instance);
        }
    }
}