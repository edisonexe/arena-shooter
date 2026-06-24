using UnityEngine;

namespace ArenaShooter.Features.Camera.Configs
{
    [CreateAssetMenu(fileName = "CameraConfig", menuName = "Configs/CameraConfig")]
    public class CameraConfig : ScriptableObject
    {
        [Header("Follow Settings")]
        [SerializeField, Min(0.1f)] private float _anchorLerpSpeed = 3f;
        
        public float AnchorLerpSpeed => _anchorLerpSpeed;
    }
}