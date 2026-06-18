using UnityEngine;

namespace ArenaShooter.Gameplay.Combat
{
    public class HitFlashVisual : MonoBehaviour
    {
        [SerializeField] private MeshRenderer _meshRenderer;
        
        private MaterialPropertyBlock _propertyBlock;
        private int _baseColorPropertyId;
        private Color _initialColor;
        
        private float _flashTimer;
        private float _flashDuration;
        private float _flashCount;
        private bool _isFlashing;

        public void Initialize()
        {
            _propertyBlock ??= new MaterialPropertyBlock();
            _baseColorPropertyId = Shader.PropertyToID("_BaseColor");
            
            if (_meshRenderer)
            {
                _initialColor = _meshRenderer.sharedMaterial.GetColor(_baseColorPropertyId);
            }
        }
        
        public void PlayFlash(float duration, int flashes = 3)
        {
            if (!_meshRenderer) return;

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
            
            Color currentFlashColor = Color.Lerp(_initialColor, Color.white, wave * progress);

            _meshRenderer.GetPropertyBlock(_propertyBlock);
            _propertyBlock.SetColor(_baseColorPropertyId, currentFlashColor);
            _meshRenderer.SetPropertyBlock(_propertyBlock);
        }

        private void ResetFlash()
        {
            if (!_meshRenderer) return;
            
            _meshRenderer.GetPropertyBlock(_propertyBlock);
            _propertyBlock.SetColor(_baseColorPropertyId, _initialColor);
            _meshRenderer.SetPropertyBlock(_propertyBlock);
        }

        private void OnValidate()
        {
            if (!_meshRenderer) _meshRenderer = GetComponent<MeshRenderer>();
        }
    }
}