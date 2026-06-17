using ArenaShooter.Infrastructure.Pooling;
using UnityEngine;

namespace ArenaShooter.Gameplay.Items
{
    public class XPGem : MonoBehaviour, IPoolable<XPGem>
    {
        private int _xpValue;
        private bool _isActive;
        
        public int XpValue => _xpValue;
        public bool IsActive => _isActive;

        public void Initialize(Vector3 position, int xpValue)
        {
            transform.position = position;
            _xpValue = xpValue;
        }

        public void Collect() => _isActive = false;

        public void Spawn() => _isActive = true;

        public void Despawn() { }
    }
}