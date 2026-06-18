using UnityEngine;

namespace ArenaShooter.Gameplay.Combat
{
    public interface IDamageable
    {
        void TakeDamage(float amount, Vector3 damageSourcePosition);
    }
}