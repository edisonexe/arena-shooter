namespace ArenaShooter.Infrastructure.Signals
{
    public struct DamageTakenSignal
    {
        public float Amount;

        public DamageTakenSignal(float amount)
        {
            Amount = amount;
        }
    }
}