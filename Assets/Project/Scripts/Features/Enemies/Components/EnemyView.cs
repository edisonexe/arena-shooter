using ArenaShooter.Features.CombatVisuals;
using ArenaShooter.Infrastructure.Pooling;
using UnityEngine;

namespace ArenaShooter.Features.Enemies.Components
{
    [RequireComponent(typeof(Collider), typeof(DamageVisualTilt), typeof(HitFlashVisual))]
    public class EnemyView : MonoBehaviour, IPoolable<EnemyView>
    {
        [Header("Renderers Configuration")]
        [SerializeField] private Renderer[] _allRenderers;

        [Header("Physics & Visual Components")]
        [SerializeField] private Collider _collider;
        [SerializeField] private DamageVisualTilt _visualTilt;
        [SerializeField] private HitFlashVisual _hitFlash;

        private Transform _cachedTransform;
        public Transform Transform => _cachedTransform ??= transform;
        
        public void Initialize()
        {
            _cachedTransform ??= transform;
            if (_hitFlash) _hitFlash.Initialize();
        }

        public void Spawn()
        {
            SetRenderersVisible(true);
            if (_collider) _collider.enabled = true;
        }

        public void Despawn()
        {
            SetRenderersVisible(false);
            if (_collider) _collider.enabled = false;
            
            if (_hitFlash) _hitFlash.ResetFlash();
        }

        public void ApplyVisualDamage(Vector3 damageSourcePosition, float flashDuration, int flashes)
        {
            if (_visualTilt)
            {
                Vector3 hitDirection = (Transform.position - damageSourcePosition);
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
        
        private void SetRenderersVisible(bool isVisible)
        {
            if (_allRenderers == null) return;

            for (int i = 0; i < _allRenderers.Length; i++)
            {
                if (_allRenderers[i] != null)
                {
                    _allRenderers[i].enabled = isVisible;
                }
            }
        }

        private void OnValidate()
        {
            if (_allRenderers == null || _allRenderers.Length == 0)
            {
                _allRenderers = GetComponentsInChildren<Renderer>(true);
            }

            if (_allRenderers.Length == 0) 
                Debug.LogError($"[EnemyView] No MeshRenderers found on '{name}'!", this);

            if (!(_collider ??= GetComponent<Collider>())) 
                Debug.LogError($"[EnemyView] Collider is missing on '{name}'!", this);
            
            if (!(_visualTilt ??= GetComponent<DamageVisualTilt>())) 
                Debug.LogError($"[EnemyView] DamageVisualTilt is missing on '{name}'!", this);
            
            if (!(_hitFlash ??= GetComponent<HitFlashVisual>())) 
                Debug.LogError($"[EnemyView] HitFlashVisual is missing on '{name}'!", this);
        }
    }
}