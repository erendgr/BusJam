using _Game._Dev.Scripts.Runtime.Level.Models;
using _Game._Dev.Scripts.Runtime.MVC.Passenger.Views;
using UnityEngine;

namespace _Game._Dev.Scripts.Runtime.Core.Factories
{
    public interface IPassengerFactory
    {
        PassengerView Create(Colors color, Vector3 position, Vector2Int gridPosition);
    }
}