using UnityEngine;

namespace ArenaShooter.Configs
{
    [CreateAssetMenu(fileName = "HeroConfig", menuName = "Configs/HeroConfig")]
    public class HeroConfig : ScriptableObject
    {
        [SerializeField, Min(0.01f)] private float _moveSpeed = 5f;
        [SerializeField, Min(0.01f)] private float _maxHealth = 100f;
        [SerializeField, Min(0.01f)] private float _fireRate = 0.5f;

        public float MoveSpeed => _moveSpeed;
        public float MaxHealth => _maxHealth;
        public float FireRate => _fireRate;
    }
}