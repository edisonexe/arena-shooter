using ArenaShooter.Configs.Upgrades;

namespace ArenaShooter.Services.Progression.StatsCalculation
{
    public class DamageCalculation : IStatCalculation
    {
        public UpgradeType TargetUpgradeType => UpgradeType.DamageBoost;

        public void CalculateAndApply(in StatCalculationContext context, float modifierValue)
        {
            context.RuntimeStats.SetBulletDamage(context.WeaponConfig.Damage * modifierValue);
        }
    }
}