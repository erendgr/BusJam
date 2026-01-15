using _Game._Dev.Scripts.Runtime.Core.Events;
using _Game._Dev.Scripts.Runtime.Core.Factories;
using _Game._Dev.Scripts.Runtime.Core.Grid;
using _Game._Dev.Scripts.Runtime.Level.Models;
using UnityEngine;
using Zenject;

namespace _Game._Dev.Scripts.Runtime.Level.Controllers
{
    public class LevelController : ILevelController
    {
        private readonly SignalBus _signalBus;
        private readonly IGridSystemManager _gridSystemManager;
        private readonly IPassengerFactory _passengerFactory;
        private readonly IObstacleFactory _obstacleFactory;
        
        public LevelController(SignalBus signalBus, IGridSystemManager gridSystemManager, IPassengerFactory passengerFactory, IObstacleFactory obstacleFactory)
        {
            _signalBus = signalBus;
            _gridSystemManager = gridSystemManager;
            _passengerFactory = passengerFactory;
            _obstacleFactory = obstacleFactory;
            
            _signalBus.Subscribe<LevelLoadRequestedSignal>(OnLevelLoadRequested);
        }

        private void OnLevelLoadRequested(LevelLoadRequestedSignal signal)
        {
            LoadLevel(signal.LevelData);
        }

        public void LoadLevel(LevelSO levelData)
        {
            _gridSystemManager.CreateGrids(levelData);
            
            Debug.Log($"Loading Level: {levelData.name}");
    
            var mainGrid = _gridSystemManager.MainGrid;

            foreach (var obstacleData in levelData.Obstacles)
            {
                var obstaclePosition = mainGrid.GetWorldPosition(obstacleData.GridPosition);
                var obstacleInstance = _obstacleFactory.Create(obstaclePosition);
                mainGrid.PlaceObject(obstacleInstance, obstacleData.GridPosition);
            }
    
            foreach (var passengerData in levelData.Passengers)
            {
                var passengerPosition = mainGrid.GetWorldPosition(passengerData.GridPosition, 0.5f);
                var passengerController = _passengerFactory.Create(passengerData.Color, passengerPosition, passengerData.GridPosition);
                mainGrid.PlaceObject(passengerController.View.gameObject, passengerData.GridPosition);
            }

            Debug.Log("Level Loaded Successfully. Firing LevelReadySignal.");
            _signalBus.Fire(new LevelReadySignal(levelData));
        }
    }
}