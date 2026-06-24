using ArenaShooter.Features.Hero.Components;

namespace ArenaShooter.Features.Progression.Upgrades
{
    public class PickupRadiusCalculation : IUpgradeCalculation
    {
        public UpgradeType TargetUpgradeType => UpgradeType.PickupRadiusBoost;

        public void Apply(HeroRuntimeStats stats, float addFactor) 
            => stats.SetPickupRadius(stats.PickupRadius * (1f + addFactor));
    }
}