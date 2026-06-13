using UnityEngine;

namespace ArenaShooter.Gameplay.Hero
{
    [RequireComponent(typeof(Rigidbody))]
    public class HeroView : MonoBehaviour
    {
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private Transform _firePoint;

        public Rigidbody Rigidbody => _rigidbody;
        public Transform FirePoint => _firePoint;

        private void OnValidate()
        {
            if (_rigidbody == null)
            {
                _rigidbody = GetComponent<Rigidbody>();
            }
            
            if (_firePoint == null)
            {
                Debug.LogError("[HeroView] FirePoint Transform is not assigned in the Inspector!", this);
            }
        }
    }
}