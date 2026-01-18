using System.Threading;
using _Game._Dev.Scripts.Runtime.Core.Events;
using _Game._Dev.Scripts.Runtime.Core.GameplayState;
using _Game._Dev.Scripts.Runtime.Core.Level;
using _Game._Dev.Scripts.Runtime.Features.Bus.Models;
using _Game._Dev.Scripts.Runtime.Features.Bus.Views;
using _Game._Dev.Scripts.Runtime.Utilities;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace _Game._Dev.Scripts.Runtime.Features.Bus.Controllers
{
    public class BusController : IBusController
    {
        public BusView View { get; }
        public bool IsAcceptingPassengers { get; set; }

        private readonly SignalBus _signalBus;
        private readonly BusModel _busModel;
        private readonly IGameplayStateHolder _gameplayStateHolder;

        public BusController(BusModel model, BusView view, SignalBus signalBus,
            IGameplayStateHolder gameplayStateHolder)
        {
            _busModel = model;
            View = view;
            _signalBus = signalBus;
            _gameplayStateHolder = gameplayStateHolder;

            View.SetColor(ColorMapper.GetColorFromEnum(_busModel.BusColor));
        }

        public async UniTask Initialize(Vector3 arrivalPosition, CancellationToken cancellationToken)
        {
            await View.AnimateArrival(arrivalPosition, cancellationToken);
        }

        public bool HasSpace()
        {
            return _busModel.HasSpace();
        }

        public Colors GetColor()
        {
            return _busModel.BusColor;
        }

        public bool CanBoard(Colors passengerColor)
        {
            return IsAcceptingPassengers && _busModel.HasSpace() && _busModel.IsColorMatch(passengerColor);
        }

        public int ReserveSlot()
        {
            _busModel.AddPassenger();

            if (!_busModel.HasSpace() && _gameplayStateHolder.IsGameplayActive)
            {
                _signalBus.Fire(new BusFullSignal(this));
            }

            return _busModel.CurrentPassengerCount - 1;
        }
    }
}