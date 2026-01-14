using UnityEngine;
using Zenject;

namespace _Game._Dev.Scripts.Runtime.Core.Factories
{
    public class ObstacleFactory : IObstacleFactory
    {
        private readonly DiContainer _container;
        private readonly GameObject _obstaclePrefab;

        public ObstacleFactory(DiContainer container, GameObject obstaclePrefab)
        {
            _container = container;
            _obstaclePrefab = obstaclePrefab;
        }

        public GameObject Create(Vector3 position)
        {
            return _container.InstantiatePrefab(_obstaclePrefab, position, Quaternion.identity, null);
        }
    }
}