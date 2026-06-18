using UnityEngine;

namespace ArenaShooter.Infrastructure.Signals
{
    public readonly struct DamageTakenSignal
    {
        public readonly float Amount;
        public readonly Vector3 OriginPosition;
        
        public DamageTakenSignal(float amount, Vector3 originPosition)
        {
            Amount = amount;
            OriginPosition = originPosition;
        }
    }
}