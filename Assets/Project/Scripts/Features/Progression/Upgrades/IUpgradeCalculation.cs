using ArenaShooter.Features.Hero.Components;

namespace ArenaShooter.Features.Progression.Upgrades
{
    public interface IUpgradeCalculation
    {
        UpgradeType TargetUpgradeType { get; }
        void Apply(HeroRuntimeStats stats, float addFactor);
    }
}