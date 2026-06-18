using UnityEngine;

namespace ArenaShooter.Services.Gameplay
{
    public class ArenaBoundsService : MonoBehaviour
    {
        [Header("Arena Bounds Settings")]
        [SerializeField] private Vector2 _center = Vector2.zero;
        [SerializeField] private Vector2 _size = new(40f, 40f);

        public Vector3 ClampToArena(Vector3 targetPosition)
        {
            float halfX = _size.x / 2f;
            float halfZ = _size.y / 2f;

            float clampedX = Mathf.Clamp(targetPosition.x, _center.x - halfX, _center.x + halfX);
            float clampedZ = Mathf.Clamp(targetPosition.z, _center.y - halfZ, _center.y + halfZ);

            return new Vector3(clampedX, targetPosition.y, clampedZ);
        }

        public bool IsInsideArena(Vector3 position, float offset = 0.5f)
        {
            float halfX = (_size.x / 2f) - offset;
            float halfZ = (_size.y / 2f) - offset;

            return position.x >= _center.x - halfX && position.x <= _center.x + halfX &&
                   position.z >= _center.y - halfZ && position.z <= _center.y + halfZ;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.cyan;
            Vector3 center3D = new Vector3(_center.x, 0f, _center.y);
            Vector3 size3D = new Vector3(_size.x, 0.1f, _size.y);
            Gizmos.DrawWireCube(center3D, size3D);
        }
    }
}