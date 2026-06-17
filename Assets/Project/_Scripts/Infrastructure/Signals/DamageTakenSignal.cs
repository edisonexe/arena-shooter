namespace ArenaShooter.Infrastructure.Signals
{
    public readonly struct DamageTakenSignal
    {
        public readonly float Amount;

        public DamageTakenSignal(float amount)
        {
            Amount = amount;
        }
    }
}