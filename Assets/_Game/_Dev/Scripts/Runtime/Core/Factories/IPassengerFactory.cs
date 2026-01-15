using _Game._Dev.Scripts.Runtime.Features.Passenger.Controllers;
using _Game._Dev.Scripts.Runtime.Level.Models;
using UnityEngine;

namespace _Game._Dev.Scripts.Runtime.Core.Factories
{
    public interface IPassengerFactory
    {
        IPassengerController Create(Colors color, Vector3 position, Vector2Int gridPosition);
    }
}