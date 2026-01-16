using System.Threading;
using _Game._Dev.Scripts.Runtime.Features.Bus.Views;
using _Game._Dev.Scripts.Runtime.Level.Models;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Game._Dev.Scripts.Runtime.Features.Bus.Controllers
{
    public interface IBusController
    {
        BusView View { get; }
        bool CanBoard(Colors passengerColor);
        UniTask  Initialize(Vector3 arrivalPosition, CancellationToken cancellationToken);
        bool HasSpace();
        Colors GetColor();
        bool IsAcceptingPassengers { get; set; }
        int ReserveSlot();
    }
}