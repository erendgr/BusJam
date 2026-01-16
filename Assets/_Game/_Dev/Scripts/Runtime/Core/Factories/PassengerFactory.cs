using _Game._Dev.Scripts.Runtime.Core.Level;
using _Game._Dev.Scripts.Runtime.Core.PassengerSystem;
using _Game._Dev.Scripts.Runtime.Features.Passenger.Controllers;
using _Game._Dev.Scripts.Runtime.Features.Passenger.Models;
using _Game._Dev.Scripts.Runtime.Features.Passenger.Views;
using UnityEngine;
using Zenject;

namespace _Game._Dev.Scripts.Runtime.Core.Factories
{
    public class PassengerFactory : IPassengerFactory
    {
        private readonly DiContainer _container;
        private readonly GameObject _passengerPrefab;
        private readonly IPassengerSystemManager _passengerSystemManager;

        public PassengerFactory(
            DiContainer container,
            GameObject passengerPrefab,
            IPassengerSystemManager passengerSystemManager)
        {
            _container = container;
            _passengerPrefab = passengerPrefab;
            _passengerSystemManager = passengerSystemManager;
        }

        public IPassengerController Create(Colors color, Vector3 worldPosition, Vector2Int gridPosition)
        {
            var view = _container.InstantiatePrefabForComponent<PassengerView>(_passengerPrefab, worldPosition, Quaternion.identity, null);
            view.Initialize(color);

            var model = new PassengerModel(gridPosition);

            var controller = _container.Instantiate<PassengerController>(new object[] { view, model });

            _passengerSystemManager.Register(controller);

            return controller;
        }
    }
}