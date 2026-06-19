using ArenaShooter.Configs;
using ArenaShooter.Gameplay.Hero;

namespace ArenaShooter.Services.Progression.StatsCalculation
{
    public readonly struct StatCalculationContext
    {
        public HeroConfig HeroConfig { get; }
        public WeaponConfig WeaponConfig { get; }
        public HeroRuntimeStats RuntimeStats { get; }

        public StatCalculationContext(HeroConfig heroConfig, WeaponConfig weaponConfig, HeroRuntimeStats runtimeStats)
        {
            HeroConfig = heroConfig;
            WeaponConfig = weaponConfig;
            RuntimeStats = runtimeStats;
        }
    }
}