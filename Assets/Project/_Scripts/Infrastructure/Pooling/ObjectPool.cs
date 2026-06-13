using System;
using System.Collections.Generic;
using UnityEngine;

namespace ArenaShooter.Infrastructure.Pooling
{
    public class ObjectPool<T> where T : MonoBehaviour, IPoolable<T>
    {
        private readonly T _prefab;
        private readonly Transform _parent;
        private readonly Queue<T> _pool = new();

        public ObjectPool(T prefab, Transform parent, int initialCapacity)
        {
            _prefab = prefab ?? throw new ArgumentNullException(nameof(prefab), "[ObjectPool] Prefab cannot be null!");
            _parent = parent ?? throw new ArgumentNullException(nameof(parent), "[ObjectPool] Parent Transform cannot be null!");

            Prewarm(initialCapacity);
        }

        public T Get()
        {
            T element = _pool.Count > 0 ? _pool.Dequeue() : UnityEngine.Object.Instantiate(_prefab, _parent);

            element.gameObject.SetActive(true);
            element.Spawn();
            return element;
        }

        public void Return(T element)
        {
            if (!element)
            {
                Debug.LogError("[ObjectPool] Attempted to return a null element to the pool.");
                return;
            }

            element.Despawn();
            element.gameObject.SetActive(false);
            _pool.Enqueue(element);
        }

        private void Prewarm(int capacity)
        {
            for (var i = 0; i < capacity; i++)
            {
                T element = UnityEngine.Object.Instantiate(_prefab, _parent);
                element.gameObject.SetActive(false);
                _pool.Enqueue(element);
            }
        }
    }
}