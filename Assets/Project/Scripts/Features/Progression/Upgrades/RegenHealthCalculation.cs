using ArenaShooter.Features.Hero.Components;

namespace ArenaShooter.Features.Progression.Upgrades
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