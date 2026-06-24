using UnityEngine;

namespace ArenaShooter.Features.Hero.Configs
{
    [CreateAssetMenu(fileName = "HeroConfig", menuName = "Configs/HeroConfig")]
    public class HeroConfig : ScriptableObject
    {
        [Header("Hero Stats")]
        [SerializeField, Min(0.01f)] private float _moveSpeed = 5f;
        [SerializeField, Min(0.1f)] private float _maxHealth = 100f;
        [SerializeField, Min(0f)] private float _baseHealthRegen = 0.5f;
        [SerializeField, Min(1f)] private float _basePickupRadius = 3f;
        
        public float MoveSpeed => _moveSpeed;
        public float MaxHealth => _maxHealth;
        public float BaseHealthRegen => _baseHealthRegen;
        public float BasePickupRadius => _basePickupRadius;
    }
}