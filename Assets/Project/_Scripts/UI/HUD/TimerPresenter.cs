using System;
using ArenaShooter.Gameplay.Enemies;
using ArenaShooter.Infrastructure.Reset;
using ArenaShooter.Services.Gameplay;
using UnityEngine;
using Zenject;

namespace ArenaShooter.UI.HUD
{
    public class TimerPresenter : ITickable, IResettable
    {
        private readonly ITimerView _view;
        private readonly MatchDurationSystem _durationSystem;
        
        private int _lastUpdatedSecond = -1;

        public TimerPresenter(ITimerView view, MatchDurationSystem durationSystem)
        {
            _view = view ?? throw new ArgumentNullException(nameof(view));
            _durationSystem = durationSystem ?? throw new ArgumentNullException(nameof(durationSystem));
        }

        public void Tick()
        {
            float remainingTime = _durationSystem.RemainingTime;
            int totalIntSeconds = Mathf.FloorToInt(remainingTime);

            if (totalIntSeconds != _lastUpdatedSecond)
            {
                _lastUpdatedSecond = totalIntSeconds;
                
                int minutes = totalIntSeconds / 60;
                int seconds = totalIntSeconds % 60;

                _view.SetTime(minutes, seconds);
            }
        }


        public void ResetState() => _view.SetTime(0,0);
    }
}