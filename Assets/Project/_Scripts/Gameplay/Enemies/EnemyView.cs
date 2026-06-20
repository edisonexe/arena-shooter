using ArenaShooter.Gameplay.Combat;
using ArenaShooter.Infrastructure.Pooling;
using UnityEngine;

namespace ArenaShooter.Gameplay.Enemies
{
    [RequireComponent(typeof(Collider), typeof(DamageVisualTilt), typeof(HitFlashVisual))]
    public class EnemyView : MonoBehaviour, IPoolable<EnemyView>
    {
        [SerializeField] private MeshRenderer _meshRenderer;
        [SerializeField] private Collider _collider;
        [SerializeField] private DamageVisualTilt _visualTilt;
        [SerializeField] private HitFlashVisual _hitFlash;

        public Transform Transform => transform;

        public void Initialize()
        {
            if (_hitFlash) _hitFlash.Initialize();
        }

        public void Spawn()
        {
            gameObject.SetActive(true);
            _meshRenderer.enabled = true;
            _collider.enabled = true;
        }

        public void Despawn()
        {
            _meshRenderer.enabled = false;
            _collider.enabled = false;
            gameObject.SetActive(false);
        }

        public void ApplyVisualDamage(Vector3 damageSourcePosition, float flashDuration, int flashes)
        {
            if (_visualTilt)
            {
                Vector3 hitDirection = (transform.position - damageSourcePosition);
                hitDirection.y = 0f;

                if (hitDirection.sqrMagnitude > 0.001f)
                {
                    _visualTilt.ApplyDirectionalTilt(hitDirection.normalized);
                }
            }

            if (_hitFlash) _hitFlash.PlayFlash(flashDuration, flashes);
        }

        public void TickVisuals(float deltaTime)
        {
            if (_visualTilt) _visualTilt.TickTilt(deltaTime);
            if (_hitFlash) _hitFlash.TickFlash(deltaTime);
        }

        private void OnValidate()
        {
            if (!_meshRenderer) Debug.LogError($"[EnemyView] MeshRenderer is not assigned'!", this);
            
            if (!(_collider ??= GetComponent<Collider>())) 
                Debug.LogError($"[Enemy] Collider is missing on '{name}'!", this);
            
            if (!(_visualTilt ??= GetComponent<DamageVisualTilt>())) 
                Debug.LogError($"[Enemy] DamageVisualTilt is missing on '{name}'!", this);
            
            if (!(_hitFlash ??= GetComponent<HitFlashVisual>())) Debug.LogError($"[Enemy] HitFlashVisual is missing!", this);
        }
    }
}