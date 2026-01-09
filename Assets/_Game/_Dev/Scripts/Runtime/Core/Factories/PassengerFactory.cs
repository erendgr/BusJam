using _Game._Dev.Scripts.Runtime.Level.Models;
using _Game._Dev.Scripts.Runtime.MVC.Passenger.Views;
using UnityEngine;
using Zenject;

namespace _Game._Dev.Scripts.Runtime.Core.Factories
{
    public class PassengerFactory : IPassengerFactory
    {
        private readonly DiContainer _container;
        private readonly GameObject _characterPrefab;

        public PassengerFactory(DiContainer container, GameObject characterPrefab)
        {
            _container = container;
            _characterPrefab = characterPrefab;
        }

        public PassengerView Create(Colors color, Vector3 position, Vector2Int gridPosition)
        {
            var characterInstance =
                _container.InstantiatePrefabForComponent<PassengerView>(_characterPrefab, position, Quaternion.identity,
                    null);
            characterInstance.Initialize(color, gridPosition);

            return characterInstance;
        }
    }
}