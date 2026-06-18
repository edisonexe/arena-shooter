using UnityEngine;

namespace ArenaShooter.Services.Input
{
    public interface IInputService
    {
        Vector2 Axis { get; }
        void Enable();
        void Disable();
    }
}