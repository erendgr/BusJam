using System;
using System.Collections.Generic;
using _Game._Dev.Scripts.Runtime.Core.Events;
using _Game._Dev.Scripts.Runtime.Core.Grid;
using _Game._Dev.Scripts.Runtime.Core.Pathfinding;
using _Game._Dev.Scripts.Runtime.Misc;
using _Game._Dev.Scripts.Runtime.MVC.Passenger.Views;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace _Game._Dev.Scripts.Runtime.MVC.Passenger.Controllers
{
    public class PassengerMovementController : IInitializable, IDisposable
    {
        private readonly SignalBus _signalBus;
        private readonly IGridSystemManager _gridSystemManager;
        private readonly IWaitingAreaController _waitingAreaController;
        private readonly IGameplayStateHolder _gameplayStateHolder;
        private readonly IPathfindingService _pathfindingService;

        public PassengerMovementController(SignalBus signalBus, IGridSystemManager gridSystemManager,
            IWaitingAreaController waitingAreaController, IGameplayStateHolder gameplayStateHolder,
            IPathfindingService pathfindingService)
        {
            _signalBus = signalBus;
            _gridSystemManager = gridSystemManager;
            _waitingAreaController = waitingAreaController;
            _gameplayStateHolder = gameplayStateHolder;
            _pathfindingService = pathfindingService;
        }
        
        public void Initialize() => _signalBus.Subscribe<CharacterClickedSignal>(OnCharacterClicked);
        public void Dispose() => _signalBus.TryUnsubscribe<CharacterClickedSignal>(OnCharacterClicked);

        private void OnCharacterClicked(CharacterClickedSignal signal)
        {
            if (!_gameplayStateHolder.IsGameplayActive) return;

            HandleCharacterMovement(signal.ClickedPassenger).Forget();
        }

        private async UniTask HandleCharacterMovement(PassengerView passenger)
        {
            if (passenger.IsMoving) return;

            var path = FindPathToExit(passenger, _gridSystemManager.MainGrid);
            if (path == null) return;
            
            var reservedSlot = _waitingAreaController.ReserveNextAvailableSlot();
            if (reservedSlot == null) return;

            passenger.IsMoving = true;
            
            _gridSystemManager.MainGrid.ClearCell(passenger.GridPosition);
            await passenger.MoveAlongPath(path);
            await _waitingAreaController.FinalizeMoveToSlot(passenger, reservedSlot.Value);
            
            passenger.IsMoving = false;
        }
        
        private List<Vector2Int> FindPathToExit(PassengerView passenger, IGrid grid)
        {
            var exitPoints = GetExitPoints(grid, passenger);
            if (exitPoints.Count == 0)
            {
                return null;
            }

            List<Vector2Int> shortestPath = null;
            foreach (var exitPoint in exitPoints)
            {
                var foundPath = _pathfindingService.FindPath(grid, passenger.GridPosition, exitPoint);
                if (foundPath != null && (shortestPath == null || foundPath.Count < shortestPath.Count))
                {
                    shortestPath = foundPath;
                }
            }

            return (shortestPath != null && shortestPath.Count >= 1) ? shortestPath : null;
        }


        private List<Vector2Int> GetExitPoints(IGrid grid, PassengerView passengerToMove)
        {
            var exits = new List<Vector2Int>();
            int topRowIndex = grid.Height - 1;

            for (int x = 0; x < grid.Width; x++)
            {
                var exitPos = new Vector2Int(x, topRowIndex);
                var objectAtExit = grid.GetObjectAt(exitPos);

                if (objectAtExit == null || objectAtExit == passengerToMove.gameObject)
                {
                    exits.Add(exitPos);
                }
            }
            return exits;
        }
    }
}