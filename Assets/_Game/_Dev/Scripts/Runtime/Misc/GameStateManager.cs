using System;
using System.Linq;
using _Game._Dev.Scripts.Runtime.Core.BusSystem;
using _Game._Dev.Scripts.Runtime.Core.Events;
using _Game._Dev.Scripts.Runtime.Core.Grid;
using _Game._Dev.Scripts.Runtime.Core.Movement;
using _Game._Dev.Scripts.Runtime.Features.Passenger.Views;
using UnityEngine;
using Zenject;

namespace _Game._Dev.Scripts.Runtime.Misc
{
    public class GameStateManager : IInitializable, IDisposable
    {
        private readonly SignalBus _signalBus;
        private readonly IGridSystemManager _gridSystemManager;
        private readonly IBusSystemManager _busSystemManager;
        private readonly IWaitingAreaController _waitingAreaController;
        private readonly IMovementTracker _movementTracker;
        private readonly IGameplayStateHolder _gameplayStateHolder;
        private readonly ITimerController _timerController;
        
        private bool _isLevelWon;
        
        public GameStateManager(SignalBus signalBus, IGridSystemManager gridSystemManager, 
            IWaitingAreaController waitingAreaController, IMovementTracker movementTracker, 
            IBusSystemManager busSystemManager, IGameplayStateHolder gameplayStateHolder, 
            ITimerController timerController)
        {
            _signalBus = signalBus;
            _gridSystemManager = gridSystemManager;
            _waitingAreaController = waitingAreaController;
            _movementTracker = movementTracker;
            _busSystemManager = busSystemManager;
            _gameplayStateHolder = gameplayStateHolder;
            _timerController = timerController;
        }

        public void Initialize()
        {
            _signalBus.Subscribe<AllBusesDispatchedSignal>(OnAllBusesDispatched);
            _signalBus.Subscribe<RestartLevelRequestedSignal>(OnNewLevelSequenceRequested);
            _signalBus.Subscribe<NextLevelRequestedSignal>(OnNewLevelSequenceRequested);
            _signalBus.Subscribe<BusArrivedSignal>(OnBusArrived);
            _signalBus.Subscribe<TimeIsUpSignal>(OnTimeIsUp);
            _signalBus.Subscribe<PassengerEnteredWaitingAreaSignal>(OnPassengerEnteredWaitingArea);
        }

        public void Dispose()
        {
            _signalBus.TryUnsubscribe<AllBusesDispatchedSignal>(OnAllBusesDispatched);
            _signalBus.TryUnsubscribe<RestartLevelRequestedSignal>(OnNewLevelSequenceRequested);
            _signalBus.TryUnsubscribe<NextLevelRequestedSignal>(OnNewLevelSequenceRequested);
            _signalBus.TryUnsubscribe<BusArrivedSignal>(OnBusArrived);
            _signalBus.TryUnsubscribe<TimeIsUpSignal>(OnTimeIsUp);
            _signalBus.TryUnsubscribe<PassengerEnteredWaitingAreaSignal>(OnPassengerEnteredWaitingArea);
        }
        
        private void OnTimeIsUp() => ProcessGameOverState("Time is up!");
        private void OnPassengerEnteredWaitingArea() => CheckForWaitingAreaDeadlock();
        private void OnBusArrived() => CheckForWaitingAreaDeadlock();

        private void OnAllBusesDispatched()
        {
            if (_isLevelWon) return;

            CheckWinCondition();
        }

        private void CheckWinCondition()
        {
            var allGridObjects = _gridSystemManager.MainGrid.GetAllOccupiedObjects();
            var passengerCountOnGrid = allGridObjects.Count(obj => obj.GetComponent<PassengerView>() != null);
            var waitingAreaCount = _waitingAreaController.GetWaitingPassengersCount();
    
            if (passengerCountOnGrid == 0 && waitingAreaCount == 0)
            {
                ProcessWinState();
            }
            else
            {
                CheckForStuckPassengers();
            }
        }
        
        private void CheckForStuckPassengers()
        {
            if (!_gameplayStateHolder.IsGameplayActive) return;

            var allGridObjects = _gridSystemManager.MainGrid.GetAllOccupiedObjects();
            var passengerCountOnGrid = allGridObjects.Count(obj => obj.GetComponent<PassengerView>() != null);
            var waitingAreaCount = _waitingAreaController.GetWaitingPassengersCount();

            if (passengerCountOnGrid > 0 || waitingAreaCount > 0)
            {
                ProcessGameOverState("Game ended with stuck passengers.");
            }
        }
        
        private void CheckForWaitingAreaDeadlock()
        {
            if (!_gameplayStateHolder.IsGameplayActive || _busSystemManager.IsBusInTransition || !_waitingAreaController.IsFull())
            {
                return;
            }

            var currentBus = _busSystemManager.CurrentBus;
            if (currentBus == null)
            {
                if (_waitingAreaController.GetWaitingPassengersCount() > 0)
                {
                    ProcessGameOverState("No more buses available, but passengers are waiting.");
                }
                return;
            }

            var busColor = currentBus.GetColor();
            bool canAnyoneBoard = _waitingAreaController.GetWaitingPassengers().Any(c => c != null && c.PassengerColor == busColor);

            if (!canAnyoneBoard)
            {
                ProcessGameOverState($"Deadlock: No matching passenger for the {busColor} bus.");
            }
        }
        
        private void ProcessGameOverState(string reason)
        {
            if (!_gameplayStateHolder.IsGameplayActive) return;
            
            _gameplayStateHolder.Pause();
            _timerController.Stop();
            
            _signalBus.Fire<GameOverSignal>();
            Debug.Log(reason);
        }
        
        private void ProcessWinState()
        {
            if (!_gameplayStateHolder.IsGameplayActive) return;

            _gameplayStateHolder.Pause();
            _timerController.Stop();
            _isLevelWon = true;
            _signalBus.Fire<LevelCompletedSignal>();
        }
        
        private void OnNewLevelSequenceRequested()
        {
            _gameplayStateHolder.Resume();
            _movementTracker.Reset();
            _isLevelWon = false;
        }
    }
}