using System.Threading;
using _Game._Dev.Scripts.Runtime.Features.Passenger.Views;
using _Game._Dev.Scripts.Runtime.Level.Models;
using _Game._Dev.Scripts.Runtime.MVC.Bus.Views;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Game._Dev.Scripts.Runtime.Features.Bus.Controllers
{
    public interface IBusController
    {
        BusView View { get; }
        bool CanBoard(PassengerView passenger);
        UniTask BoardPassengerAsync(PassengerView passenger);
        UniTask  Initialize(Vector3 arrivalPosition, CancellationToken cancellationToken);
        bool HasSpace();
        Colors GetColor();
        bool IsAcceptingPassengers { get; set; }
    }
}