using System;
using ArenaShooter.Configs;

namespace ArenaShooter.Gameplay.Hero
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

            BulletDamage = _weaponConfig.Damage;
            WeaponCooldown = _weaponConfig.FireCooldown;
        }
        
        public void SetCurrentHealth(float health) => CurrentHealth = health;
        public void SetMaxHealth(float health) => MaxHealth = health;
        public void SetMoveSpeed(float moveSpeed) => MoveSpeed = moveSpeed;
        public void SetBulletDamage(float damage) => BulletDamage = damage;
        public void SetWeaponCooldown(float cooldown) => WeaponCooldown = cooldown;
    }
}