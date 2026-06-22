using UnityEngine;

namespace ArenaShooter.Gameplay.Combat
{
    public class DamageVisualTilt : MonoBehaviour
    {
        [SerializeField] private Transform _tiltRoot;
        
        private float _tiltAngle;
        private Vector3 _localTiltAxis;
        
        private const float SPRING_STIFFNESS = 130f; 
        private const float SPRING_DAMPING = 24f;    
        private float _springVelocity;
        
        public void ApplyDirectionalTilt(Vector3 hitDirection)
        {
            if (!_tiltRoot) return;
            
            Transform parentLayer = _tiltRoot.parent ? _tiltRoot.parent : transform;
            Vector3 localHitDirection = parentLayer.InverseTransformDirection(hitDirection).normalized;
            
            _localTiltAxis = Vector3.Cross(Vector3.up, localHitDirection).normalized;
            
            _tiltAngle = 18f; 
            _springVelocity = 0f;
        }

        public void TickTilt(float deltaTime)
        {
            if (!_tiltRoot) return;
            if (Mathf.Abs(_tiltAngle) < 0.05f && Mathf.Abs(_springVelocity) < 0.05f)
            {
                _tiltAngle = 0f;
                _springVelocity = 0f;
                _tiltRoot.localRotation = Quaternion.identity;
                return;
            }

            float springForce = -SPRING_STIFFNESS * _tiltAngle;
            _springVelocity += (springForce - SPRING_DAMPING * _springVelocity) * deltaTime;
            _tiltAngle += _springVelocity * deltaTime;
            
            if (_tiltAngle < 0f)
            {
                _tiltAngle = 0f;
                _springVelocity = 0f;
            }

            _tiltRoot.localRotation = Quaternion.AngleAxis(_tiltAngle, _localTiltAxis);
        }

        private void OnValidate()
        {
            if (!_tiltRoot) Debug.LogError($"[DamageVisualTilt] Tilt Root Transform is missing on '{name}'!", this);
        }
    }
}