using ArenaShooter.Infrastructure.Pooling;
using UnityEngine;

namespace ArenaShooter.Features.Items
{
    public class XPGem : MonoBehaviour, IPoolable<XPGem>
    {
        [SerializeField] private MeshRenderer _meshRenderer;

        public int XpValue { get; private set; }
    
        public bool IsActive { get; private set; }

        public void Initialize(Vector3 position, int xpValue)
        {
            transform.position = position;
            XpValue = xpValue;
        }

        public void Spawn()
        {
            IsActive = true;
            
            _meshRenderer.enabled = true;
        }

        public void Despawn()
        {
            IsActive = false;
            _meshRenderer.enabled = false;
        }
        
        private void OnValidate()
        {
            if (!(_meshRenderer ??= GetComponent<MeshRenderer>())) 
                Debug.LogError($"[XPGem] MeshRenderer is missing on '{name}'!", this);
        }
    }
}