using System;
using System.Collections.Generic;
using System.Linq;
using _Game._Dev.Scripts.Runtime.Core.BoardingSystem;
using _Game._Dev.Scripts.Runtime.Core.BusSystem;
using _Game._Dev.Scripts.Runtime.Core.Events;
using _Game._Dev.Scripts.Runtime.Features.Passenger.Controllers;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace _Game._Dev.Scripts.Runtime.Core.Grid
{
    public class WaitingAreaController : IWaitingAreaController, IInitializable, IDisposable
    {
        private readonly IGridSystemManager _gridSystemManager;
        private readonly IBusSystemManager _busSystemManager;
        private readonly IBoardingSystem _boardingSystem;
        private readonly SignalBus _signalBus;
        private readonly HashSet<Vector2Int> _reservedSlots;
        private readonly IPassengerController[] _slots;
        private bool _isBoardingLocked;
        
        public WaitingAreaController(IGridSystemManager gridSystemManager, GridConfigurationSO gridConfig,
            IBusSystemManager busSystemManager, SignalBus signalBus, IBoardingSystem boardingSystem)
        {
            _gridSystemManager = gridSystemManager;
            _busSystemManager = busSystemManager;
            _signalBus = signalBus;
            _slots = new IPassengerController[gridConfig.WaitingGridSize.x];
            _reservedSlots = new HashSet<Vector2Int>();
            _boardingSystem = boardingSystem;
        }

        public void Initialize()
        {
            _signalBus.Subscribe<BusArrivalSequenceStartedSignal>(OnBusArrivalSequenceStarted);
            _signalBus.Subscribe<BusArrivedSignal>(OnBusArrived);
            _signalBus.Subscribe<PassengerEnteredWaitingAreaSignal>(OnPassengerEnteredArea);
            _signalBus.Subscribe<ResetGameplaySignal>(Reset);
        }

        public void Dispose()
        {
            _signalBus.TryUnsubscribe<BusArrivalSequenceStartedSignal>(OnBusArrivalSequenceStarted);
            _signalBus.TryUnsubscribe<BusArrivedSignal>(OnBusArrived);
            _signalBus.TryUnsubscribe<PassengerEnteredWaitingAreaSignal>(OnPassengerEnteredArea);
            _signalBus.TryUnsubscribe<ResetGameplaySignal>(Reset);
        }

        private void OnBusArrivalSequenceStarted(BusArrivalSequenceStartedSignal signal)
        {
            _isBoardingLocked = true;
        }

        public bool IsPassengerInArea(IPassengerController passenger)
        {
            return _slots.Contains(passenger);
        }

        public Vector2Int? ReserveNextAvailableSlot()
        {
            var grid = _gridSystemManager.WaitingAreaGrid;
            for (int x = 0; x < _slots.Length; x++)
            {
                var cell = new Vector2Int(x, 0);
                if (grid.IsCellAvailable(cell) && !_reservedSlots.Contains(cell))
                {
                    _reservedSlots.Add(cell);
                    return cell;
                }
            }

            return null;
        }

        private void RemovePassengerFromArea(IPassengerController passenger)
        {
            int index = Array.IndexOf(_slots, passenger);
            if (index != -1)
            {
                var gridPos = new Vector2Int(index, 0);
                _gridSystemManager.WaitingAreaGrid.ClearCell(gridPos);
                _slots[index] = null;
            }
        }

        public async UniTask FinalizeMoveToSlot(IPassengerController passenger, Vector2Int reservedSlot)
        {
            var waitingGrid = _gridSystemManager.WaitingAreaGrid;
            var targetPosition = waitingGrid.GetWorldPosition(reservedSlot, 0.5f);

            await passenger.View.MoveToPoint(targetPosition);

            _reservedSlots.Remove(reservedSlot);
            waitingGrid.PlaceObject(passenger.View.gameObject, reservedSlot);
            _slots[reservedSlot.x] = passenger;
            passenger.Model.UpdateGridPosition(reservedSlot);

            _signalBus.Fire(new  PassengerEnteredWaitingAreaSignal(passenger));
        }

        private async void OnBusArrived(BusArrivedSignal signal)
        {
            _isBoardingLocked = false;

            var waitingPassengers = _slots.Where(c => c != null).ToList();
            foreach (var passenger in waitingPassengers)
            {
                if (_busSystemManager.CurrentBus == null || !_busSystemManager.CurrentBus.HasSpace()) break;
                await CheckAndBoardPassenger(passenger);
            }
        }

        private async void OnPassengerEnteredArea(PassengerEnteredWaitingAreaSignal signal)
        {
            if (!_isBoardingLocked)
            {
                await CheckAndBoardPassenger(signal.Passenger);
            }
        }

        // private async UniTask<bool> CheckAndBoardPassenger(PassengerView passenger)
        // {
        //     var currentBus = _busSystemManager.CurrentBus;
        //     if (currentBus == null) return false;
        //
        //     if (currentBus.CanBoard(passenger.PassengerColor))
        //     {
        //         // RemovePassengerFromArea(passenger);
        //         await currentBus.BoardPassengerAsync(passenger);
        //         Debug.Log($"{passenger.PassengerColor.ToString()} board");
        //         return true;
        //     }
        //
        //     return false;
        // }
        
        private async UniTask<bool> CheckAndBoardPassenger(IPassengerController passenger)
        {
            var currentBus = _busSystemManager.CurrentBus;
            if (currentBus == null) return false;

            var boarded = await _boardingSystem.TryBoard(passenger, currentBus);

            if (boarded)
            {
                RemovePassengerFromArea(passenger);
            }

            return boarded;
        }

        public int GetWaitingPassengersCount()
        {
            return _slots.Count(passenger => passenger != null);
        }

        public void Reset()
        {
            if (_slots != null)
            {
                Array.Clear(_slots, 0, _slots.Length);
            }

            _reservedSlots.Clear();
        }

        public IReadOnlyList<IPassengerController> GetWaitingPassengers()
        {
            return _slots;
        }

        public bool IsFull()
        {
            return GetWaitingPassengersCount() >= _slots.Length;
        }
    }
}