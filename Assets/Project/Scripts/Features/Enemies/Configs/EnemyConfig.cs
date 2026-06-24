using ArenaShooter.Features.Enemies.Components;
using UnityEngine;

namespace ArenaShooter.Features.Enemies.Configs
{
    [CreateAssetMenu(fileName = "EnemyConfig", menuName = "Configs/Enemies/EnemyConfig")]
    public class EnemyConfig : ScriptableObject
    {
        [Header("Visual Prefab")]
        [SerializeField] private EnemyView _prefab;
        
        [Header("Enemy Stats")]
        [SerializeField, Min(0.01f)] private float _moveSpeed = 3f;
        [SerializeField, Min(0.01f)] private float _maxHealth = 30f;
        [SerializeField, Min(0.01f)] private float _damage = 10f;
        [SerializeField,Min(0.01f)] private float _attackRadius = 1.2f;
        [SerializeField, Min(0)] private int _xpValue = 10;
        
        [Header("Spatial System Settings")]
        [SerializeField, Min(0.01f)] private float _separationRadius = 1f;
        
        public EnemyView Prefab => _prefab;
        public float MoveSpeed => _moveSpeed;
        public float MaxHealth => _maxHealth;
        public float Damage => _damage;
        public int XpValue => _xpValue;
        public float SeparationRadius => _separationRadius;
        public float AttackRadiusSqr => _attackRadius * _attackRadius;
    }
}