using UnityEngine;

namespace ArenaShooter.Infrastructure.StateMachine.States
{
    public class GameVictoryState : IState
    {
        public void Enter()
        {
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        public void Exit()
        {
            Time.timeScale = 1f;
        }
    }
}