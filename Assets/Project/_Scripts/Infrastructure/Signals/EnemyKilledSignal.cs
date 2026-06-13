using UnityEngine;

namespace ArenaShooter.Infrastructure.Signals
{
    public struct EnemyKilledSignal
    {
        public Vector3 DeathPosition;
        public int XpValue;

        public EnemyKilledSignal(Vector3 deathPosition, int xpValue)
        {
            DeathPosition = deathPosition;
            XpValue = xpValue;
        }
    }
}