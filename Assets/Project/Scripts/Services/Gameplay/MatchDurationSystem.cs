using System;
using ArenaShooter.Features.Progression.Configs;
using ArenaShooter.Infrastructure.Reset;
using UnityEngine;
using Zenject;

namespace ArenaShooter.Services.Gameplay
{
    public class MatchDurationSystem : ITickable, IResettable
    {
        private readonly MatchConfig _matchConfig;
        
        private float _elapsedTime;
        private bool _isTimerRunning;
        
        public event Action OnTimeout;

        public float RemainingTime => Mathf.Max(0f, _matchConfig.SurvivalDuration - _elapsedTime);
        public float ElapsedTime => _elapsedTime;

        public MatchDurationSystem(MatchConfig matchConfig)
        {
            _matchConfig = matchConfig ?? throw new ArgumentNullException(nameof(matchConfig));
        }

        public void StartTimer() => _isTimerRunning = true;

        public void StopTimer() => _isTimerRunning = false;

        public void ResetState() => _elapsedTime = 0f;
        
        public void Tick()
        {
            if (!_isTimerRunning) return;

            _elapsedTime += Time.deltaTime;

            if (_elapsedTime >= _matchConfig.SurvivalDuration)
            {
                StopTimer();
                OnTimeout?.Invoke();
            }
        }
    }
}