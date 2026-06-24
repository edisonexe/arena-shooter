using UnityEngine;

namespace ArenaShooter.Features.CombatVisuals
{
    public interface IDamageable
    {
        void TakeDamage(float amount, Vector3 damageSourcePosition);
    }
}