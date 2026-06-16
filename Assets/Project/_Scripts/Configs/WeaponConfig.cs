using UnityEngine;

namespace ArenaShooter.Configs
{
    [CreateAssetMenu(fileName = "WeaponConfig", menuName = "Configs/WeaponConfig")]
    public class WeaponConfig : ScriptableObject
    {
        [Header("Combat Settings")]
        [SerializeField, Min(1f)] private float _damage = 15f;
        [SerializeField, Min(0.05f)] private float _fireCooldown = 0.5f;
        [SerializeField, Min(1f)] private float _fireRadius = 15f;

        [Header("Projectile Settings")]
        [SerializeField, Min(1f)] private float _bulletSpeed = 20f;

        public float Damage => _damage;
        public float FireCooldown => _fireCooldown;
        public float FireRadius => _fireRadius;
        public float BulletSpeed => _bulletSpeed;
        
    }
}