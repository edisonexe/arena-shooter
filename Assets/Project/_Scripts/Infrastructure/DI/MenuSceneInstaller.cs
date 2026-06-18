using ArenaShooter.UI.Menu;
using UnityEngine;
using Zenject;

namespace ArenaShooter.Infrastructure.DI
{
    public class MenuSceneInstaller : MonoInstaller
    {
        [SerializeField] private MainMenuView _mainMenuView;

        public override void InstallBindings()
        {
            ValidateInInspector();
            
            Container.BindInterfacesAndSelfTo<MainMenuView>().FromInstance(_mainMenuView).AsSingle();
            Container.BindInterfacesAndSelfTo<MainMenuPresenter>().AsSingle().NonLazy();
        }

        private void ValidateInInspector()
        {
            if (!_mainMenuView) Debug.LogError("[MenuSceneInstaller] MainMenuView missing!", this);
        }
    }
}