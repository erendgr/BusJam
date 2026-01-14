using UnityEngine;

namespace _Game._Dev.Scripts.Runtime.Core.Factories
{
    public interface IObstacleFactory
    {
        GameObject Create(Vector3 position);
    }
}