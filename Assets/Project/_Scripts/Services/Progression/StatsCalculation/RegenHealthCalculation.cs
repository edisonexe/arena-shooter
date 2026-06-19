using ArenaShooter.Configs.Upgrades;

namespace ArenaShooter.Services.Progression.StatsCalculation
{
    public class RegenHealthCalculation : IStatCalculation
    {
        public UpgradeType TargetUpgradeType => UpgradeType.HealthRegenBoost;

        public void CalculateAndApply(in StatCalculationContext context, float modifierValue)
        {
            context.RuntimeStats.SetHealthRegen(context.HeroConfig.BaseHealthRegen * modifierValue);
        }
    }
}