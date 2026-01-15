using _Game._Dev.Scripts.Runtime.Features.Passenger.Models;
using _Game._Dev.Scripts.Runtime.Features.Passenger.Views;

namespace _Game._Dev.Scripts.Runtime.Features.Passenger.Controllers
{
    public interface IPassengerController
    {
        public PassengerView View { get; }
        public PassengerModel Model { get; }
    }
}