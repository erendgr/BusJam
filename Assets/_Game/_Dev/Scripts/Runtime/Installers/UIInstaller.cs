using _Game._Dev.Scripts.Runtime.UI.StateMachine;
using _Game._Dev.Scripts.Runtime.UI.States;
using _Game._Dev.Scripts.Runtime.UI.Views;
using UnityEngine;
using Zenject;

namespace _Game._Dev.Scripts.Runtime.Installers
{
    public class UIInstaller : MonoInstaller
    {
        [SerializeField] private  GameObject startScreenPanel;
        [SerializeField] private  GameObject gameplayPanel;
        [SerializeField] private  EndScreenView  endScreenView;
        
        public override void InstallBindings()
        {
            Container.Bind<IUIState>().To<UIStartState>().AsSingle().WithArguments(startScreenPanel);
            Container.Bind<IUIState>().To<UIGameplayState>().AsSingle().WithArguments(gameplayPanel);
            Container.Bind<IUIState>().To<UIEndGameState>().AsSingle().WithArguments(endScreenView);

            
            Container.BindInterfacesAndSelfTo<UIStateMachine>().AsSingle().NonLazy();
        }
    }
}