using System.Collections.Generic;
using _Game._Dev.Scripts.Runtime.Level.Models;
using _Game._Dev.Scripts.Runtime.MVC.Passenger.Views;

namespace _Game._Dev.Scripts.Runtime.MVC.Bus.Models
{
    public class BusModel
    {
        public Colors BusColor { get; }
        public int Capacity { get; }
        public IReadOnlyList<PassengerView> Passengers => _passengers;
        
        private readonly List<PassengerView> _passengers;

        public BusModel(Colors busColor, int capacity)
        {
            BusColor = busColor;
            Capacity = capacity;
            _passengers = new List<PassengerView>(capacity);
        }

        public bool HasSpace() => _passengers.Count < Capacity;
        public bool IsColorMatch(Colors color) => color == BusColor;
        
        public void AddPassenger(PassengerView passenger)
        {
            if (HasSpace())
            {
                _passengers.Add(passenger);
            }
        }
    }
}