using ArenaShooter.Configs.Upgrades;

namespace ArenaShooter.Services.Progression.StatsCalculation
{
    public class MoveSpeedCalculation : IStatCalculation
    {
        public UpgradeType TargetUpgradeType => UpgradeType.MoveSpeedBoost;

        public void CalculateAndApply(in StatCalculationContext context, float modifierValue)
        {
            context.RuntimeStats.SetMoveSpeed(context.HeroConfig.MoveSpeed * modifierValue);
        }
    }
}