using System;
using _Game._Dev.Scripts.Runtime.Core.Events;
using _Game._Dev.Scripts.Runtime.Level.Models;
using UnityEngine;
using Zenject;

namespace _Game._Dev.Scripts.Runtime.Core.Grid
{
    public class GridSystemManager : IGridSystemManager, IInitializable, IDisposable
    {
        private readonly SignalBus _signalBus;
        private readonly GridConfigurationSO _config;
        private readonly DiContainer _container;
        
        public IGrid MainGrid { get; private set; }
        public IGrid WaitingAreaGrid { get; private set; }
        
        public GridSystemManager(SignalBus signalBus, DiContainer container, GridConfigurationSO config)
        {
            _signalBus = signalBus;
            _container = container;
            _config = config;
        }

        public void Initialize() => _signalBus.Subscribe<ResetGameplaySignal>(Reset);
        
        public void Dispose() => _signalBus.TryUnsubscribe<ResetGameplaySignal>(Reset);

        public void CreateGrids(LevelSO levelData)
        {
            var mainGridOrigin = Vector3.zero;
            MainGrid = new Grid(levelData.MainGridSize.x, levelData.MainGridSize.y, mainGridOrigin,
                _config.MainGridTilePrefab, _container);

            float mainGridWidth = levelData.MainGridSize.x;
            float waitingAreaWidth = _config.WaitingGridSize.x;
            float xOffset = (mainGridWidth - waitingAreaWidth) / 2f;

            float zOffset = levelData.MainGridSize.y + _config.SpacingBetweenGrids;
            var waitingAreaOrigin = mainGridOrigin + new Vector3(xOffset, 0, zOffset);
    
            WaitingAreaGrid = new Grid(_config.WaitingGridSize.x, _config.WaitingGridSize.y, waitingAreaOrigin,
                _config.WaitingAreaTilePrefab, _container);
        }

        public void Reset()
        {
            MainGrid?.ClearAllCells();
            WaitingAreaGrid?.ClearAllCells();

            MainGrid = null;
            WaitingAreaGrid = null;
        }
    }
}