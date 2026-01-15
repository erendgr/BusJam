using _Game._Dev.Scripts.Runtime.Features.Passenger.Controllers;
using _Game._Dev.Scripts.Runtime.Features.Passenger.Views;

namespace _Game._Dev.Scripts.Runtime.Core.PassengerSystem
{
    public interface IPassengerSystemManager
    {
        void Register(IPassengerController passenger);
        IPassengerController GetByView(PassengerView view);
    }
}