using ArenaShooter.Features.Hero.Components;

namespace ArenaShooter.Features.Progression.Upgrades
{
    public class MoveSpeedCalculation : IUpgradeCalculation
    {
        public UpgradeType TargetUpgradeType => UpgradeType.MoveSpeedBoost;

        public void Apply(HeroRuntimeStats stats, float addFactor) 
            => stats.SetMoveSpeed(stats.MoveSpeed * (1f + addFactor));
    }
}