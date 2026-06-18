using UnityEngine;

namespace ArenaShooter.Configs
{
    [CreateAssetMenu(fileName = "MatchConfig", menuName = "Configs/MatchConfig")]
    public class MatchConfig : ScriptableObject
    {
        [SerializeField, Min(1f)] private float _survivalDuration = 300f;

        public float SurvivalDuration => _survivalDuration;
    }
}