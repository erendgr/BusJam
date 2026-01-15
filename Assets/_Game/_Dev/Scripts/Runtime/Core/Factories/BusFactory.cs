using System.Threading;
using _Game._Dev.Scripts.Runtime.Features.Bus.Controllers;
using _Game._Dev.Scripts.Runtime.Level.Models;
using _Game._Dev.Scripts.Runtime.MVC.Bus.Models;
using _Game._Dev.Scripts.Runtime.MVC.Bus.Views;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace _Game._Dev.Scripts.Runtime.Core.Factories
{
    public class BusFactory : IBusFactory
    {
        private readonly DiContainer _container;
        private readonly GameObject _busPrefab;
        private readonly BusSettingsSO _busSettings;

        public BusFactory(DiContainer container, GameObject busPrefab, BusSettingsSO busSettings)
        {
            _container = container;
            _busPrefab = busPrefab;
            _busSettings = busSettings;
        }

        public async UniTask<IBusController> Create(Colors color, Vector3 arrivalPosition, CancellationToken cancellationToken)
        {
            var busView = _container.InstantiatePrefabForComponent<BusView>(_busPrefab);
            var busModel = new BusModel(color, _busSettings.Capacity); 
            var busController = _container.Instantiate<BusController>(new object[] { busModel, busView });

            await busController.Initialize(arrivalPosition, cancellationToken);

            return busController;
        }
        
        public IBusController CreateAtPosition(Colors color, Vector3 position)
        {
            var busView = _container.InstantiatePrefabForComponent<BusView>(_busPrefab, position, Quaternion.identity, null);
            var busModel = new BusModel(color, _busSettings.Capacity);
            var busController = _container.Instantiate<BusController>(new object[] { busModel, busView });
            
            return busController;
        }
    }
}