using System.Collections.Generic;
using _Game._Dev.Scripts.Runtime.Features.Passenger.Controllers;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Game._Dev.Scripts.Runtime.Core.Grid
{
    public interface IWaitingAreaController
    {
        Vector2Int? ReserveNextAvailableSlot();
        UniTask FinalizeMoveToSlot(IPassengerController passenger, Vector2Int reservedSlot);
        int GetWaitingPassengersCount();
        void Reset();
        bool IsFull();
        IReadOnlyList<IPassengerController> GetWaitingPassengers();
    }
}