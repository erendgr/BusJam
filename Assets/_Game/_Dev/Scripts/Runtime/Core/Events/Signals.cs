using _Game._Dev.Scripts.Runtime.Features.Bus.Controllers;
using _Game._Dev.Scripts.Runtime.Features.Passenger.Controllers;
using _Game._Dev.Scripts.Runtime.Features.Passenger.Views;
using _Game._Dev.Scripts.Runtime.Level.Models;

namespace _Game._Dev.Scripts.Runtime.Core.Events
{
    public struct RestartLevelRequestedSignal{ }

    public struct StartGameRequestedSignal { }

    public struct NextLevelRequestedSignal { }

    public struct LevelReadySignal
    {
        public readonly LevelSO LevelData;
        public LevelReadySignal(LevelSO levelData) => LevelData = levelData;
    }

    public struct TimerUpdatedSignal
    {
        public readonly float RemainingTime;
        public TimerUpdatedSignal(float remainingTime) => RemainingTime = remainingTime;
    }

    public struct LevelCompletedSignal { }

    public struct LevelLoadRequestedSignal
    {
        public readonly LevelSO LevelData;
        public LevelLoadRequestedSignal(LevelSO levelData) => LevelData = levelData;
    }
    
    public struct ResetGameplaySignal { }
    
    public struct PassengerClickedSignal
    {
        public readonly PassengerView ClickedPassenger;
        public PassengerClickedSignal(PassengerView clickedPassenger) => ClickedPassenger = clickedPassenger;
    }
    
    public struct BusFullSignal
    {
        public readonly IBusController FullBus;
        public BusFullSignal(IBusController bus) => FullBus = bus;
    }

    public struct AllBusesDispatchedSignal { }

    public struct BusArrivalSequenceStartedSignal 
    {
        public readonly IBusController ArrivingBus;
        public BusArrivalSequenceStartedSignal(IBusController bus) => ArrivingBus = bus;
    }

    public struct BusArrivedSignal
    {
        public readonly IBusController ArrivedBus;
        public BusArrivedSignal(IBusController bus) => ArrivedBus = bus;
    }

    public struct GameOverSignal { }

    public struct TimeIsUpSignal { }

    public struct PassengerEnteredWaitingAreaSignal
    {
        public readonly IPassengerController Passenger;
        public PassengerEnteredWaitingAreaSignal(IPassengerController passenger) => Passenger = passenger;
    }
    
    public struct WaitingAreaChangedSignal { }

    public struct LevelCompleteSequenceFinishedSignal { }
}