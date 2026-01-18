using _Game._Dev.Scripts.Runtime.Core.BoardingSystem;
using _Game._Dev.Scripts.Runtime.Core.BusSystem;
using _Game._Dev.Scripts.Runtime.Core.Camera;
using _Game._Dev.Scripts.Runtime.Core.GameplayState;
using _Game._Dev.Scripts.Runtime.Core.Grid;
using _Game._Dev.Scripts.Runtime.Core.Level;
using _Game._Dev.Scripts.Runtime.Core.MovementTracker;
using _Game._Dev.Scripts.Runtime.Core.PassengerSystem;
using _Game._Dev.Scripts.Runtime.Core.Pathfinding;
using _Game._Dev.Scripts.Runtime.Core.Timer;
using _Game._Dev.Scripts.Runtime.Features.Passenger.Controllers;
using _Game._Dev.Scripts.Runtime.Utilities;
using UnityEngine;
using Zenject;

namespace _Game._Dev.Scripts.Runtime.Installers
{
    public class GameLogicInstaller : MonoInstaller
    {
        [SerializeField] private Camera mainCamera;
        
        public override void InstallBindings()
        {
            Container.Bind<ILevelController>().To<LevelController>().AsSingle().NonLazy();
            Container.Bind<IPathfindingService>().To<PathfindingService>().AsSingle();
            Container.Bind<Camera>().FromInstance(mainCamera).AsSingle();
            Container.BindInterfacesAndSelfTo<CameraController>().AsSingle();
            Container.BindInterfacesAndSelfTo<GridSystemManager>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<BusSystemManager>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<PassengerSystemManager>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<BoardingSystem>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<WaitingAreaController>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<GameStateManager>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<LevelProgressController>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<PassengerMovementController>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<GameplayTimer>().AsSingle();
            Container.BindInterfacesAndSelfTo<MovementTracker>().AsSingle();
            Container.BindInterfacesAndSelfTo<GameplayStateHolder>().AsSingle();
        }
    }
}