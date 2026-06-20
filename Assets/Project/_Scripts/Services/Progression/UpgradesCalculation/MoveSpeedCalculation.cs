using ArenaShooter.Configs.Upgrades;
using ArenaShooter.Gameplay.Hero;

namespace ArenaShooter.Services.Progression.UpgradesCalculation
{
    public class MoveSpeedCalculation : IUpgradeCalculation
    {
        public UpgradeType TargetUpgradeType => UpgradeType.MoveSpeedBoost;

        public void Apply(HeroRuntimeStats stats, float addFactor) 
            => stats.SetMoveSpeed(stats.MoveSpeed * (1f + addFactor));
    }
}