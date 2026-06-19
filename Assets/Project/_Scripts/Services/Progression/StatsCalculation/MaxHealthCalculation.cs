using ArenaShooter.Configs.Upgrades;

namespace ArenaShooter.Services.Progression.StatsCalculation
{
    public class MaxHealthCalculation : IStatCalculation
    {
        public UpgradeType TargetUpgradeType => UpgradeType.MaxHealthBoost;

        public void CalculateAndApply(in StatCalculationContext context, float modifierValue)
        {
            float finalMaxHealth = context.HeroConfig.MaxHealth * modifierValue;
            var stats = context.RuntimeStats;
            
            if (finalMaxHealth > stats.MaxHealth)
            {
                float healthDifference = finalMaxHealth - stats.MaxHealth;
                stats.SetCurrentHealth(stats.CurrentHealth + healthDifference);
            }
            
            stats.SetMaxHealth(finalMaxHealth);
        }
    }
}