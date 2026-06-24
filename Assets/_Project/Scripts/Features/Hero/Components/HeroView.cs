using ArenaShooter.Features.CombatVisuals;
using UnityEngine;

namespace ArenaShooter.Features.Hero.Components
{
    [RequireComponent(typeof(Rigidbody), typeof(DamageVisualTilt))]
    public class HeroView : MonoBehaviour
    {
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private Transform _firePoint;
        [SerializeField] private Transform _visualRoot;
        [SerializeField] private DamageVisualTilt _visualTilt;
        [SerializeField] private Transform _cameraAnchor;
        
        public Rigidbody Rigidbody => _rigidbody;
        public Transform FirePoint => _firePoint;
        public Transform VisualRoot => _visualRoot;
        public DamageVisualTilt VisualTilt => _visualTilt;
        public Transform CameraAnchor => _cameraAnchor;
        
        private void OnValidate()
        {
            if (!_rigidbody) _rigidbody = GetComponent<Rigidbody>();
            if (!_visualTilt) _visualTilt = GetComponent<DamageVisualTilt>();
            if (!_visualRoot) Debug.LogError("[HeroView] Visual Root is not assigned!", this);
            if (!_firePoint) Debug.LogError("[HeroView] FirePoint Transform is not assigned!", this);
            if (!_cameraAnchor) Debug.LogError("[HeroView] Camera Anchor Transform is not assigned!", this);
        }
    }
}