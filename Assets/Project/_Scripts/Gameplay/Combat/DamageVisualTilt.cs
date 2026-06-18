using UnityEngine;

namespace ArenaShooter.Gameplay.Combat
{
    public class DamageVisualTilt : MonoBehaviour
    {
        [SerializeField] private Transform _visualRoot;
        
        private float _tiltAngle;
        private Vector3 _worldTiltAxis;
        
        private const float SPRING_STIFFNESS = 130f; 
        private const float SPRING_DAMPING = 24f;    
        private float _springVelocity;
        
        public void ApplyDirectionalTilt(Vector3 hitDirection)
        {
            if (!_visualRoot) return;
            
            _worldTiltAxis = Vector3.Cross(Vector3.up, hitDirection).normalized;
            
            _tiltAngle = 18f; 
            _springVelocity = 0f;
        }

        public void TickTilt(float deltaTime)
        {
            if (!_visualRoot) return;
            if (Mathf.Abs(_tiltAngle) < 0.05f && Mathf.Abs(_springVelocity) < 0.05f)
            {
                _tiltAngle = 0f;
                _springVelocity = 0f;
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

            _visualRoot.localRotation = Quaternion.Inverse(transform.rotation) * Quaternion.AngleAxis(_tiltAngle, _worldTiltAxis) * transform.rotation;
        }

        private void OnValidate()
        {
            if (!_visualRoot) Debug.LogError($"[DamageVisualTilt] VisualRoot Transform is missing on '{name}'!", this);
        }
    }
}