using System;
using ArenaShooter.Features.Hero.Configs;
using ArenaShooter.Features.Weapons.Configs;

namespace ArenaShooter.Features.Hero.Components
{
    public class HeroRuntimeStats
    {
        private readonly HeroConfig _heroConfig;
        private readonly WeaponConfig _weaponConfig;
        
        public float MoveSpeed { get; private set; }
        public float BulletDamage { get; private set; }
        public float WeaponCooldown { get; private set; }
        
        public float MaxHealth { get; private set; }
        public float CurrentHealth { get; private set; }
        public float HealthRegen { get; private set; } 
        
        public float PickupRadius { get; private set; }
        
        public event Action<float, float> OnHealthStatsChanged;
        
        public HeroRuntimeStats(HeroConfig heroConfig, WeaponConfig weaponConfig)
        {
            _heroConfig = heroConfig ?? throw new ArgumentNullException(nameof(heroConfig));
            _weaponConfig = weaponConfig ?? throw new ArgumentNullException(nameof(weaponConfig));
            
            ResetToConfigs();
        }
        
        public void ResetToConfigs()
        {
            MoveSpeed = _heroConfig.MoveSpeed;
            MaxHealth = _heroConfig.MaxHealth;
            CurrentHealth = MaxHealth;
            HealthRegen = _heroConfig.BaseHealthRegen;
            
            BulletDamage = _weaponConfig.Damage;
            WeaponCooldown = _weaponConfig.FireCooldown;
            
            PickupRadius = _heroConfig.BasePickupRadius;
            
            OnHealthStatsChanged?.Invoke(CurrentHealth, MaxHealth);
        }
        
        public void SetCurrentHealth(float health)
        {
            CurrentHealth = health;
            OnHealthStatsChanged?.Invoke(CurrentHealth, MaxHealth);
        }

        public void SetMaxHealth(float health)
        {
            MaxHealth = health;
            OnHealthStatsChanged?.Invoke(CurrentHealth, MaxHealth);
        }

        public void SetHealthRegen(float regen)
        {
            HealthRegen = regen;
        }

        public void SetMoveSpeed(float moveSpeed)
        {
            MoveSpeed = moveSpeed;
        }

        public void SetBulletDamage(float damage)
        {
            BulletDamage = damage;
        }

        public void SetWeaponCooldown(float cooldown)
        {
            WeaponCooldown = cooldown;
        }

        public void SetPickupRadius(float radius)
        {
            PickupRadius = radius;
        }
    }
}