using _Game._Dev.Scripts.Runtime.Core.Level;

namespace _Game._Dev.Scripts.Runtime.Features.Bus.Models
{
    public class BusModel
    {
        public Colors BusColor { get; }
        public int Capacity { get; }

        private int _currentPassengerCount;
        public int CurrentPassengerCount => _currentPassengerCount;
        
        public BusModel(Colors busColor, int capacity)
        {
            BusColor = busColor;
            Capacity = capacity;
        }

        public bool HasSpace() => _currentPassengerCount < Capacity;
        public bool IsColorMatch(Colors color) => color == BusColor;
        
        public void AddPassenger()
        {
            if (!HasSpace()) return;
            _currentPassengerCount++;
        }
    }
}