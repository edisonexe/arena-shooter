using ArenaShooter.Configs.Upgrades;

namespace ArenaShooter.Services.Progression.StatsCalculation
{
    public class PickupRadiusCalculation : IStatCalculation
    {
        public UpgradeType TargetUpgradeType => UpgradeType.PickupRadiusBoost;

        public void CalculateAndApply(in StatCalculationContext context, float modifierValue)
        {
            context.RuntimeStats.SetPickupRadius(context.HeroConfig.BasePickupRadius * modifierValue);
        }
    }
}