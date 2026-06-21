using ArenaShooter.Configs.Upgrades;
using ArenaShooter.Gameplay.Hero;

namespace ArenaShooter.Services.UpgradesCalculation
{
    public interface IUpgradeCalculation
    {
        UpgradeType TargetUpgradeType { get; }
        void Apply(HeroRuntimeStats stats, float addFactor);
    }
}