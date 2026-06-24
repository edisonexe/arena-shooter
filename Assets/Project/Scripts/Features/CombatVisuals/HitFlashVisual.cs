using UnityEngine;

namespace ArenaShooter.Features.CombatVisuals
{
    public class HitFlashVisual : MonoBehaviour
    {
        [Header("Renderers to Flash (Mesh & Skinned)")]
        [SerializeField] private Renderer[] _renderers;
        
        private MaterialPropertyBlock _flashPropertyBlock;
        private MaterialPropertyBlock _emptyPropertyBlock;
        
        private int _emissionColorPropertyId;
        
        private float _flashTimer;
        private float _flashDuration;
        private float _flashCount;
        private bool _isFlashing;

        private void Awake()
        {
            _flashPropertyBlock = new MaterialPropertyBlock();
            _emptyPropertyBlock = new MaterialPropertyBlock();
            
            _emissionColorPropertyId = Shader.PropertyToID("_EmissionColor");
            
            if (_renderers == null || _renderers.Length == 0)
            {
                _renderers = GetComponentsInChildren<Renderer>(true);
            }
        }

        public void Initialize()
        {
            _isFlashing = false;
            _flashTimer = 0f;
        }
        
        public void PlayFlash(float duration, int flashes = 3)
        {
            if (_renderers == null || _renderers.Length == 0) return;

            _flashDuration = duration;
            _flashCount = flashes;
            _flashTimer = duration;
            _isFlashing = true;
        }

        public void TickFlash(float deltaTime)
        {
            if (!_isFlashing) return;

            _flashTimer -= deltaTime;

            if (_flashTimer <= 0f)
            {
                _isFlashing = false;
                ResetFlash();
                return;
            }

            float progress = _flashTimer / _flashDuration;
            float wave = Mathf.Abs(Mathf.Sin(progress * Mathf.PI * _flashCount));

            float flashIntensity = wave * progress * 2f; 
            Color flashColor = new Color(flashIntensity, flashIntensity, flashIntensity, 1f);

            int count = _renderers.Length;
            for (int i = 0; i < count; i++)
            {
                if (_renderers[i] == null) continue;

                _flashPropertyBlock.SetColor(_emissionColorPropertyId, flashColor);
                _renderers[i].SetPropertyBlock(_flashPropertyBlock);
            }
        }

        public void ResetFlash()
        {
            if (_renderers == null) return;
            
            int count = _renderers.Length;
            for (int i = 0; i < count; i++)
            {
                if (_renderers[i] != null)
                {
                    _renderers[i].SetPropertyBlock(_emptyPropertyBlock);
                }
            }
        }

        private void OnValidate()
        {
            if (_renderers == null || _renderers.Length == 0)
            {
                _renderers = GetComponentsInChildren<Renderer>(true);
            }
        }
    }
}