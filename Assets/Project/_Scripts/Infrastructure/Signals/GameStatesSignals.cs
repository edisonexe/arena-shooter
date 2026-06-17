namespace ArenaShooter.Infrastructure.Signals
{
    public class GameStatesSignals
    {
        public readonly struct PlayerDiedSignal { }
        public readonly struct LevelUpSignal { }
        
        public readonly struct RequestGameplayStateSignal { }
        public readonly struct ShowUpgradeWindowSignal { }
    }
}