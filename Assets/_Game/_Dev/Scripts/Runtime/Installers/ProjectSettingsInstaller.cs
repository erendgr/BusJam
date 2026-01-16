using _Game._Dev.Scripts.Runtime.Core.Events;
using _Game._Dev.Scripts.Runtime.Core.Grid;
using _Game._Dev.Scripts.Runtime.Features.Bus.Models;
using _Game._Dev.Scripts.Runtime.Level.Models;
using UnityEngine;
using Zenject;

namespace _Game._Dev.Scripts.Runtime.Installers
{
    public class ProjectSettingsInstaller : MonoInstaller
    {
        [SerializeField] private GridConfigurationSO gridConfiguration;
        [SerializeField] private LevelProgressionSO levelProgression;
        [SerializeField] private BusSettingsSO busSettings;
        
        public override void InstallBindings()
        {
            SignalBusInstaller.Install(Container);
            Container.DeclareSignal<LevelLoadRequestedSignal>();
            Container.DeclareSignal<LevelCompletedSignal>();
            Container.DeclareSignal<LevelReadySignal>();
            Container.DeclareSignal<TimerUpdatedSignal>();
            Container.DeclareSignal<PassengerClickedSignal>();
            Container.DeclareSignal<BusArrivedSignal>();
            Container.DeclareSignal<BusFullSignal>();
            Container.DeclareSignal<GameOverSignal>();
            Container.DeclareSignal<AllBusesDispatchedSignal>();
            Container.DeclareSignal<ResetGameplaySignal>();
            Container.DeclareSignal<NextLevelRequestedSignal>();
            Container.DeclareSignal<StartGameRequestedSignal>();
            Container.DeclareSignal<RestartLevelRequestedSignal>();
            Container.DeclareSignal<WaitingAreaChangedSignal>();
            Container.DeclareSignal<LevelCompleteSequenceFinishedSignal>();
            Container.DeclareSignal<TimeIsUpSignal>();
            Container.DeclareSignal<BusArrivalSequenceStartedSignal>();
            Container.DeclareSignal<PassengerEnteredWaitingAreaSignal>();
            
            Container.Bind<GridConfigurationSO>().FromInstance(gridConfiguration).AsSingle();
            Container.Bind<LevelProgressionSO>().FromInstance(levelProgression).AsSingle();
            Container.Bind<BusSettingsSO>().FromInstance(busSettings).AsSingle();
        }
    }
}