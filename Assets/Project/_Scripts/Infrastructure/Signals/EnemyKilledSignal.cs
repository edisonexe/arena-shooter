using UnityEngine;

namespace ArenaShooter.Infrastructure.Signals
{
    public readonly struct EnemyKilledSignal
    {
        public readonly Vector3 DeathPosition;
        public readonly int XpValue;

        public EnemyKilledSignal(Vector3 deathPosition, int xpValue)
        {
            DeathPosition = deathPosition;
            XpValue = xpValue;
        }
    }
}