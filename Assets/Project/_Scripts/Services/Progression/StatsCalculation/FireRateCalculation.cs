using ArenaShooter.Configs.Upgrades;

namespace ArenaShooter.Services.Progression.StatsCalculation
{
    public class FireRateCalculation : IStatCalculation
    {
        public UpgradeType TargetUpgradeType => UpgradeType.FireRateBoost;

        public void CalculateAndApply(in StatCalculationContext context, float modifierValue)
        {
            context.RuntimeStats.SetWeaponCooldown(context.WeaponConfig.FireCooldown / modifierValue);
        }
    }
}