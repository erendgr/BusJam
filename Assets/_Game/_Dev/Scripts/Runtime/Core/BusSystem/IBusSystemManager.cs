using _Game._Dev.Scripts.Runtime.Features.Bus.Controllers;

namespace _Game._Dev.Scripts.Runtime.Core.BusSystem
{
    public interface IBusSystemManager
    {
        IBusController CurrentBus { get; }
        bool IsBusInTransition { get; } 
        void Reset();
    }
}