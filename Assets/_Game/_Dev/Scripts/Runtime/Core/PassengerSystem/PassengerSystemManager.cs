using System.Collections.Generic;
using _Game._Dev.Scripts.Runtime.Features.Passenger.Controllers;
using _Game._Dev.Scripts.Runtime.Features.Passenger.Views;

namespace _Game._Dev.Scripts.Runtime.Core.PassengerSystem
{
    public class PassengerSystemManager : IPassengerSystemManager
    {
        private readonly List<IPassengerController> _passengers = new();

        public void Register(IPassengerController passenger)
        {
            _passengers.Add(passenger);
        }

        public IPassengerController GetByView(PassengerView view)
        {
            for (int i = 0; i < _passengers.Count; i++)
            {
                if (_passengers[i].View == view)
                    return _passengers[i];
            }
            return null;
        }
    }

}