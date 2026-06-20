using ArenaShooter.Configs.Upgrades;
using ArenaShooter.Gameplay.Hero;

namespace ArenaShooter.Services.Progression.UpgradesCalculation
{
    public class RegenHealthCalculation : IUpgradeCalculation
    {
        public UpgradeType TargetUpgradeType => UpgradeType.HealthRegenBoost;

        public void Apply(HeroRuntimeStats stats, float addFactor)
        {
            float healthRegenIncrement = stats.MaxHealth * addFactor;
            stats.SetHealthRegen(stats.HealthRegen + healthRegenIncrement);
        }
    }
}