using System.Threading;
using _Game._Dev.Scripts.Runtime.Core.Events;
using _Game._Dev.Scripts.Runtime.Features.Passenger.Views;
using _Game._Dev.Scripts.Runtime.Level.Models;
using _Game._Dev.Scripts.Runtime.Misc;
using _Game._Dev.Scripts.Runtime.MVC.Bus.Models;
using _Game._Dev.Scripts.Runtime.MVC.Bus.Views;
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

        public BusController(BusModel model, BusView view, SignalBus signalBus, IGameplayStateHolder gameplayStateHolder)
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
        
        public bool CanBoard(PassengerView passenger)
        {
            return IsAcceptingPassengers && _busModel.HasSpace() && _busModel.IsColorMatch(passenger.PassengerColor);
        }
        
        public async UniTask BoardPassengerAsync(PassengerView passenger)
        {
            var slotIndex = _busModel.Passengers.Count;
            var slotTransform = View.GetSlotTransform(slotIndex);

            _busModel.AddPassenger(passenger);
            
            try
            {
                await passenger.MoveToPoint(slotTransform.position);
            }
            catch (System.OperationCanceledException)
            {
                // do nothing because the game is already over
            }
            
            if (passenger != null)
            {
                passenger.transform.SetParent(slotTransform);
            }

            if (!_busModel.HasSpace() && _gameplayStateHolder.IsGameplayActive)
            {
                _signalBus.Fire(new BusFullSignal(this));
            }
        }
    }
}