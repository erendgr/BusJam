using System;
using System.Collections.Generic;
using _Game._Dev.Scripts.Runtime.Core.Events;
using _Game._Dev.Scripts.Runtime.Core.Grid;
using _Game._Dev.Scripts.Runtime.Core.PassengerSystem;
using _Game._Dev.Scripts.Runtime.Core.Pathfinding;
using _Game._Dev.Scripts.Runtime.Features.Passenger.Models;
using _Game._Dev.Scripts.Runtime.Features.Passenger.Views;
using _Game._Dev.Scripts.Runtime.Misc;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace _Game._Dev.Scripts.Runtime.Features.Passenger.Controllers
{
    public class PassengerMovementController : IInitializable, IDisposable
    {
        private readonly SignalBus _signalBus;
        private readonly IPassengerSystemManager _passengerSystemManager;
        private readonly IGridSystemManager _gridSystemManager;
        private readonly IWaitingAreaController _waitingAreaController;
        private readonly IGameplayStateHolder _gameplayStateHolder;
        private readonly IPathfindingService _pathfindingService;

        public PassengerMovementController(SignalBus signalBus, IPassengerSystemManager passengerSystemManager, IGridSystemManager gridSystemManager,
            IWaitingAreaController waitingAreaController, IGameplayStateHolder gameplayStateHolder, IPathfindingService pathfindingService)
        {
            _signalBus = signalBus;
            _passengerSystemManager = passengerSystemManager;
            _gridSystemManager = gridSystemManager;
            _waitingAreaController = waitingAreaController;
            _gameplayStateHolder = gameplayStateHolder;
            _pathfindingService = pathfindingService;
        }
        
        public void Initialize() => _signalBus.Subscribe<PassengerClickedSignal>(OnCharacterClicked);
        public void Dispose() => _signalBus.TryUnsubscribe<PassengerClickedSignal>(OnCharacterClicked);
        
        private void OnCharacterClicked(PassengerClickedSignal signal)
        {
            if (!_gameplayStateHolder.IsGameplayActive) return;

            var passenger = _passengerSystemManager.GetByView(signal.ClickedPassenger);
            if (passenger == null) return;

            HandleCharacterMovement(passenger).Forget();
        }

        private async UniTask HandleCharacterMovement(IPassengerController passenger)
        {
            var model = passenger.Model;
            var view = passenger.View;
            
            if (model.IsMoving) return;

            var path = FindPathToExit(view, model, _gridSystemManager.MainGrid);
            if (path == null) return;
            
            var reservedSlot = _waitingAreaController.ReserveNextAvailableSlot();
            if (reservedSlot == null) return;

            model.IsMoving = true;
            view.SetInteractionEnabled(false);
            
            _gridSystemManager.MainGrid.ClearCell(model.GridPosition);
            await view.MoveAlongPath(path);
            await _waitingAreaController.FinalizeMoveToSlot(view, model, reservedSlot.Value);
            
            model.IsMoving = false;
            view.SetInteractionEnabled(true);
        }
        
        private List<Vector2Int> FindPathToExit(PassengerView passengerView, PassengerModel passengerModel, IGrid grid)
        {
            var exitPoints = GetExitPoints(grid, passengerView);
            if (exitPoints.Count == 0)
            {
                return null;
            }

            List<Vector2Int> shortestPath = null;
            foreach (var exitPoint in exitPoints)
            {
                var foundPath = _pathfindingService.FindPath(grid, passengerModel.GridPosition, exitPoint);
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