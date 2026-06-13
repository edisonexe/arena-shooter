using UnityEngine;

namespace ArenaShooter.Configs
{
    [CreateAssetMenu(fileName = "EnemyConfig", menuName = "Configs/EnemyConfig")]
    public class EnemyConfig : ScriptableObject
    {
        [SerializeField, Min(0.01f)] private float _moveSpeed = 3f;
        [SerializeField, Min(0.01f)] private float _maxHealth = 30f;
        [SerializeField, Min(0.01f)] private float _damage = 10f;

        public float MoveSpeed => _moveSpeed;
        public float MaxHealth => _maxHealth;
        public float Damage => _damage;
    }
}