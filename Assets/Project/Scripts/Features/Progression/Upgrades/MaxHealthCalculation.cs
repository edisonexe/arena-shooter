using ArenaShooter.Features.Hero.Components;

namespace ArenaShooter.Features.Progression.Upgrades
{
    public class MaxHealthCalculation : IUpgradeCalculation
    {
       public UpgradeType TargetUpgradeType => UpgradeType.MaxHealthBoost;
        
       public void Apply(HeroRuntimeStats stats, float addFactor)
       {
           float oldMax = stats.MaxHealth;
           float newMax = oldMax * (1f + addFactor);
           
           float healthDeficit = oldMax - stats.CurrentHealth;

           if (oldMax > 0f)
           {
               float growthRatio = newMax / oldMax;
               stats.SetHealthRegen(stats.HealthRegen * growthRatio);
           }
           
           float targetCurrentHealth = newMax - healthDeficit;

           stats.SetCurrentHealth(targetCurrentHealth);
           stats.SetMaxHealth(newMax);
       }
    }
}