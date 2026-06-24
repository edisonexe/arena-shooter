using ArenaShooter.Features.Progression.Configs;
using ArenaShooter.Infrastructure.SceneLoading;
using ArenaShooter.Infrastructure.Signals;
using ArenaShooter.Infrastructure.StateMachine;
using ArenaShooter.Infrastructure.StateMachine.States;
using ArenaShooter.Services.Gameplay;
using ArenaShooter.Services.Input;
using UnityEngine;
using Zenject;

namespace ArenaShooter.Infrastructure.DI
{
    public class ProjectInstaller : MonoInstaller
    {
        [Header("Global Match Config")]
        [SerializeField] private MatchConfig _matchConfig;

        public override void InstallBindings()
        {
            ValidateInInspector();
            
            Container.Bind<SignalBus>().AsSingle();
            
            Container.BindInstance(_matchConfig).AsSingle();
            
            Container.BindInterfacesAndSelfTo<NewInputService>().AsSingle().NonLazy();
            Container.Bind<SceneLoaderService>().AsSingle();
            Container.Bind<GameNavigationService>().AsSingle();
            
            Container.BindInterfacesAndSelfTo<MatchDurationSystem>().AsSingle().NonLazy();
            
            Container.Bind<StateFactory>().AsSingle();
            Container.BindInterfacesAndSelfTo<GameStateMachine>().AsSingle().NonLazy();
            
            Container.Bind<BootstrapState>().AsSingle();
            Container.Bind<GameMenuState>().AsSingle();
            Container.Bind<GameplayState>().AsSingle();
            Container.Bind<UpgradeSelectionState>().AsSingle();
            Container.Bind<GameOverState>().AsSingle();
            Container.Bind<GameVictoryState>().AsSingle();
        }
        
        private void ValidateInInspector()
        {
            if (!_matchConfig) Debug.LogError("[ProjectInstaller] MatchConfig is not assigned in ProjectContext Prefab!", this);
        }
    }
}