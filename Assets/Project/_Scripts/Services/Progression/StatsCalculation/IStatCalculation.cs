using ArenaShooter.Configs.Upgrades;

namespace ArenaShooter.Services.Progression.StatsCalculation
{
    public interface IStatCalculation
    {
        UpgradeType TargetUpgradeType { get; }
        void CalculateAndApply(in StatCalculationContext context, float modifierValue);
    }
}