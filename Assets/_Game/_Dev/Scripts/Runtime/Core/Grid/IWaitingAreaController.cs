using System.Collections.Generic;
using _Game._Dev.Scripts.Runtime.MVC.Passenger.Views;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Game._Dev.Scripts.Runtime.Core.Grid
{
    public interface IWaitingAreaController
    {
        Vector2Int? ReserveNextAvailableSlot();
        UniTask FinalizeMoveToSlot(PassengerView character, Vector2Int reservedSlot);
        int GetWaitingCharacterCount();
        void Reset();
        bool IsFull();
        IReadOnlyList<PassengerView> GetWaitingCharacters();
    }
}