using ArenaShooter.Configs.Upgrades;
using ArenaShooter.Gameplay.Hero;

namespace ArenaShooter.Services.UpgradesCalculation
{
    public class PickupRadiusCalculation : IUpgradeCalculation
    {
        public UpgradeType TargetUpgradeType => UpgradeType.PickupRadiusBoost;

        public void Apply(HeroRuntimeStats stats, float addFactor) 
            => stats.SetPickupRadius(stats.PickupRadius * (1f + addFactor));
    }
}