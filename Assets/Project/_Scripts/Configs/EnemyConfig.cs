using ArenaShooter.Gameplay.Enemies;
using UnityEngine;

namespace ArenaShooter.Configs
{
    [CreateAssetMenu(fileName = "EnemyConfig", menuName = "Configs/Enemies/EnemyConfig")]
    public class EnemyConfig : ScriptableObject
    {
        [Header("Visual Prefab")]
        [SerializeField] private EnemyView _prefab;
        
        [SerializeField, Min(0.01f)] private float _moveSpeed = 3f;
        [SerializeField, Min(0.01f)] private float _maxHealth = 30f;
        [SerializeField, Min(0.01f)] private float _damage = 10f;
        [SerializeField, Min(0)] private int _xpValue = 10;
        
        public EnemyView Prefab => _prefab;
        public float MoveSpeed => _moveSpeed;
        public float MaxHealth => _maxHealth;
        public float Damage => _damage;
        public int XpValue => _xpValue;
    }
}