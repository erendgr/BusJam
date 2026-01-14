using _Game._Dev.Scripts.Runtime.Core;
using _Game._Dev.Scripts.Runtime.Core.Factories;
using UnityEngine;
using Zenject;

namespace _Game._Dev.Scripts.Runtime.Installers
{
    public class FactoriesInstaller : MonoInstaller
    {
        [SerializeField] private PrefabConfigurationSO prefabConfig;
        
        public override void InstallBindings()
        {
            Container.Bind<IPassengerFactory>().To<PassengerFactory>().AsSingle().WithArguments(prefabConfig.PassengerPrefab);
            Container.Bind<IObstacleFactory>().To<ObstacleFactory>().AsSingle().WithArguments(prefabConfig.ObstaclePrefab);
            Container.Bind<IBusFactory>().To<BusFactory>().AsSingle().WithArguments(prefabConfig.BusPrefab);
        }
    }
}