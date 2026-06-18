using UnityEngine;

namespace ArenaShooter.Gameplay.Combat
{
    public class DamageVisualTilt : MonoBehaviour
    {
        [SerializeField] private Transform _visualRoot;
        
        private float _tiltAngle;
        private Vector3 _worldTiltAxis;
        
        private const float SpringStiffness = 130f; 
        private const float SpringDamping = 24f;    
        private float _springVelocity;
        
        public void ApplyStrictBackwardTilt(Vector3 lookAtTargetDirection)
        {
            if (!_visualRoot) return;

            Vector3 forwardDirection = new Vector3(lookAtTargetDirection.x, 0f, lookAtTargetDirection.z).normalized;
            
            _worldTiltAxis = Vector3.Cross(Vector3.up, forwardDirection).normalized;

            _tiltAngle = -18f; 
            _springVelocity = 0f;
        }

        public void TickTilt(float deltaTime)
        {
            if (!_visualRoot) return;
            if (Mathf.Abs(_tiltAngle) < 0.05f && Mathf.Abs(_springVelocity) < 0.05f)
            {
                _visualRoot.localRotation = Quaternion.identity;
                return;
            }

            float springForce = -SpringStiffness * _tiltAngle;
            _springVelocity += (springForce - SpringDamping * _springVelocity) * deltaTime;
            _tiltAngle += _springVelocity * deltaTime;
            
            if (_tiltAngle > 0f)
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